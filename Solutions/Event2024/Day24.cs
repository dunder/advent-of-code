using MoreLinq;
using Newtonsoft.Json.Linq;
using Shared.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    public delegate bool Op(bool input1, bool input2);

    public enum Operation { AND, OR, XOR }

    public record Wire(string Name)
    {
        public bool? Signal { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Signal})";
        }
    }

    public record Gate(Wire In1, Wire In2, Wire Out, Operation Operation)
    {
        public bool Connected => In1.Signal.HasValue && In2.Signal.HasValue;

        public void TryConnect()
        {
            if (Connected)
            {
                Out.Signal = Operation switch
                {
                    Operation.AND => In1.Signal.Value && In2.Signal.Value,
                    Operation.OR => In1.Signal.Value || In2.Signal.Value,
                    Operation.XOR => In1.Signal.Value != In2.Signal.Value,
                    _ => throw new InvalidOperationException($"{Operation}")
                };
            }
        }

        public override string ToString()
        {
            return $"{In1} {Operation} {In2} -> {Out}";
        }
    }

    // --- Day 24: Crossed Wires ---
    public class Day24
    {
        private readonly ITestOutputHelper output;

        public Day24(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static List<Gate> Parse(IList<string> input)
        {
            Dictionary<string, Wire> wires = new();

            List<IEnumerable<string>> result = input.Split("").ToList();

            foreach (var line in result.First())
            {
                var parts = line.Split(": ");
                var port = parts[0];
                var signal = parts[1] == "1";

                wires.Add(port, new Wire(port) { Signal = signal });
            }

            List<Gate> gates = new();

            foreach (var line in result.Last())
            {
                var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var port1 = parts[0];
                var operation = parts[1] switch { "AND" => Operation.AND, "OR" => Operation.OR, "XOR" => Operation.XOR };
                var port2 = parts[2];
                var outputPort = parts[4];

                var input1 = new Wire(port1) { Signal = null };
                var input2 = new Wire(port2) { Signal = null };
                var output = new Wire(outputPort) { Signal = null };

                if (wires.TryGetValue(port1, out Wire input1Wire))
                {
                    input1 = input1Wire;
                }
                else
                {
                    wires.Add(input1.Name, input1);
                }

                if (wires.TryGetValue(port2, out Wire input2Wire))
                {
                    input2 = input2Wire;
                }
                else
                {
                    wires.Add(input2.Name, input2);
                }

                if (wires.TryGetValue(outputPort, out Wire outputWire))
                {
                    output = outputWire;
                }
                else
                {
                    wires.Add(output.Name, output);
                }
                
                gates.Add(new Gate(input1, input2, output, operation));
            }

            return gates;
        }

        private static string Output(List<Gate> gates, string prefix)
        {
            Gate gateWithoutOutputSignal = gates.FirstOrDefault(c => c.Out.Signal == null);

            if (gateWithoutOutputSignal != null)
            {
                throw new InvalidOperationException($"Insufficient input: {gateWithoutOutputSignal.Out.Name}");
            }

            var binary = gates
                .Where(gate => gate.Out.Name.StartsWith(prefix))
                .OrderByDescending(gate => gate.Out.Name)
                .Select(gate => gate.Out.Signal.Value ? "1" : "0");

            return string.Join("", binary);

        }
        private static long OutputValue(List<Gate> gates, string prefix)
        {
            return Convert.ToInt64(Output(gates, prefix), 2);
        }

        private static List<Wire> InputWires(List<Gate> gates, string prefix)
        {
            return gates
                .Select(gate => new List<Wire> { gate.In1, gate.In2 })
                .SelectMany(x => x)
                .Where(wire => wire.Name.StartsWith(prefix))
                .Distinct()
                .OrderByDescending(wire => wire.Name)
                .ToList();
        }

        private static long InputValue(List<Gate> gates, string prefix)
        {
            string binary = string.Join("", Input(gates, prefix));
            return Convert.ToInt64(binary, 2);
        }

        private static string Input(List<Gate> gates, string prefix)
        {
            Gate gateWithoutOutputSignal = gates.FirstOrDefault(c => c.Out.Signal == null);

            if (gateWithoutOutputSignal != null)
            {
                throw new InvalidOperationException($"Insufficient input: {gateWithoutOutputSignal.Out.Name}");
            }

            var inputWireSignalValues = InputWires(gates, prefix).Select(wire => wire.Signal.Value ? "1" : "0");


            return string.Join("", inputWireSignalValues);
        }

        private static List<Gate> Connect(List<Gate> gates)
        {
            List<Gate> unconnected = gates.Where(gate => !gate.Out.Signal.HasValue).ToList();

            while (unconnected.Count > 0)
            {
                foreach (var gate in unconnected)
                {
                    gate.TryConnect();
                }

                unconnected = unconnected.Where(c => !c.Out.Signal.HasValue).ToList();
            }

            return gates;
        }

        private static long Problem1(IList<string> input)
        {
            List<Gate> gates = Parse(input);

            gates = Connect(gates);

            return OutputValue(gates, "z");
        }

        private static List<Gate> Reset(List<Gate> gates, long x, long y)
        {
            foreach (var gate in gates)
            {
                gate.In1.Signal = null;
                gate.In2.Signal = null;
                gate.Out.Signal = null;
            }

            List<Wire> xwires = InputWires(gates, "x");

            var xbinary = Convert.ToString(x, 2).PadLeft(xwires.Count, '0');

            for (int i = 0; i < xwires.Count; i++)
            {
                xwires[i].Signal = xbinary[i] == '1';
            }

            List<Wire> ywires = InputWires(gates, "y");

            var ybinary = Convert.ToString(y, 2).PadLeft(ywires.Count, '0');

            for (int i = 0; i < ywires.Count; i++)
            {
                ywires[i].Signal = ybinary[i] == '1';
            }


            return gates;
        }

        private static Gate LookupOutputGate(List<Gate> gates, int i)
        {
            var name = $"z{i:00}";
            return gates.Single(gate => gate.Out.Name == name);
        }

        private static List<Gate> ConnectedGates(Gate gate, List<Gate> gates, List<Gate> connected)
        {

            List<Gate> in1Gates = gates.Where(g => g.Out.Name == gate.In1.Name).ToList();
            
            foreach (var g1 in in1Gates)
            {
                connected.Add(g1);
                ConnectedGates(g1, gates, connected);
            }
            
            List<Gate> in2Gates = gates.Where(g => g.Out.Name == gate.In2.Name).ToList();


            foreach (var g2 in in2Gates)
            {
                connected.Add(g2);
                ConnectedGates(g2, gates, connected);
            }

            return connected;
        }

        private static List<Wire> ConnectedWires(Wire wire, List<Gate> gates, List<Wire> connected)
        {
            List<Gate> connectedGates = gates.Where(g => g.Out.Name == wire.Name).ToList();
            
            foreach (var gate in connectedGates)
            {
                connected.Add(gate.In1);
                connected.Add(gate.In2);
                ConnectedWires(gate.In1, gates, connected);
                ConnectedWires(gate.In2, gates, connected);
            }

            return connected;
        }

        private static List<Gate> Swap(string firstName, string secondName, List<Gate> gates)
        {
            var first = gates.Where(gate => gate.Out.Name == firstName).Single();
            var second = gates.Where(gate => gate.Out.Name == secondName).Single();

            return Swap(first.Out, second.Out, gates);
        }

        private static List<Gate> Swap(Wire first, Wire second, List<Gate> gates)
        {
            return gates.Select(gate =>
            {
                //var input1 = gate.In1;
                //var input2 = gate.In2;
                var output = gate.Out;

                if (gate.Out.Name == first.Name)
                {
                    output = second;
                }

                if (gate.Out.Name == second.Name)
                {
                    output = first;
                }

                //if (gate.In1.Name == first.Name)
                //{
                //    input1 = second;
                //}

                //if (gate.In2.Name == first.Name)
                //{
                //    input2 = second;
                //}

                //if (gate.In1.Name == second.Name)
                //{
                //    input1 = first;
                //}

                //if (gate.In2.Name == second.Name)
                //{
                //    input2 = first;
                //}

                return gate with { Out = output };
            })
            .ToList();
        }

        private static (bool correct, string x, string y, string expected, string actual) TryAdd(List<Gate> gates, long a, long b)
        {
            var inputWidth = InputWires(gates, "x").Count;
            var outputWidth = inputWidth + 1;

            gates = Reset(gates, a, b);
            gates = Connect(gates);

            var z = Output(gates, "z");
            var zValue = OutputValue(gates, "z");

            var x = InputValue(gates, "x");
            var y = InputValue(gates, "y");

            long actual = zValue;
            long expected = x + y;

            bool correct = actual == expected;

            string expectedZ = Convert.ToString(expected, 2).PadLeft(outputWidth, '0');

            return (correct, Input(gates, "x"), Input(gates, "y"), expectedZ, z);
        }

        private static List<(bool correct, string a, string b, string expected, string actual)> TryAddMany(List<Gate> gates)
        {
            var inputWidth = InputWires(gates, "x").Count;
            var outputWidth = inputWidth + 1;

            List<(bool correct, string a, string b, string expected, string actual)> result = new();

            for (var i = 0; i < inputWidth; i++)
            {
                long value = (long)(Math.Pow(2, i));
                long a = value-1;
                long b = value;
                (bool correct, string x, string y, string expected, string actual) = TryAdd(gates, a, b);

                result.Add((correct, x, y, expected, actual));
            }

            return result;
        }

        private static string Problem2(IList<string> input)
        {
            List<Gate> gates = Parse(input);

            List<(string first, string second)> swaps = [("z10", "mkk"), ("z14", "qbw"), ("z34", "wcb"), ("wjb", "cvp")];

            foreach (var swap in swaps)
            {
                gates = Swap(swap.first, swap.second, gates);
            }

            //gates = Swap("z14", "qbw", gates);
            //gates = Swap("z34", "wcb", gates);
            //gates = Swap("wjb", "cvp", gates);

            var outputGates = gates.Where(gate => gate.Out.Name.StartsWith("z")).OrderBy(gate => gate.Out.Name).ToList();

            var faultyOuputGates = outputGates.Where(gate => gate.Operation != Operation.XOR).ToList();

            // z10, z14, z34, z45
            // mkk, wcb, qbw

            var faultyXorGates = gates
                .Where(gate => gate.Operation == Operation.XOR)
                .Where(gate => !gate.In1.Name.StartsWith("x") && !gate.In1.Name.StartsWith("y"))
                .Where(gate => !gate.In2.Name.StartsWith("x") && !gate.In2.Name.StartsWith("y"))
                .Where(gate => !gate.Out.Name.StartsWith("z"))
                .ToList();

            var mkkGates = gates.Where(gate => gate.In1.Name == "mkk" || gate.In2.Name == "mkk").ToList();
            var wcbGates = gates.Where(gate => gate.In1.Name == "wcb" || gate.In2.Name == "wcb").ToList();
            var qbwGates = gates.Where(gate => gate.In1.Name == "qbw" || gate.In2.Name == "qbw").ToList();
            

            var xorGates = gates.Where(gate => gate.Operation == Operation.XOR).OrderBy(gate => gate.In1.Name).ThenBy(gate => gate.In2.Name).ToList();
            var andGates = gates.Where(gate => gate.Operation == Operation.AND).OrderBy(gate => gate.In1.Name).ThenBy(gate => gate.In2.Name).ToList();
            var orGates = gates.Where(gate => gate.Operation == Operation.OR).OrderBy(gate => gate.In1.Name).ThenBy(gate => gate.In2.Name).ToList();

            var z11 = gates.Where(gate => gate.Out.Name.StartsWith("z11")).ToList();
            var cbv = gates.Where(gate => gate.Out.Name.StartsWith("cbv")).ToList();
            var bmt = gates.Where(gate => gate.Out.Name.StartsWith("bmt")).ToList();

            var dkk = gates.Where(gate => gate.Out.Name.StartsWith("dkk")).ToList();
            var tqj = gates.Where(gate => gate.Out.Name.StartsWith("tqj")).ToList();
            var fhf = gates.Where(gate => gate.Out.Name.StartsWith("fhf")).ToList();
            var btf = gates.Where(gate => gate.Out.Name.StartsWith("btf")).ToList();

            var x9A = gates.Where(gate => gate.In1.Name.StartsWith("x09") || gate.In2.Name.StartsWith("x09")).ToList();

            var mqk = gates.Where(gate => gate.In1.Name.StartsWith("mqk") || gate.In2.Name.StartsWith("mqk")).ToList();
            var vbs = gates.Where(gate => gate.Out.Name.StartsWith("vbs")).ToList();
            var nvv = gates.Where(gate => gate.Out.Name.StartsWith("nvv")).ToList();
            var kvd = gates.Where(gate => gate.Out.Name.StartsWith("kvd")).ToList();

            var z25 = gates.Where(gate => gate.Out.Name.StartsWith("z25")).ToList();
            var xy25 = gates.Where(gate => gate.In1.Name.StartsWith("x25") || gate.In2.Name.StartsWith("x25")).ToList();

            var wjb = gates.Where(gate => gate.In1.Name.StartsWith("wjb") || gate.In2.Name.StartsWith("wjb")).ToList();
            var cvp = gates.Where(gate => gate.In1.Name.StartsWith("cvp") || gate.In2.Name.StartsWith("cvp")).ToList();

           

            var andGatesWithoutXorGate = andGates.Where(andGate =>
            
                !xorGates.Any(xorGate => 
                    (xorGate.In1.Name == andGate.In1.Name && 
                    xorGate.In2.Name == andGate.In2.Name )|| 
                    (xorGate.In1.Name == andGate.In2.Name &&
                    xorGate.In2.Name == andGate.In1.Name)
                    )
            ).ToList();

            var potentialSwapGates = faultyOuputGates.Select(gate => gate.Out).ToList();
            potentialSwapGates.AddRange(faultyXorGates.Select(gate => gate.Out));

            gates = Connect(gates);

            // qbw, mkk, wcb

            

            var z = Output(gates, "z");
            var x = InputValue(gates, "x");
            var y = InputValue(gates, "y");

            var width = InputWires(gates, "x").Count;

            var wires = gates.Select(gate => gate.Out).Distinct().ToList();

            //gates = Swap(z10.Out, z11.Out, gates);

            var result = TryAddMany(gates);


            long value = (long)(Math.Pow(2, 12));
            long a = value - 1;
            long b = value;

            var added = TryAdd(gates, a, b);

            var c11 = ConnectedGates(outputGates[11], gates, [outputGates[11]]);


            string answer = string.Join(",", swaps.Select(swap => new List<string> { swap.first, swap.second }).SelectMany(x => x).OrderBy(x => x));
            return answer;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(36902370467952, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal("", Problem2(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(2024, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal("", Problem2(exampleInput));
        }
    }
}

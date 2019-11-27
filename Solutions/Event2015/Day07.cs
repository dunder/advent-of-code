using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 7: Some Assembly Required ---
    public class Day07
    {
        public class Circuit
        {
            private readonly IDictionary<string, Wire> wires;
            private readonly List<IGate> gates;


            public Circuit()
            {
                wires = new Dictionary<string, Wire>();
                gates = new List<IGate>();
            }


            public void Evaluate()
            {
                while (gates.Any(g => !g.Out.Signal.HasValue))
                {
                    foreach (var gate in gates)
                    {
                        gate.Evaluate();
                    }
                }
            }

            public ushort GetSignal(string wireId)
            {
                if (!wires.ContainsKey(wireId))
                {
                    throw new ArgumentOutOfRangeException($"This circuit has no such wire: {wireId}");
                }
                var signal = wires[wireId].Signal;

                if (!signal.HasValue)
                {
                    throw new InvalidOperationException($"Wire {wireId} has no signal");
                }

                return signal.Value;
            }

            public void SetSignal(string wireId, ushort signal)
            {
                if (!wires.ContainsKey(wireId))
                {
                    var wire = new Wire(wireId);
                    wires.Add(wire.Id, wire);
                }

                wires[wireId].Signal = signal;
            }

            public void ConnectAndGate(string in1, string in2, string outputId)
            {
                
                AddGate(new BinaryGate(CreateSignalSource(in1), GetWire(in2), GetWire(outputId), (i1, i2) => (ushort) (i1 & i2)));
            }

            public void ConnectOrGate(string in1, string in2, string outputId)
            {
                AddGate(new BinaryGate(CreateSignalSource(in1), GetWire(in2), GetWire(outputId), (i1, i2) => (ushort)(i1 | i2)));
            }

            public void ConnectRightShiftGate(string source1, ushort source2, string outputId)
            {
                AddGate(new BinaryGate(GetWire(source1), new FixedSignal(source2), GetWire(outputId), (i1, i2) => (ushort)(i1 >> i2)));
            }

            public void ConnectLeftShiftGate(string source1, ushort source2, string outputId)
            {
                AddGate(new BinaryGate(GetWire(source1), new FixedSignal(source2), GetWire(outputId), (i1, i2) => (ushort)(i1 << i2)));
            }

            public void ConnectNotGate(string source, string outputId)
            {
                AddGate(new UnaryGate(GetWire(source), GetWire(outputId), i => (ushort)(~i)));
            }

            public void ConnectNopGate(string source, string outputId)
            {
                AddGate(new UnaryGate(GetWire(source), GetWire(outputId), i => i));
            }
            private void AddGate(IGate gate)
            {
                gates.Add(gate);
            }

            private Wire GetWire(string wireId)
            {
                if (!wires.ContainsKey(wireId))
                {
                    wires.Add(wireId, new Wire(wireId));
                }

                return wires[wireId];
            }

            private ISignalSource CreateSignalSource(string sourceDescription)
            {
                ISignalSource source;
                if (ushort.TryParse(sourceDescription, out ushort signal))
                {
                    source = new FixedSignal(signal);
                }
                else
                {
                    var wire = GetWire(sourceDescription);
                    source = wire;
                }

                return source;
            }
        }

        public interface ISignalSource
        {
            ushort? Signal { get; }
        }

        public class Wire : ISignalSource
        {
            public Wire(string id)
            {
                Id = id;
            }

            public string Id { get; }
            public ushort? Signal { get; set; }

            public override string ToString()
            {
                return $"Wire {Id}: {Signal}";
            }
        }

        public class FixedSignal : ISignalSource
        {
            public FixedSignal(ushort signal)
            {
                Signal = signal;
            }

            public ushort? Signal { get; }

            public override string ToString()
            {
                return $"Signal {Signal}";
            }
        }

        public interface IGate
        {
            Wire Out { get; }
            void Evaluate();
        }

        public abstract class Gate : IGate
        {
            public Wire Out { get; }

            protected Gate(Wire output)
            {
                Out = output;
            }

            public abstract void Evaluate();
        }

        public class BinaryGate : Gate
        {
            public ISignalSource In1 { get; }
            public ISignalSource In2 { get; }
            private Func<ushort, ushort, ushort> Operation { get; }


            public BinaryGate(ISignalSource in1, ISignalSource in2, Wire output, Func<ushort, ushort, ushort> operation) : base(output)
            {
                In1 = in1;
                In2 = in2;
                Operation = operation;
            }


            public override void Evaluate()
            {
                if (In1.Signal.HasValue && In2.Signal.HasValue)
                {
                    Out.Signal = Operation(In1.Signal.Value, In2.Signal.Value);
                }
            }

        }

        public class UnaryGate : Gate
        {
            public ISignalSource In { get; }
            private Func<ushort, ushort> Operation { get; }


            public UnaryGate(ISignalSource input, Wire output, Func<ushort, ushort> operation) : base(output)
            {
                In = input;
                Operation = operation;
            }


            public override void Evaluate()
            {
                if (In.Signal.HasValue)
                {
                    Out.Signal = Operation(In.Signal.Value);
                }
            }

        }

        public static void Connect(string descriptor, Circuit circuit)
        {
            var signalPattern = new Regex(@"^(\d+) -> ([a-z]+)");
            var andPattern = new Regex(@"^(\d+|[a-z]+) AND ([a-z]+) -> ([a-z]+)");
            var orPattern = new Regex(@"^(\d+|[a-z]+) OR ([a-z]+) -> ([a-z]+)");
            var rightShiftPattern = new Regex(@"^([a-z]+) RSHIFT (\d+) -> ([a-z]+)");
            var leftShiftPattern = new Regex(@"^([a-z]+) LSHIFT (\d+) -> ([a-z]+)");
            var notPattern = new Regex(@"^NOT ([a-z]+) -> ([a-z]+)");
            var nopPattern = new Regex(@"^([a-z]+) -> ([a-z]+)");

            switch (descriptor)
            {
                case var _ when signalPattern.IsMatch(descriptor):
                {
                    var match = signalPattern.Match(descriptor);
                    var signal = ushort.Parse(match.Groups[1].Value);
                    var wireId = match.Groups[2].Value;
                    circuit.SetSignal(wireId, signal);
                    break;
                }

                case var _ when andPattern.IsMatch(descriptor):
                {
                    var match = andPattern.Match(descriptor);
                    circuit.ConnectAndGate(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
                    break;
                }

                case var _ when orPattern.IsMatch(descriptor):
                {
                    var match = orPattern.Match(descriptor);
                    circuit.ConnectOrGate(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
                    break;
                }

                case var _ when rightShiftPattern.IsMatch(descriptor):
                {
                    var match = rightShiftPattern.Match(descriptor);
                    circuit.ConnectRightShiftGate(match.Groups[1].Value, ushort.Parse(match.Groups[2].Value), match.Groups[3].Value);
                    break;
                }

                case var _ when leftShiftPattern.IsMatch(descriptor):
                {
                    var match = leftShiftPattern.Match(descriptor);
                    circuit.ConnectLeftShiftGate(match.Groups[1].Value, ushort.Parse(match.Groups[2].Value), match.Groups[3].Value);
                    break;
                }

                case var _ when notPattern.IsMatch(descriptor):
                {
                    var match = notPattern.Match(descriptor);
                    circuit.ConnectNotGate(match.Groups[1].Value, match.Groups[2].Value);
                    break;
                }

                case var _ when nopPattern.IsMatch(descriptor):
                {
                    var match = nopPattern.Match(descriptor);
                    circuit.ConnectNopGate(match.Groups[1].Value, match.Groups[2].Value);
                        break;
                }

                default:
                    throw new ArgumentException($"Unrecognized descriptor: {descriptor}");
            }
        }

        public static Circuit Run(IEnumerable<string> input)
        {
            var circuit = new Circuit();

            input.ToList().ForEach(d => Connect(d, circuit));

            circuit.Evaluate();

            return circuit;
        }       
        
        public static Circuit Run2(IEnumerable<string> input)
        {
            var circuit1 = new Circuit();

            var inputList = input.ToList();

            inputList.ForEach(d => Connect(d, circuit1));
            circuit1.Evaluate();

            var aSignal = circuit1.GetSignal("a");

            var circuit2 = new Circuit();

            inputList.ForEach(d => Connect(d, circuit2));
            circuit2.SetSignal("b", aSignal);

            circuit2.Evaluate();

            return circuit2;
        }

        public static int FirstStar()
        {
            var input = ReadLineInput();

            var circuit = Run(input);

            return circuit.GetSignal("a");
        }

        public static int SecondStar()
        {
            var input = ReadLineInput();

            var circuit = Run2(input);

            return circuit.GetSignal("a");
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(16076, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(2797, result);
        }

        [Fact]
        public void FirstStar_Example()
        {
            var input = new[]
            {
                "123 -> x",
                "456 -> y",
                "x AND y -> d",
                "x OR y -> e",
                "x LSHIFT 2 -> f",
                "y RSHIFT 2 -> g",
                "NOT x -> h",
                "NOT y -> i"
            };

            var result = Run(input);

            Assert.Equal(72, result.GetSignal("d"));
            Assert.Equal(507, result.GetSignal("e"));
            Assert.Equal(492, result.GetSignal("f"));
            Assert.Equal(114, result.GetSignal("g"));
            Assert.Equal(65412, result.GetSignal("h"));
            Assert.Equal(65079, result.GetSignal("i"));
            Assert.Equal(123, result.GetSignal("x"));
            Assert.Equal(456, result.GetSignal("y"));
        }
    }
}

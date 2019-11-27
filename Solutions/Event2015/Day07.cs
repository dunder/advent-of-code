using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 6: Probably a Fire Hazard ---
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

            public void AddGate(IGate gate)
            {
                gates.Add(gate);
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
                ISignalSource source1;
                if (ushort.TryParse(in1, out ushort signal))
                {
                    source1 = new FixedSignal(signal);
                }
                else
                {
                    var wire = GetWire(in1);
                    source1 = wire;
                }
                AddGate(new AndGate(source1, GetWire(in2), GetWire(outputId)));
            }

            public void ConnectOrGate(string in1, string in2, string outputId)
            {
                ISignalSource source1;
                if (ushort.TryParse(in1, out ushort signal))
                {
                    source1 = new FixedSignal(signal);
                }
                else
                {
                    var wire = GetWire(in1);
                    source1 = wire;
                }
                AddGate(new OrGate(source1, GetWire(in2), GetWire(outputId)));
            }

            public void ConnectRightShiftGate(string source1, ushort source2, string outputId)
            {
                AddGate(new RightShiftGate(GetWire(source1), new FixedSignal(source2), GetWire(outputId)));

            }

            public void ConnectLeftShiftGate(string source1, ushort source2, string outputId)
            {
                AddGate(new LeftShiftGate(GetWire(source1), new FixedSignal(source2), GetWire(outputId)));
            }

            public void ConnectNotGate(string source, string outputId)
            {
                AddGate(new NotGate(GetWire(source), GetWire(outputId)));
            }

            public void ConnectNopGate(string source, string outputId)
            {
                AddGate(new NopGate(GetWire(source), GetWire(outputId)));
            }

            public Wire GetWire(string wireId)
            {
                if (!wires.ContainsKey(wireId))
                {
                    wires.Add(wireId, new Wire(wireId));
                }

                return wires[wireId];
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

            public int? GetSignal(string wireId)
            {
                return wires[wireId].Signal;
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

            public void Evaluate()
            {
                if (CanEvaluate)
                {
                    Out.Signal = EvaluateInternal();
                }
            }
            
            protected abstract bool CanEvaluate { get; }
            protected abstract ushort EvaluateInternal();
        }

        public abstract class BinaryGate : Gate
        {
            public ISignalSource In1 { get; }
            public ISignalSource In2 { get; }


            protected BinaryGate(ISignalSource in1, ISignalSource in2, Wire output) : base(output)
            {
                In1 = in1;
                In2 = in2;
            }

            protected override bool CanEvaluate => In1.Signal.HasValue && In2.Signal.HasValue;
        }

        public class AndGate : BinaryGate
        {
            public AndGate(ISignalSource in1, ISignalSource in2, Wire output) : base(in1, in2, output)
            {
            }

            protected override ushort EvaluateInternal()
            {
                return (ushort) (In1.Signal.Value & In2.Signal.Value);
            }

            public override string ToString()
            {
                return $"{In1} AND {In2} -> {Out}";
            }
        }
        
        public class OrGate : BinaryGate
        {
            public OrGate(ISignalSource in1, ISignalSource in2, Wire output) : base(in1, in2, output)
            {
            }

            protected override ushort EvaluateInternal()
            {
                return (ushort) (In1.Signal.Value | In2.Signal.Value);
            }

            public override string ToString()
            {
                return $"{In1} OR {In2} -> {Out}";
            }
        }        
        
        public class LeftShiftGate : BinaryGate
        {
            public LeftShiftGate(ISignalSource in1, ISignalSource in2, Wire output) : base(in1, in2, output)
            {
            }

            protected override ushort EvaluateInternal()
            {
                return (ushort) (In1.Signal.Value << In2.Signal.Value);
            }

            public override string ToString()
            {
                return $"{In1} LSHIFT {In2} -> {Out}";
            }
        }

        public class RightShiftGate : BinaryGate
        {
            public RightShiftGate(ISignalSource in1, ISignalSource in2, Wire output) : base(in1, in2, output)
            {
            }

            protected override ushort EvaluateInternal()
            {
                return (ushort) (In1.Signal.Value >> In2.Signal.Value);
            }

            public override string ToString()
            {
                return $"{In1} RSHIFT {In2} -> {Out}";
            }
        }

        public abstract class UnaryGate : Gate
        {
            public ISignalSource In { get; }


            protected UnaryGate(ISignalSource input, Wire output) : base(output)
            {
                In = input;
            }

            protected override bool CanEvaluate => In.Signal.HasValue;
        }

        public class NotGate : UnaryGate
        {
            public NotGate(ISignalSource in1, Wire output) : base(in1, output)
            {
            }

            protected override ushort EvaluateInternal()
            {
                return (ushort) ~In.Signal.Value;
            }

            public override string ToString()
            {
                return $"NOT {In} -> {Out}";
            }
        }

        public class NopGate : UnaryGate
        {
            public NopGate(ISignalSource in1, Wire output) : base(in1, output)
            {
            }

            protected override ushort EvaluateInternal()
            {
                return (ushort) In.Signal.Value;
            }

            public override string ToString()
            {
                return $"{In} -> {Out}";
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
            var circuit = new Circuit();

            input.ToList().ForEach(d => Connect(d, circuit));

            // manually inserting override, refactor sometime maybe
            circuit.SetSignal("b", 16076);

            circuit.Evaluate();

            return circuit;
        }

        public static int FirstStar()
        {
            var input = ReadLineInput();

            var circuit = Run(input);

            return circuit.GetSignal("a").Value;
        }

        public static int SecondStar()
        {
            var input = ReadLineInput();

            var circuit = Run2(input);

            return circuit.GetSignal("a").Value;
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

            Assert.Equal(72, result.GetSignal("d").Value);
            Assert.Equal(507, result.GetSignal("e").Value);
            Assert.Equal(492, result.GetSignal("f").Value);
            Assert.Equal(114, result.GetSignal("g").Value);
            Assert.Equal(65412, result.GetSignal("h").Value);
            Assert.Equal(65079, result.GetSignal("i").Value);
            Assert.Equal(123, result.GetSignal("x").Value);
            Assert.Equal(456, result.GetSignal("y").Value);
        }
    }
}

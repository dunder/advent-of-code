
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 20: Pulse Propagation ---
    public class Day20
    {
        private ITestOutputHelper output;

        public Day20(ITestOutputHelper output)
        {
            this.output = output;
        }

        private enum Pulse { Low, High }

        private abstract class Module
        {
            private string _name;
            private List<string> _output;

            protected Module(string name) : this(name, new List<string>())
            {
            }            
            protected Module(string name, List<string> output)
            {
                _name = name;
                _output = output;
            }

            public List<string> Output => _output;

            public string Name => _name;

            public abstract List<PendingSend> Send(Dictionary<string, Module> modules, string dispatcher, Pulse pulse);
        }

        private class Button : Module
        {
            public Button(Dictionary<string, Module> modules, Broadcast broadcast) : base("button", new List<string> { "broadcaster" })
            {
                Broadcast = broadcast;
                Modules = modules;
            }

            public Module Broadcast { get; private set; }
            public Dictionary<string, Module> Modules { get; }

            public override List<PendingSend> Send(Dictionary<string, Module> modules, string dispatcher, Pulse pulse)
            {
                return new List<PendingSend> { new PendingSend(Pulse.Low, Name, Broadcast) };
            }
        }

        private class Broadcast : Module
        {
            public Broadcast(List<string> output) : base("broadcaster", output)
            {
            }

            public override List<PendingSend> Send(Dictionary<string, Module> modules, string dispatcher, Pulse pulse)
            {
                return Output.Select(output => new PendingSend(pulse, Name, modules[output])).ToList();
            }

            public override string ToString()
            {
                return $"{Name:20}: {string.Join(",", Output):20}";
            }
        }

        private class FlipFlop : Module
        {
            private bool _isFlipped = false;

            public FlipFlop(string name, List<string> output) : base(name, output)
            {
            }

            private bool On => _isFlipped;

            public override List<PendingSend> Send(Dictionary<string, Module> modules, string dispatcher, Pulse pulse)
            {
                if (pulse == Pulse.High)
                {
                    return new List<PendingSend>();
                }

                Pulse sendPulse = On? Pulse.Low: Pulse.High;

                _isFlipped = !_isFlipped;

                return Output.Select(output => new PendingSend(sendPulse, Name, modules[output])).ToList();
            }

            public override string ToString()
            {
                var flipped = _isFlipped ? "1" : "0";
                return $"{Name:20}: {flipped:20}";
            }
        }

        private class Conjunction : Module
        {
            private readonly Dictionary<string, Pulse> _input;

            public Conjunction(string name, List<string> input, List<string> output) : base(name, output)
            {
                _input = new Dictionary<string, Pulse>();

                foreach (var moduleName in input)
                {
                    _input.Add(moduleName, Pulse.Low);
                }
            }

            public bool AllOn => _input.Values.All(input => input == Pulse.High);


            public override List<PendingSend> Send(Dictionary<string, Module> modules, string dispatcher, Pulse pulse)
            {
                _input[dispatcher] = pulse;

                Pulse sendPulse = AllOn ? Pulse.Low : Pulse.High;

                return Output.Select(output => new PendingSend(sendPulse, Name, modules[output])).ToList();
            }

            public override string ToString()
            {
                var input = string.Join("", _input.Values.Select(i => i == Pulse.High ? "1" : "0"));
                return $"{Name:20}: {input:20}";
            }
        }

        private class Empty : Module
        {
            public Empty(string name) : base(name, new List<string>())
            {
            }

            public override List<PendingSend> Send(Dictionary<string, Module> modules, string dispatcher, Pulse pulse)
            {
                return new();
            }
            public override string ToString()
            {
                return $"{Name:20}: -";
            }
        }

        private Dictionary<string, Module> Parse(IList<string> input)
        {
            var modules = new Dictionary<string, Module>();
            var connections = new Dictionary<string, List<string>>();
            var types = new Dictionary<string, char>();

            foreach (var line in input)
            {
                var parts = line.Split(" -> ");
                var module = parts[0];
                List<string> receivers = parts[1].Split(", ").ToList();

                var type = module[0];

                bool broadcaster = module.StartsWith("broadcaster");

                var name = broadcaster ? "broadcaster" : module[1..];

                types.Add(name, type);
                connections.Add(name, receivers);
            }

            foreach (var connection in connections.ToArray())
            {
                var name = connection.Key;
                var receivers = connection.Value;

                switch (types[connection.Key])
                {
                    case '%':
                        modules.Add(name, new FlipFlop(name, receivers));
                        break;
                    case '&':
                        var inputs = connections
                            .Where(connection => connection.Value.Contains(name))
                            .Select(connection => connection.Key).ToList();

                        modules.Add(name, new Conjunction(name, inputs, receivers));
                        break;
                    case 'b':
                        modules.Add(name, new Broadcast(receivers));
                        break;
                }
            }

            var emptyModules = connections
                .Select(c => c.Value)
                .SelectMany(c => c)
                .Where(c => !modules.ContainsKey(c));

            foreach (var empty in emptyModules)
            {
                modules.Add(empty, new Empty(empty));
            }

            return modules;
        }

        private class PendingSend
        {
            public PendingSend(Pulse pulse, string dispatcher, Module module)
            {
                Pulse = pulse;
                Dispatcher = dispatcher;
                Module = module;
            }
            public Pulse Pulse { get; }
            public string Dispatcher { get; }
            public Module Module { get; }
        }

        private int Run1(IList<string> input)
        {
            Dictionary<string, Module> modules = Parse(input);

            var button = new Button(modules, modules["broadcaster"] as Broadcast);

            var sendQueue = new Queue<PendingSend>();
            var sent = new List<Pulse>();

            for (int i = 1; i <= 1000000000; i++)
            {
                List<PendingSend> pendingFromButton = button.Send(modules, "hq", Pulse.Low);
                sendQueue.Enqueue(pendingFromButton.Single());

                while (sendQueue.Count > 0)
                {
                    var pendingSend = sendQueue.Dequeue();

                    //output.WriteLine($"{pendingSend.Dispatcher} -{pendingSend.Pulse} -> {pendingSend.Module.Name}");

                    var pendingSends = pendingSend.Module.Send(modules, pendingSend.Dispatcher, pendingSend.Pulse);

                    sent.Add(pendingSend.Pulse);

                    foreach (var next in pendingSends)
                    {
                        sendQueue.Enqueue(next);
                    }
                }

                DrawSchema(modules, i);
            }
    
            return sent.Count(s => s == Pulse.Low) * sent.Count(s => s == Pulse.High);
        }

        private void DrawSchema(Dictionary<string, Module> modules, int counter)
        {
            Console.WriteLine($"iteration: {counter:D8} modules: {modules.Count:D8}");

            foreach (var module in modules.Where(m => m.Value is Conjunction))
            {
                Console.WriteLine($"{module.Value.ToString():40}");
            }
            Console.CursorTop = 1;
        }

        private int Run2(IList<string> input)
        {
            return 0;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Run1(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return Run2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(832957356, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "broadcaster -> a, b, c",
                "%a -> b",
                "%b -> c",
                "%c -> inv",
                "&inv -> a",
            };

            Assert.Equal(32000000, Run1(example));
        }

        [Fact]
        public void FirstStarExample2()
        {
            var example = new List<string>
            {
                "broadcaster -> a",
                "%a -> inv, con",
                "&inv -> b",
                "%b -> con",
                "&con -> output"
            };

            Assert.Equal(11687500, Run1(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            
        }
    }
}

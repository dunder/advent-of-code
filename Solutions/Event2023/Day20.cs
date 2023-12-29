
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
                return $"{Name} -> {string.Join(",", Output)}";
            }
        }

        private class FlipFlop : Module
        {
            private bool _isFlipped = false;

            public FlipFlop(string name, List<string> output) : base(name, output)
            {
            }

            public bool On => _isFlipped;

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
                return $"%{Name}: {flipped} -> {string.Join(",", Output)}";
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
                return $"{Name} {input} -> {string.Join(",", Output)}";
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
                return $"{Name,20}: -";
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

            public override string ToString()
            {
                return $"Send {Pulse} from {Dispatcher} to {Module.Name}";
            }
        }

        private int Run1(IList<string> input)
        {
            Dictionary<string, Module> modules = Parse(input);

            var button = new Button(modules, modules["broadcaster"] as Broadcast);

            var sendQueue = new Queue<PendingSend>();
            var sent = new List<PendingSend>();

            for (int i = 1; i <= 1000; i++)
            {
                (_, var sentSinglePress) = PressButton(modules, p => false);

                sent.AddRange(sentSinglePress);
            }

            var sentPulses = sent.Select(s => s.Pulse).ToList();
    
            return sentPulses.Count(s => s == Pulse.Low) * sentPulses.Count(s => s == Pulse.High);
        }

        private (bool, List<PendingSend>) PressButton(Dictionary<string, Module> modules, Func<PendingSend, bool> onPendingSend)
        {
            var button = new Button(modules, modules["broadcaster"] as Broadcast);

            List<PendingSend> pendingFromButton = button.Send(modules, "hq", Pulse.Low);
            
            var sendQueue = new Queue<PendingSend>();
            sendQueue.Enqueue(pendingFromButton.Single());

            var sent = new List<PendingSend>();

            while (sendQueue.Count > 0)
            {
                var pendingSend = sendQueue.Dequeue();

                if (onPendingSend(pendingSend))
                {
                    return (true, sent);
                }

                var pendingSends = pendingSend.Module.Send(modules, pendingSend.Dispatcher, pendingSend.Pulse);
                
                sent.Add(pendingSend);

                foreach (var next in pendingSends)
                {
                    sendQueue.Enqueue(next);
                }
            }

            return (false, sent);
        }

        private long Run2(IList<string> input)
        {
            Dictionary<string, Module> modules = Parse(input);

            var button = new Button(modules, modules["broadcaster"] as Broadcast);

            var sendQueue = new Queue<PendingSend>();

            // found by inspection of input (&xj, &qs, &kz, &km) -> &gq -> rx
            // &mf -> &xj
            // &ph -> &qs
            // &zp -> &kz
            // &jn -> &km

            Dictionary<string, int> targetCounters = new()
            {
                { "mf", 0 },
                { "ph", 0 },
                { "zp", 0 },
                { "jn", 0 }
            };

            int pressCounter = 0;
            bool targetsFound = false;

            while (!targetsFound)
            {
                bool OnButtonPress(PendingSend pendingSend)
                {
                    var updatedTargetCounters = targetCounters
                        .Where(t => t.Key == pendingSend.Dispatcher && pendingSend.Pulse == Pulse.Low);

                    foreach (var updatedTargetCounter in updatedTargetCounters)
                    {
                        targetCounters[updatedTargetCounter.Key] = pressCounter;
                    }

                    return targetCounters.All(t => t.Value > 0);
                }

                pressCounter++;

                (bool found, List<PendingSend> sent) = PressButton(modules, OnButtonPress);

                targetsFound = found;
            }

            // all primes
            return targetCounters.Values.Aggregate(1L, (total, count) => total * count);
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Run1(input);
        }

        public long SecondStar()
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
            Assert.Equal(240162699605221, SecondStar());
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

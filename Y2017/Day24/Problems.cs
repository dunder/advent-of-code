using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day24 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day24\input.txt");

            var result = ElectromagneticMoat.StrongestBridge(input);

            _output.WriteLine($"Day 24 problem 1: {result}");  // 2505 för högt,2029 för högt
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day24\input.txt");

            var result = ElectromagneticMoat.StrongestLongestBridge(input);

            _output.WriteLine($"Day 24 problem 2: {result}");
        }
    }

    public class ElectromagneticMoat {
        public static int StrongestBridge(string[] input) {

            var repository = new PortComponentRepository();

            var startComponents = new List<(int PortA, int PortB)>();

            foreach (var line in input) {
                var ports = line.Split('/');
                var a = int.Parse(ports[0]);
                var b = int.Parse(ports[1]);
                
                repository.Add((a, b));

                if (a == 0 || b == 0) {
                    startComponents.Add((a,b));
                }
            }

            var maxPath = 0;
            foreach (var startComponent in startComponents) {
                var allPaths = new List<List<(int PortA, int PortB)>>();
                var firstPath = new List<(int PortA, int PortB)>();

                RecursivePath((startComponent.PortB, startComponent.PortA, startComponent.PortB), allPaths, firstPath, repository);

                foreach (var path in allPaths) {
                    maxPath = Math.Max(maxPath, path.Sum(x => x.PortA + x.PortB));
                }
            }

            return maxPath;
        }

        public static int StrongestLongestBridge(string[] input) {

            var repository = new PortComponentRepository();

            var startComponents = new List<(int PortA, int PortB)>();

            foreach (var line in input) {
                var ports = line.Split('/');
                var a = int.Parse(ports[0]);
                var b = int.Parse(ports[1]);
                
                repository.Add((a, b));

                if (a == 0 || b == 0) {
                    startComponents.Add((a,b));
                }
            }

            var maxPath = 0;
            var allPaths = new List<List<(int PortA, int PortB)>>();

            foreach (var startComponent in startComponents) {
                var firstPath = new List<(int PortA, int PortB)>();

                RecursivePath((startComponent.PortB, startComponent.PortA, startComponent.PortB), allPaths, firstPath, repository);
            }

            var longestPaths = allPaths.GroupBy(p => p.Count).OrderByDescending(x => x.Key).First();

            foreach (var path in longestPaths) {
                maxPath = Math.Max(maxPath, path.Sum(x => x.PortA + x.PortB));
            }

            return maxPath;
        }

        private static List<(int PortA, int PortB)> RecursivePath(
            (int PortUnused, int PortA, int PortB) current, 
            List<List<(int PortA, int PortB)>> allPaths,
            List<(int PortA, int PortB)> path,
            PortComponentRepository repository) {

            path.Add((current.PortA, current.PortB));

            List<(int PortA, int PortB)> neighbours = repository
                .Find(current.PortUnused)
                .Where(c => !path.Contains(c))
                .ToList();

            foreach (var neighbour in neighbours) {

                var portUnused = current.PortUnused == neighbour.PortA ? neighbour.PortB : neighbour.PortA;

                var list = new List<(int PortA, int PortB)>(path);
                allPaths.Add(RecursivePath((portUnused, neighbour.PortA, neighbour.PortB), allPaths, list, repository));
            }

            allPaths.Add(path);

            return path;
        }

        public static IEnumerable<List<(int PortA, int PortB)>> DepthFirstPaths((int PortA, int PortB) start, PortComponentRepository repository) {
            var visited = new HashSet<(int PortA, int PortB)>();
            var stack = new Stack<(int PortUnused, int PortA, int PortB)>();

            stack.Push((start.PortB, start.PortA, start.PortB));

            var pathStack = new Stack<List<(int PortA, int PortB)>>();
            pathStack.Push(new List<(int PortA, int PortB)>());

            while (stack.Count != 0) {
                var current = stack.Pop();

                pathStack.Peek().Add((current.PortA, current.PortB));

                if (!visited.Add((current.PortA, current.PortB))) {
                    continue;
                }

                List<(int PortA, int PortB)> neighbours = repository
                    .Find(current.PortUnused)
                    .Where(n => !visited.Contains(n))
                    .ToList();

                if (!neighbours.Any()) {
                    yield return pathStack.Pop();
                }


                foreach (var neighbour in neighbours) {
                    var portUnused = current.PortUnused == neighbour.PortA ? neighbour.PortB : neighbour.PortA;

                    var extendedPath = new List<(int PortA, int PortB)>(pathStack.Pop()) {
                        (neighbour.PortA, neighbour.PortB)
                    };
                    pathStack.Push(extendedPath);

                    stack.Push((portUnused, neighbour.PortA, neighbour.PortB));
                }
            }
        }
    }

    public class PortComponentRepository {
        private readonly Dictionary<int, List<(int, int)>> _aPorts = new Dictionary<int, List<(int, int)>>();
        private readonly Dictionary<int, List<(int, int)>> _bPorts = new Dictionary<int, List<(int, int)>>();

        public void Add((int portA, int portB) component) {
            if (!_aPorts.ContainsKey(component.portA)) {
                _aPorts.Add(component.portA, new List<(int, int)>());
            }
            _aPorts[component.portA].Add(component);

            if (!_bPorts.ContainsKey(component.portB)) {
                _bPorts.Add(component.portB, new List<(int, int)>());
            }
            _bPorts[component.portB].Add(component);
        }

        public List<(int PortA, int PortB)> Find(int portUnused) {
            List<(int PortA, int PortB)> matchingPorts = new List<(int, int)>();

            if (_aPorts.ContainsKey(portUnused)) {
                matchingPorts = matchingPorts
                    .Concat(_aPorts[portUnused])
                    .ToList();
            }

            if (_bPorts.ContainsKey(portUnused)) {
                matchingPorts = matchingPorts
                    .Concat(_bPorts[portUnused])
                    .ToList();
            }

            return matchingPorts.Distinct().ToList();
        }

        public void Use(int portValue, (int portA, int portB) component) {

            if (portValue != component.portA && portValue != component.portB) {
                throw new ArgumentOutOfRangeException(nameof(portValue), $"The value {portValue} is icompatible with component: {component}");
            }

            if (portValue == component.portA) {
                _aPorts[portValue].Remove(component);
                if (!_aPorts[portValue].Any()) {
                    _aPorts.Remove(portValue);
                }
            } else if (portValue == component.portB) {
                _bPorts[portValue].Remove(component);
                if (!_bPorts[portValue].Any()) {
                    _bPorts.Remove(portValue);
                }
            }
        }
    }
}

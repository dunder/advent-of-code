using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.MapGeometry;
using Shared.Tree;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day15 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day15;
        public string Name => "Beverage Bandits";

        public int Input => 793031;

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = Combat(input);
            return result.ToString();
        }

        private int Combat(IList<string> input)
        {
            var (walls, elves, goblins) = Parse(input);

            var game = new Game(walls, elves, goblins);

            return game.Run();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = "";
            return result;
        }

        public class Soldier
        {
            public int Id { get; set; }
            public Point Position { get; set; }
            public char Class { get; }
            public List<Soldier> Targets { get; }
            public List<Soldier> AliveTargets => Targets.Where(t => !t.IsDead).ToList();
            public int HitPoints { get; private set; }
            public int AttackPower { get; }
            public bool IsDead => HitPoints <= 0;

            public Soldier(int id, Point position, char @class, List<Soldier> targets, int hitPoints, int attackPower)
            {
                Id = id;
                Position = position;
                Class = @class;
                Targets = targets;
                HitPoints = hitPoints;
                AttackPower = attackPower;
            }


            public Soldier Move(HashSet<Point> occupied)
            {
                var possibleMoves = Position.AdjacentInMainDirections()
                    .Where(occupied.Contains)
                    .ToHashSet();

                var targetsInRange = AliveTargets
                    .Where(t => possibleMoves.Contains(t.Position))
                    .ToList();

                // target is already in range
                if (targetsInRange.Any(t => possibleMoves.Contains(t.Position)))
                {
                    return targetsInRange.OrderBy(t => t.Position.Y).ThenBy(t => t.Position.X).First();
                }

                var adjacent = AliveTargets.SelectMany(t => t.Position.AdjacentInMainDirections()).ToHashSet();

                // keep only not occupied
                adjacent.ExceptWith(occupied);

                // find out which are reachable

                foreach (var targetPosition in adjacent)
                {
                    IEnumerable<Point> Neighbors(Point n)
                    {
                        return n.AdjacentInMainDirections().Where(p => !occupied.Contains(p));
                    }

                    var result = targetPosition.DepthFirst(Neighbors, p => p.Data == targetPosition);
                }


                // return the soldier in range
                return null;
            }

            public void Attack(Soldier soldier)
            {
                if (soldier == null) return;
                soldier.HitPoints -= AttackPower;
            }

            public override string ToString()
            {
                return $"{Class} ({Position.X},{Position.Y})";
            }
        }

        public class Game
        {
            private readonly HashSet<Point> walls;
            private readonly List<Soldier> elves;
            private readonly List<Soldier> goblins;
        
            public Game(HashSet<Point> walls, List<Soldier> elves, List<Soldier> goblins)
            {
                this.walls = walls;
                this.elves = elves;
                this.goblins = goblins;
            }

            public int Run()
            {
                int round = 0;
                bool gameOver = false;
                while (!gameOver)
                {
                    var order = elves.ToList().Concat(goblins)
                        .OrderBy(s => s.Position.Y)
                        .ThenBy(s => s.Position.X)
                        .ToList();

                    for (int s = 0; s < order.Count; s++)
                    {
                        var soldier = order[s];
                        if (soldier.IsDead) continue;

                        var occupied = new HashSet<Point>(walls);
                        elves.ForEach(e => occupied.Add(e.Position));
                        goblins.ForEach(g => occupied.Add(g.Position));

                        var targetInRange = soldier.Move(occupied);
                        soldier.Attack(targetInRange);

                        if (!soldier.AliveTargets.Any())
                        {
                            gameOver = true;
                            break;
                        }
                    }

                    round++;
                }

                var winningHitPoints = elves.Any(e => !e.IsDead)
                    ? elves.Sum(e => e.HitPoints)
                    : goblins.Sum(g => g.HitPoints);

                return round * winningHitPoints;
            }

        }

        public static (HashSet<Point> walls, List<Soldier> elves, List<Soldier> goblins) Parse(IList<string> input)
        {
            var walls = new HashSet<Point>();
            var elves = new List<Soldier>();
            var goblins = new List<Soldier>();
            var id = 1;
            for (var y = 0; y < input.Count; y++)
            {
                var line = input[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var point = new Point(x, y);
                    var c = line[x];
                    switch (c)
                    {
                        case '#':
                            walls.Add(point);
                            break;
                        case 'E':
                            elves.Add(new Soldier(id++, point, c, goblins, 200, 3));
                            break;
                        case 'G':
                            goblins.Add(new Soldier(id++, point, c, elves, 200, 3));
                            break;
                    }
                }
            }
            return (walls, elves, goblins);
        }


        [Fact]
        public void FirstStartTest()
        {
            var lines = new[]
            {
                "#######",
                "#.G.E.#",
                "#E.G.E#",
                "#.G.E.#",
                "#######",
            };

            var result = Combat(lines);

            Assert.Equal(0, result);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual);
        }
    }
}
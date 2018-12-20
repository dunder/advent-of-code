using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day15 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day15;
        public string Name => "Beverage Bandits";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = Combat(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = CombatWithElevatedElfPower(input);
            return result.ToString();
        }

        public void FirstStarConsole()
        {
            Combat(ReadLineInput(), true);
        }

        public static int Combat(IList<string> input, bool toConsole = false)
        {
            var ui = toConsole ? (IGameEventHandler) new ConsoleUi() : new NoUi();
            var game = Game.Create(input, ui);

            return game.Run();
        }

        public static int CombatWithElevatedElfPower(IList<string> input, bool toConsole = false)
        {
            var ui = toConsole ? (IGameEventHandler)new ConsoleUi() : new NoUi();
            int survivingElves;
            int elvesAtStart;
            int outcome;
            var elvesAttackingPower = 3;
            do
            {
                var game = Game.Create(input, ui);
                elvesAtStart = game.AliveElves.Count;
                game.AliveElves.ForEach(e => e.UpdateAttackPower(elvesAttackingPower++));
                outcome = game.Run();
                survivingElves = game.AliveGoblins.Count;

            } while (survivingElves < elvesAtStart);

            return outcome;
        }

        [Fact]
        public void FirstStarCombatTest1()
        {
            var lines = new[]
            {

                "#######",
                "#.G...#",
                "#...EG#",
                "#.#.#G#",
                "#..G#E#",
                "#.....#",
                "#######"
            };

            var outcome = Combat(lines);

            Assert.Equal(27730, outcome);
        }

        [Fact]
        public void FirstStarCombatTest2()
        {
            var lines = new[]
            {
                "#######", 
                "#G..#E#", 
                "#E#E.E#", 
                "#G.##.#", 
                "#...#E#", 
                "#...E.#", 
                "#######"
            };

            var outcome = Combat(lines);

            Assert.Equal(36334, outcome);
        }

        [Fact]
        public void FirstStarCombatTest3()
        {
            var lines = new[]
            {
                "#######", 
                "#E..EG#", 
                "#.#G.E#", 
                "#E.##E#", 
                "#G..#.#", 
                "#..E#.#", 
                "#######"
            };

            var outcome = Combat(lines);

            Assert.Equal(39514, outcome);
        }

        [Fact]
        public void FirstStarCombatTest4()
        {
            var lines = new[]
            {
                "#######",
                "#E.G#.#",
                "#.#G..#",
                "#G.#.G#",
                "#G..#.#",
                "#...E.#",
                "#######"
            };

            var outcome = Combat(lines);

            Assert.Equal(27755, outcome);
        }

        [Fact]
        public void FirstStarCombatTest5()
        {
            var lines = new[]
            {
                "#######",
                "#.E...#",
                "#.#..G#",
                "#.###.#",
                "#E#G#G#",
                "#...#G#",
                "#######"
            };

            var outcome = Combat(lines);

            Assert.Equal(28944, outcome);
        }

        [Fact]
        public void FirstStarCombatTest6()
        {
            var lines = new[]
            {
                "#########",
                "#G......#",
                "#.E.#...#",
                "#..##..G#",
                "#...##..#",
                "#...#...#",
                "#.G...G.#",
                "#.....G.#",
                "#########"
            };

            var outcome = Combat(lines);

            Assert.Equal(18740, outcome);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("207059", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("49120", actual);
        }
    }

    public class Game
    {
        private readonly IGameEventHandler gameHandler;

        private readonly HashSet<Point> walls;
        private readonly List<Unit> elves;
        private readonly List<Unit> goblins;
        private int rounds;

        public Game(HashSet<Point> walls, List<Unit> elves, List<Unit> goblins, IGameEventHandler gameHandler)
        {
            this.gameHandler = gameHandler;
            this.walls = walls;
            this.elves = elves;
            this.goblins = goblins;
        }

        public List<Unit> AliveElves => elves.Where(e => e.IsAlive).ToList();
        public List<Unit> AliveGoblins => goblins.Where(g => g.IsAlive).ToList();
        public List<Unit> AliveUnits => AliveElves.Concat(AliveGoblins).ToList();
        public List<Unit> AliveUnitsInOrder => AliveUnits
            .OrderBy(u => u.Position.Y)
            .ThenBy(u => u.Position.X)
            .ToList();

        public HashSet<Point> Occupied
        {
            get
            {
                var occupied = new HashSet<Point>(walls);
                AliveElves.ForEach(e => occupied.Add(e.Position));
                AliveGoblins.ForEach(g => occupied.Add(g.Position));
                return occupied;
            }
        }

        public static Game Create(IList<string> input, IGameEventHandler gameHandler)
        {
            var walls = new HashSet<Point>();
            var elves = new List<Unit>();
            var goblins = new List<Unit>();
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
                            elves.Add(new Unit(point, c, goblins, 200, 3, gameHandler));
                            break;
                        case 'G':
                            goblins.Add(new Unit(point, c, elves, 200, 3, gameHandler));
                            break;
                    }
                }
            }
            var game = new Game(walls, elves, goblins, gameHandler);
            gameHandler.SetGame(game);
            return game;
        }

        public int Run()
        {
            bool gameOver = false;

            while (!gameOver)
            {
                var order = AliveUnitsInOrder;

                for (int u = 0; u < order.Count; u++)
                {
                    var unit = order[u];

                    gameHandler.ReportCurrentUnitTurn(unit);

                    if (unit.IsDead) continue;

                    if (!unit.AliveTargets.Any())
                    {
                        var winners = AliveGoblins.Any() ? "Goblins" : "Elves";
                        gameHandler.ReportGameOver(rounds, winners, AliveUnits.Sum(alive => alive.HitPoints), rounds * AliveUnits.Sum(alive => alive.HitPoints));
                        gameOver = true;
                        break;
                    }

                    //MoveUnit(unit); trying to move responsibility for movement to game but something is wrong
                    unit.Move(Occupied, walls);
                    unit.Attack();
                }

                if (!gameOver)
                {
                    rounds++;
                }
            }

            return rounds * AliveUnits.Sum(u => u.HitPoints);
        }

        private void MoveUnit(Unit unit)
        {
            ReportPosition(unit);

            var moveDirections = unit.Position.AdjacentInMainDirections().ToHashSet();

            var targetsInRange = unit.AliveTargets
                .Where(t => moveDirections.Contains(t.Position))
                .ToList();

            if (targetsInRange.Any(t => moveDirections.Contains(t.Position)))
            {
                return;
            }

            var adjacent = unit.AliveTargets.SelectMany(t => t.Position.AdjacentInMainDirections()).ToHashSet();
            adjacent.ExceptWith(Occupied);

            ReportPositions(adjacent, GameEvent.AdjacentToTarget);

            var reachable = new List<Node<Point>>();

            bool IsWalkable(Point point)
            {
                return !Occupied.Contains(point);
            }

            foreach (var movePosition in moveDirections.Where(IsWalkable))
            {
                foreach (var targetPosition in adjacent)
                {
                    IEnumerable<Point> Neighbors(Point p)
                    {
                        return p.AdjacentInMainDirections().Where(IsWalkable);
                    }

                    var targetNode = movePosition.ShortestPath(Neighbors, p => p == targetPosition);

                    if (targetNode != null)
                    {
                        reachable.Add(targetNode);
                    }
                }
            }

            if (!reachable.Any()) return;

            reachable = reachable
                .OrderBy(n => n.Depth)
                .ThenBy(n => n.Start.Data.Y)
                .ThenBy(n => n.Start.Data.X)
                .ToList();

            var shortestPath = reachable.First();

            ReportPositions(new HashSet<Point> { shortestPath.Start.Data }, GameEvent.AdjacentReachableForShortestPath);
            ReportPositions(new HashSet<Point> { shortestPath.Start.Data }, GameEvent.WalkToForShortestPath);

            unit.Position = shortestPath.Start.Data;
        }

        private void ReportPosition(Unit unit)
        {
            gameHandler.ReportEvent(new GameSituationEvent
            {
                Positions = new HashSet<Point>(new[] { unit.Position }),
                Description = unit.Class,
                EventType = GameEvent.UnitPosition
            });
        }

        private void ReportPositions(HashSet<Point> positions, GameEvent gameEvent)
        {
            gameHandler.ReportEvent(new GameSituationEvent
            {
                Positions = positions,
                Description = '+',
                EventType = gameEvent
            });
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            var aliveGoblins = goblins.Where(g => !g.IsDead).ToList();
            var aliveElves = elves.Where(e => !e.IsDead).ToList();
            s.AppendLine($"Rounds: {rounds} Goblins {aliveGoblins.Count} Elves: {aliveElves.Count}");
            s.AppendLine($"Goblin score: {aliveGoblins.Sum(g => g.HitPoints)} Elves score: {aliveElves.Sum(e => e.HitPoints)}");
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    var point = new Point(x, y);
                    var print = ".";
                    if (walls.Contains(point))
                    {
                        print = "#";
                    }
                    else if (elves.Exists(e => e.Position == point && !e.IsDead))
                    {
                        print = "E";
                    }
                    else if (goblins.Exists(g => g.Position == point && !g.IsDead))
                    {
                        print = "G";
                    }

                    s.Append(print);
                }

                s.Append(" ");

                var units = aliveGoblins.Concat(aliveElves)
                    .Where(unit => unit.Position.Y == y)
                    .OrderBy(unit => unit.Position.X);

                foreach (var unit in units)
                {
                    s.Append($" {unit.Class}({unit.HitPoints})");
                }
                s.AppendLine();
            }

            return s.ToString();
        }
    }

    public class Unit
    {
        private readonly IGameEventHandler game;

        public Point Position { get; set; }
        public char Class { get; }
        public List<Unit> Targets { get; }
        public List<Unit> AliveTargets => Targets.Where(t => !t.IsDead).ToList();
        public int HitPoints { get; private set; }
        public int AttackPower { get; private set; }
        public bool IsAlive => !IsDead;
        public bool IsDead => HitPoints <= 0;

        public Unit(Point position, char @class, List<Unit> targets, int hitPoints, int attackPower, IGameEventHandler game)
        {
            this.game = game;
            Position = position;
            Class = @class;
            Targets = targets;
            HitPoints = hitPoints;
            AttackPower = attackPower;
        }

        public void UpdateAttackPower(int newAttackPower)
        {
            AttackPower = newAttackPower;
        }

        private void ReportPosition()
        {
            game.ReportEvent(new GameSituationEvent
            {
                Positions = new HashSet<Point>(new[] { Position }),
                Description = Class,
                EventType = GameEvent.UnitPosition
            });
        }

        private void ReportPositions(HashSet<Point> positions, GameEvent gameEvent)
        {
            game.ReportEvent(new GameSituationEvent
            {
                Positions = positions,
                Description = '+',
                EventType = gameEvent
            });
        }

        public void Move(HashSet<Point> occupied, HashSet<Point> walls)
        {
            ReportPosition();

            var moveDirections = Position.AdjacentInMainDirections().ToHashSet();

            var targetsInRange = AliveTargets
                .Where(t => moveDirections.Contains(t.Position))
                .ToList();

            if (targetsInRange.Any(t => moveDirections.Contains(t.Position)))
            {
                return;
            }

            var adjacent = AliveTargets.SelectMany(t => t.Position.AdjacentInMainDirections()).ToHashSet();
            adjacent.ExceptWith(occupied);

            ReportPositions(adjacent, GameEvent.AdjacentToTarget);

            var reachable = new List<Node<Point>>();

            bool IsWalkable(Point point)
            {
                return !occupied.Contains(point);
            }

            foreach (var movePosition in moveDirections.Where(IsWalkable))
            {
                foreach (var targetPosition in adjacent)
                {
                    IEnumerable<Point> Neighbors(Point p)
                    {
                        return p.AdjacentInMainDirections().Where(IsWalkable);
                    }

                    var targetNode = movePosition.ShortestPath(Neighbors, p => p == targetPosition);

                    if (targetNode != null)
                    {
                        reachable.Add(targetNode);
                    }
                }
            }

            if (!reachable.Any()) return;

            reachable = reachable
                .OrderBy(n => n.Depth)
                .ThenBy(n => n.Start.Data.Y)
                .ThenBy(n => n.Start.Data.X)
                .ToList();

            var shortestPath = reachable.First();

            ReportPositions(new HashSet<Point> { shortestPath.Start.Data }, GameEvent.AdjacentReachableForShortestPath);
            ReportPositions(new HashSet<Point> { shortestPath.Start.Data }, GameEvent.WalkToForShortestPath);

            Position = shortestPath.Start.Data;
        }

        public void Attack()
        {
            var adjacent = Position.AdjacentInMainDirections().ToHashSet();

            var soldierToAttack = AliveTargets
                .Where(t => adjacent.Contains(t.Position))
                .OrderBy(t => t.HitPoints)
                .ThenBy(t => t.Position.Y)
                .ThenBy(t => t.Position.X)
                .FirstOrDefault();

            if (soldierToAttack == null) return;

            game.ReportEvent(new GameSituationEvent
            {
                Positions = new HashSet<Point> { soldierToAttack.Position },
                Description = soldierToAttack.Class,
                EventType = GameEvent.UnitAttacked
            });

            soldierToAttack.HitPoints -= AttackPower;
        }

        public override string ToString()
        {
            return $"{Class} ({Position.X},{Position.Y}) [{HitPoints}]";
        }
    }

    public enum GameEvent
    {
        AdjacentToTarget,
        ReachableAdjacentToTarget,
        AdjacentReachableForShortestPath,
        WalkToForShortestPath,
        UnitAttacked,
        UnitPosition
    }

    public class GameSituationEvent
    {
        public char Description { get; set; }
        public HashSet<Point> Positions { get; set; }
        public GameEvent EventType { get; set; }

    }

    public interface IGameEventHandler
    {
        void ReportEvent(GameSituationEvent @event);
        void ReportGameOver(int rounds, string winners, int score, int outcome);
        void ReportCurrentUnitTurn(Unit unit);
        void SetGame(Game game);
    }

    public class ConsoleUi : IGameEventHandler
    {
        private Game game;

        private static readonly Dictionary<GameEvent, ConsoleColor> UpdateColors =
            new Dictionary<GameEvent, ConsoleColor>
            {
                {GameEvent.AdjacentToTarget, ConsoleColor.Magenta},
                {GameEvent.ReachableAdjacentToTarget, ConsoleColor.Green},
                {GameEvent.WalkToForShortestPath, ConsoleColor.DarkYellow},
                {GameEvent.AdjacentReachableForShortestPath, ConsoleColor.DarkYellow},
                {GameEvent.UnitPosition, ConsoleColor.Green },
                {GameEvent.UnitAttacked, ConsoleColor.Red }
            };

        public void ReportEvent(GameSituationEvent @event)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = UpdateColors[@event.EventType];
            foreach (var position in @event.Positions)
            {
                Console.SetCursorPosition(position.X, position.Y + 3);
                Console.Write(@event.Description);
            }

            Console.ForegroundColor = currentColor;
            Console.ReadKey(true);
        }

        public void ReportGameOver(int rounds, string winners, int score, int outcome)
        {
            Console.WriteLine($"Game over: round {rounds}, {winners} won with {score} outcome {outcome}");
        }

        public void ReportCurrentUnitTurn(Unit unit)
        {
            Console.Clear();
            Console.WriteLine($"Current soldier: {unit}");
            Console.Write(game.ToString());
        }

        public void SetGame(Game game)
        {
            this.game = game;
        }
    }

    public class NoUi : IGameEventHandler
    {
        public void ReportEvent(GameSituationEvent @event)
        {
        }

        public void ReportGameOver(int rounds, string winners, int score, int outcome)
        {
        }

        public void ReportCurrentUnitTurn(Unit unit)
        {
        }

        public void SetGame(Game game)
        {

        }
    }

    public class Node<T>
    {
        public T Data { get; }
        public int Depth { get; }
        public Node<T> Parent { get; }

        public Node<T> Start
        {
            get
            {
                var walkTo = this;
                while (walkTo.Parent != null)
                {
                    walkTo = walkTo.Parent;
                }

                return walkTo;
            }
        }

        public IList<T> Path
        {
            get
            {
                var path = new List<T>();
                var node = this;
                path.Add(node.Data);
                while (node.Parent != null)
                {
                    node = node.Parent;
                    path.Add(node.Data);
                }

                path.Reverse();

                return path;
            }
        }

        public Node(T data, int depth, Node<T> parent)
        {
            Data = data;
            Depth = depth;
            Parent = parent;
        }
    }
    public static class PointExtensions
    {
        public static Node<T> ShortestPath<T>(this T start,
            Func<T, IEnumerable<T>> neighborFetcher,
            Predicate<T> targetCondition)
        {
            var visited = new HashSet<T>();

            var queue = new Queue<Node<T>>();
            Node<T> terminationNode = null;

            queue.Enqueue(new Node<T>(start, 0, null));

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();

                if (!visited.Add(current.Data))
                {
                    continue;
                }

                if (targetCondition(current.Data))
                {
                    terminationNode = current;
                    break;
                }

                var neighbors = neighborFetcher(current.Data).Where(n => !visited.Contains(n)).ToList();

                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(new Node<T>(neighbor, current.Depth + 1, current));
                }
            }

            return terminationNode;
        }
    }
}
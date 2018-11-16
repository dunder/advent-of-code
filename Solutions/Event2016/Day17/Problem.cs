
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.Crypto;
using Shared.MapGeometry;
using Shared.Tree;

namespace Solutions.Event2016.Day17
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day17;
        private const string Passcode = "edjrjqaa";

        public override string FirstStar()
        {
            var path = ShortestPath(Passcode);
            return path;
        }

        public override string SecondStar()
        {
            var result = LongestPath(Passcode);

            return result.ToString();
        }

        public static int LongestPath(string passcode)
        {
            var initialState = new State(passcode, new Point(0, 0), "");
            var targetPosition = new Point(3, 3);

            var (_, visited) = initialState.DepthFirst((state) => state.Neighbors(),
                (state) => state.Data.Position == targetPosition,
                continueOnTargetCondition: true);

            return visited.Where(v => v.Position == targetPosition).Max(v => v.Path.Length);
        }

        public static string ShortestPath(string passcode)
        {

            var initialState = new State(passcode, new Point(0,0), "");
            var targetPosition = new Point(3,3);
            var (terminatingState, _) = initialState.ShortestPath((state) => state.Neighbors(), (state) => state.Position == targetPosition);

            return terminatingState.Data.Path;
        }

        public static bool IsDoorOpen(char c)
        {
            switch (c)
            {
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                    return true;
                default:
                    return false;
            }
        }

        public class State
        {
            public string CurrentPassCode { get; }
            public Point Position { get; set; }
            public string Path { get; set; }

            private const string Up = "U";
            private const string Down = "D";
            private const string Left = "L";
            private const string Right = "R";

            public State(string currentPassCode, Point position, string path)
            {
                CurrentPassCode = currentPassCode;
                Position = position;
                Path = path;
            }

            public List<State> Neighbors()
            {
                string hash = Md5.Hash(CurrentPassCode);
                var neighbors = new List<State>();

                bool IsWithinBounds(Point p)
                {
                    return p.X >= 0 && p.Y >= 0 && p.X < 4 && p.Y < 4;
                }

                if (IsDoorOpen(hash[0]))
                {
                    var newPosition = Position.Move(Direction.North);
                    if (IsWithinBounds(newPosition))
                    {
                        var up = new State(CurrentPassCode + Up, newPosition, Path + Up);
                        neighbors.Add(up);
                    }
                }

                if (IsDoorOpen(hash[1]))
                {
                    var newPosition = Position.Move(Direction.South);
                    if (IsWithinBounds(newPosition))
                    {
                        var down = new State(CurrentPassCode + Down, newPosition, Path + Down);
                        neighbors.Add(down);
                    }
                }

                if (IsDoorOpen(hash[2]))
                {
                    var newPosition = Position.Move(Direction.West);
                    if (IsWithinBounds(newPosition))
                    {
                        var left = new State(CurrentPassCode + Left, newPosition, Path + Left);
                        neighbors.Add(left);
                    }
                }

                if (IsDoorOpen(hash[3]))
                {
                    var newPosition = Position.Move(Direction.East);
                    if (IsWithinBounds(newPosition))
                    {
                        var right = new State(CurrentPassCode + Right, newPosition, Path + Right);
                        neighbors.Add(right);
                    }
                }
                return neighbors;
            }
        }
    }
}
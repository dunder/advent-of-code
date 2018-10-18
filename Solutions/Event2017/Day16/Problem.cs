using Shared.Extensions;

namespace Solutions.Event2017.Day16
{
    public class Problem : ProblemBase
    {
        private const string Seed = "abcdefghijklmnop";
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day16;

        public override string FirstStar()
        {
            var input = ReadInput();
            var result = Dancing.Read(Seed, input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = Dancing.Read(Seed, input, 1_000_000_000);
            return result.ToString();
        }
    }
    public class Dancing {

        public static string Read(string positions, string input, int times) {
            var temp = positions;
            int roundedAfter = 0;
            for (int i = 0; i < times; i++) {
                foreach (var instruction in input.SplitOnCommaSpaceSeparated()) {
                    positions = TakeInstruction(positions, instruction);
                }
                if (temp.Equals(positions)) {
                    roundedAfter = i + 1;
                    break;
                }
            }
            for (int i = 0; i < times % roundedAfter; i++) {
                foreach (var instruction in input.SplitOnCommaSpaceSeparated()) {
                    positions = TakeInstruction(positions, instruction);
                }
            }
            return positions;
        }

        public static string Read(string positions, string input) {
            foreach (var instruction in input.SplitOnCommaSpaceSeparated()) {
                positions = TakeInstruction(positions, instruction);
            }
            return positions;
        }

        private static string TakeInstruction(string positions, string instruction) {
            switch (instruction) {
                case var spin when instruction.StartsWith("s"):
                    int index = int.Parse(spin.TrimStart('s'));
                    var part1 = positions.Substring(positions.Length - index, index);
                    var part2 = positions.Substring(0, positions.Length - part1.Length);
                    positions = part1 + part2;
                    break;
                case var exchange when instruction.StartsWith("x"):
                    var programs = exchange.Trim('x').Split('/');
                    int index1 = int.Parse(programs[0]);
                    int index2 = int.Parse(programs[1]);
                    char[] p = positions.ToCharArray();
                    char tmp = p[index1];
                    p[index1] = p[index2];
                    p[index2] = tmp;
                    positions = new string(p);
                    break;
                case var partner when instruction.StartsWith("p"):
                    var swap = partner.Substring(1).Split('/');
                    string letter1 = swap[0];
                    string letter2 = swap[1];
                    positions = positions.Replace(letter1, "x");
                    positions = positions.Replace(letter2, letter1);
                    positions = positions.Replace("x", letter2);
                    break;
            }
            return positions;
        }
    }

}
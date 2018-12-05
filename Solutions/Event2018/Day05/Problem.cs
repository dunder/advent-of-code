using System.Text;

namespace Solutions.Event2018.Day05
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day05;

        public override string FirstStar()
        {
            var input = ReadInput();
            var result = ReduceAll(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public static int ReduceAll(string polymer)
        {
            string previous = "";
            while (previous != polymer)
            {
                previous = polymer;
                polymer = ReactReduce(polymer);
            }

            return polymer.Length;
        }

        public static string ReactReduce(string polymer)
        {
            var result = new StringBuilder();
            for (int i = 0; i < polymer.Length - 1; i++)
            {
                var p1 = polymer[i];
                var p2 = polymer[i + 1];

                if (p1 == p2)
                {
                    result.Append(p1);
                    continue;
                };

                if (char.ToLower(p1) == char.ToLower(p2))
                {
                    i++;
                    continue;
                }

                result.Append(p1);
                if (i == polymer.Length - 2)
                {
                    result.Append(p2);
                }
            }

            return result.ToString();
        }
    }
}
using System;
using System.Linq;
using System.Text;

namespace Solutions.Event2016.Day16
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day16;

        private const string Input = "11100010111110100";

        public override string FirstStar()
        {
            var checksum = CalculateChecksum(Input, diskSize: 272);
            return checksum;
        }

        public override string SecondStar()
        {
            var checksum = CalculateChecksum(Input, diskSize: 35651584);
            return checksum;
        }

        public static string Process(string input, int diskSize)
        {
            string output = Generate(input);
            while (output.Length < diskSize)
            {
                output = Generate(output);
            }

            return output;
        }
        public static string Generate(string input)
        {
            var a = input;
            var b = new string(input.Reverse().ToArray());
            var temp = b.Replace('0', 'z');
            temp = temp.Replace('1', '0');
            temp = temp.Replace('z', '1');
            return $"{a}0{temp}";
        }

        public static string Checksum(string input)
        {
            if (input.Length % 2 != 0) throw new ArgumentOutOfRangeException(nameof(input), "Must be even length");

            var outputBuilder = new StringBuilder();
            for (int i = 0; i < input.Length-1;i += 2)
            {
                outputBuilder.Append(input[i] == input[i + 1] ? "1" : "0");
            }

            return  outputBuilder.Length % 2 == 0 ? Checksum(outputBuilder.ToString()) : outputBuilder.ToString();
        }

        public static string CalculateChecksum(string input, int diskSize)
        {
            var diskContent = Process(input, diskSize);
            var checksum = Checksum(diskContent.Substring(0, diskSize));
            return checksum;
        }
    }
}
using Shared.Extensions;

namespace Solutions.Event2017.Day01
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day01;
        public override string FirstStar()
        {
            var input = ReadInput();
            return Captcha(input);
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            return CaptchaHalfway(input);
        }

        public static string Captcha(string input)
        {
            int sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var index = i;
                var nextIndex = input.ToCharArray().WrappedIndex(i + 1);
                char digit1 = input[index];
                char digit2 = input[nextIndex];
                if (digit1 == digit2)
                {
                    sum += int.Parse(new string(new[] {digit1}));
                }
            }

            return sum.ToString();
        }

        public static string CaptchaHalfway(string input)
        {
            int sum = 0;
            for (int i = 0; i < input.Length; i++) {
                var index = i;
                var nextIndex = i + input.Length / 2;
                if (nextIndex > input.Length - 1) {
                    nextIndex = nextIndex % input.Length;
                }
                char digit1 = input[index];
                char digit2 = input[nextIndex];
                if (digit1 == digit2) {
                    sum += int.Parse(new string(new[] { digit1 }));
                }
            }
            return sum.ToString();
        }
    }
}
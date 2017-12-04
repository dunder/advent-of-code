using Utilities;

namespace Y2017 {
    public class Captcha {
        public static int Read(string input) {
            int sum = 0;
            for (int i = 0; i < input.Length; i++) {
                var index = i;
                var nextIndex = input.ToCharArray().WrappedIndex(i + 1);
                char digit1 = input[index];
                char digit2 = input[nextIndex];
                if (digit1 == digit2) {
                    sum += int.Parse(new string(new[] { digit1 }));
                }
            }
            return sum;
        }
        public static int ReadHalfway(string input) {
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
            return sum;
        }
    }
}

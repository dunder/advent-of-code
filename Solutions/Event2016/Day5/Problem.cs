using System.Linq;
using System.Text;
using Shared.Crypto;

namespace Solutions.Event2016.Day5
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day5;

        public override string FirstStar()
        {
            var input = "reyedfim";
            var result = PasswordGenerator.GenerateParallel(input, 8);
            return result;
        }

        public override string SecondStar()
        {
            var input = "reyedfim";
            var result = PasswordGenerator.GenerateNew(input, 8);
            return result;
        }
    }

    public static class PasswordGenerator {

        public static string GenerateParallel(string input, int length) {
            int startIndex = 0;
            int count = 1000000;
            StringBuilder password = new StringBuilder();
            while (true) {
                var validHashIndices = Enumerable.Range(startIndex, count)
                    .AsParallel()
                    .Where(x => Md5.Hash(input + x).StartsWith("00000"))
                    .ToList()
                    .OrderBy(x => x)
                    .ToList();
                if (validHashIndices.Any()) {
                    foreach (var validHashIndex in validHashIndices) {
                        password.Append(Md5.Hash(input + validHashIndex)[5]);
                    }
                }
                if (password.Length >= length) {
                    break;
                }
                startIndex += count;
            }

            return password.ToString().Substring(0, length);
        }

        public static string GenerateNew(string input, int length) {
            int startIndex = 0;
            int count = 1000000;
            char?[] password = new char?[length];
            while (true) {
                var validHashIndeces = Enumerable.Range(startIndex, count)
                    .AsParallel()
                    .Where(x => Md5.Hash(input + x).StartsWith("00000"))
                    .ToList()
                    .OrderBy(x => x);
                if (validHashIndeces.Any()) {
                    foreach (var validHashIndex in validHashIndeces) {
                        var hash = Md5.Hash(input + validHashIndex);

                        if (int.TryParse(hash[5].ToString(), out int hashIndex) && hashIndex >= 0 && hashIndex < length && password[hashIndex] == null) {
                            password[hashIndex] = hash[6];
                        }
                    }
                }
                if (password.All(c => c.HasValue)) {
                    break;
                }
                startIndex += count;
            }

            return new string(password.Select(c => (char)c).ToArray());
        }
    }

}
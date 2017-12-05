using System.Text;
using Utilities;

namespace Y2016 {
    public static class PasswordGenerator {

        public static string Generate(string input, int length) {
            int index = 0;
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < length; i++) {
                while (true) {
                    var hash = Md5.Hash(input + index++);
                    if (hash.StartsWith("00000")) {
                        password.Append(hash[5].ToString());
                        break;
                    }
                }
            }
            return password.ToString();
        }

        public static string GenerateNew(string input, int length) {
            int index = 0;
            string[] password = new string[length];
            for (int i = 0; i < length; i++) {
                while (true) {
                    var hash = Md5.Hash(input + index++);
                    if (hash.StartsWith("00000")) {
                        if (int.TryParse(hash[5].ToString(), out int position) && position < length) {
                            password[position] = hash[6].ToString();
                            break;
                        }
                        
                    }
                }
            }
            return string.Join("", password);
        }
    }
}

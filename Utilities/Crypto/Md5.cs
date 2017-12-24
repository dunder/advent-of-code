using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Crypto {
    public class Md5 {
        public static string Hash(string source) {
            using (MD5 md5 = MD5.Create()) {
                return GetMd5Hash(md5, source);
            }
        }

        public static bool Verify(string input, string hash) {
            using (MD5 md5 = MD5.Create()) {
                return VerifyMd5Hash(md5, input, hash);
            }
        }

        private static string GetMd5Hash(MD5 md5, string input) {
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            foreach (byte b in data) {
                sBuilder.Append(b.ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private static bool VerifyMd5Hash(MD5 md5, string input, string hash) {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

    }
}

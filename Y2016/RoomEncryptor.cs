using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Y2016 {
    public class RoomEncryptor {

        public static string[] Decrypt(string[] input) {
            return input.Select(Decrypt).ToArray();
        }

        public static string Decrypt(string input) {
            var room = Regex.Match(input, @"([a-z]*-)*").Value;
            var sector = int.Parse(Regex.Match(input, @"[0-9]+").Value);
            var roomCharacters = room.ToCharArray(); 
            for (int i = 0; i < roomCharacters.Length; i++) {
                char c = roomCharacters[i];
                if (c == '-') {
                    continue;
                }
                var decryptedCharacter = roomCharacters[i] + sector % 26;
                if (decryptedCharacter > 122) {
                    decryptedCharacter -= 26;
                }
                roomCharacters[i] = (char) decryptedCharacter;
                
            }
            return input.Replace(room, new string(roomCharacters));
        }

        public static string SectorOf(string[] input) {
            var result = input.First(x => x.Contains("north") && x.Contains("pole"));
            return Regex.Match(result, @"[0-9]+").Value;
        }
        public static int CountCorrectRoomDescriptors(string[] input) {
            int total = 0;
            foreach (var descriptor in input) {

                var room = Regex.Match(descriptor, @"([a-z]*-)*").Value;
                room = Regex.Replace(room, @"-", "");
                var checksum = Regex.Match(descriptor, @"\[[a-z]*\]").Value.Trim('[', ']');
                var count = new Dictionary<char, int>();
                foreach (var c in room) {
                    if (count.ContainsKey(c)) {
                        count[c] += 1;
                    }
                    else {
                        count.Add(c, 1);
                    }
                }
                var evaluate = string.Join("", count.OrderByDescending(kvp => kvp.Value).ThenBy(kvp => kvp.Key).Take(5).Select(kvp => kvp.Key));
                var sector = Regex.Match(descriptor, @"[0-9]+").Value;

                if (checksum.Equals(evaluate)) {
                    total += int.Parse(sector);
                }
            }
            return total;
        }
    }
}
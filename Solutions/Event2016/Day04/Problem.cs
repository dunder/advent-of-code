using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Extensions;

namespace Solutions.Event2016.Day04
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day04;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = RoomEncryptor.CountCorrectRoomDescriptors(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = RoomEncryptor.SectorOf(RoomEncryptor.Decrypt(input));
            return result;
        }
    }

    public class RoomEncryptor {

        private class RoomDescriptor {
            private readonly string _descriptor;

            public RoomDescriptor(string descriptor) {
                _descriptor = descriptor;
            }

            public string Room => Regex.Match(_descriptor, @"([a-z]*-)*").Value;
            public int Sector => int.Parse(Regex.Match(_descriptor, @"[0-9]+").Value);
            public string CheckSum => Regex.Match(_descriptor, @"\[[a-z]*\]").Value.Trim('[', ']');
        }

        public static string[] Decrypt(IList<string> input) {
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

                roomCharacters[i] = roomCharacters[i].Shift(sector);

            }
            return input.Replace(room, new string(roomCharacters));
        }

        public static string SectorOf(IList<string> input) {
            var result = input.First(x => x.Contains("north") && x.Contains("pole"));
            return Regex.Match(result, @"[0-9]+").Value;
        }

        public static int CountCorrectRoomDescriptors(IList<string> input) {
            int total = 0;

            foreach (var descriptor in input) {

                var room = Regex.Match(descriptor, @"([a-z]*-)*").Value;
                room = Regex.Replace(room, @"-", "");
                var checksum = Regex.Match(descriptor, @"\[[a-z]*\]").Value.Trim('[', ']');
                var count = new Dictionary<char, int>();
                foreach (var c in room) {
                    if (count.ContainsKey(c)) {
                        count[c] += 1;
                    } else {
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
using System.Collections.Generic;
using System.Linq;
using Shared.Crypto;

namespace Solutions.Event2016.Day14
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day14;

        public const string Salt = "ihaygndm";

        public override string FirstStar()
        {
            var result = FirstStarSolution(Salt);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var result = SecondStarSolution(Salt);
            return result.ToString();
        }

        public static int FirstStarSolution(string salt)
        {
            return HashSolution(salt, new SimpleHash());
        }

        public static int SecondStarSolution(string salt)
        {
            return HashSolution(salt, new StretchedHash());
        }

        public interface IHashStrategy
        {
            string Hash(string input);
        }

        public class StretchedHash : IHashStrategy
        {
            public string Hash(string input)
            {
                string hash = Md5.Hash(input);
                for (int i = 0; i < 2016; i++)
                {
                    hash = Md5.Hash(hash);
                }

                return hash;
            }
        }

        public class SimpleHash : IHashStrategy
        {
            public string Hash(string input)
            {
                return Md5.Hash(input);
            }
        }

        public static char? FirstRepeatedCharacter(string input, int repeatedTimes)
        {
            if (repeatedTimes > input.Length) return null;

            for (int i = 0; i < input.Length - repeatedTimes + 1;)
            {
                char current = input[i];
                
                int forwardIndex = i + 1;
                int count = 1;
                while (forwardIndex < input.Length && input[forwardIndex] == current)
                {
                    forwardIndex++;
                    count++;
                    if (count >= repeatedTimes)
                    {
                        return current;
                    }
                }

                i = forwardIndex;
            }

            return null;
        }

        public static int HashSolution(string salt, IHashStrategy hashStrategy)
        {
            var keys = new List<string>();
            var hashCache = new Dictionary<int, string>();

            string GetHash(int index)
            {
                if (!hashCache.ContainsKey(index))
                {
                    hashCache[index] = hashStrategy.Hash(salt + index);
                }

                return hashCache[index];
            }

            for (int index = 0;;index++)
            {
                var hash = hashStrategy.Hash(salt + index);

                var c = FirstRepeatedCharacter(hash, 3);

                if (c.HasValue)
                {
                    for (int promoteIndex = index + 1; promoteIndex < index + 1000; promoteIndex++)
                    {
                        string promotionHash = GetHash(promoteIndex);

                        var fiveC = Enumerable.Repeat(c.Value, 5);
                        if (promotionHash.Contains(new string(fiveC.ToArray())))
                        {
                            keys.Add(hash);
                            if (keys.Count == 64)
                            {
                                return index;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
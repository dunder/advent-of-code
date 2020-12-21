using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 21: Allergen Assessment ---

    public class Day21
    {
        private readonly ITestOutputHelper output;

        public Day21(ITestOutputHelper output)
        {
            this.output = output;
        }

        public List<(string, HashSet<string>)> Parse(List<string> lines)
        {
            var allergensResult = new List<(string, HashSet<string>)>();
             
            foreach (var line in lines)
            {
                var start = line.IndexOf("(");
                var end = line.IndexOf(")");

                var wordPart = line.Substring(0, start - 1);
                var words = wordPart.Split(" ");

                var allergenPart = line.Substring(start, end - start);

                allergenPart = allergenPart.Substring("(contains".Length);

                var allergens = allergenPart
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim());

                foreach (var allergen in allergens)
                {
                    allergensResult.Add(((allergen, new HashSet<string>(words))));
                }
            }

            return allergensResult;
        }

        private static HashSet<string> AllWords(List<(string, HashSet<string>)> allergenCollection)
        {
            var allWords = new HashSet<string>();

            foreach (var allergen in allergenCollection)
            {
                allWords.UnionWith(allergen.Item2);
            }

            return allWords;
        }
        private static List<(string, HashSet<string>)> PossibleAllergens(List<(string, HashSet<string>)> allergenCollection)
        {
            var nonAllergens = new HashSet<string>();

            var allergenGroups = allergenCollection.GroupBy(x => x.Item1);

            var possibleAllergens = new List<(string, HashSet<string>)>();

            foreach (var allergenGroup in allergenGroups)
            {
                HashSet<string> intersection = null;
                foreach (var word in allergenGroup)
                {
                    if (intersection == null)
                    {
                        intersection = word.Item2;
                    }
                    else
                    {
                        intersection.IntersectWith(word.Item2);
                    }
                }
                possibleAllergens.Add((allergenGroup.Key, intersection));
            }

            return possibleAllergens;
        }

        private int CountOccurances(List<string> ingredientList)
        {
            var allergens = Parse(ingredientList);
            var allWords = AllWords(allergens);
            var possibleAllergens = PossibleAllergens(allergens);
            var allPossible = new HashSet<string>();
            foreach (var possible in possibleAllergens)
            {
                allPossible.UnionWith(possible.Item2);
            }
            var nonAllergens = new HashSet<string>(allWords);
            nonAllergens.ExceptWith(allPossible);


            int count = 0;
            foreach (var food in ingredientList)
            {
                var words = food.Split(" ");
                count += words.Count(word => nonAllergens.Contains(word));
            }

            return count;
        }

        private string CanonicalDangerousIngredients(List<string> ingredientList)
        {
            var allergens = Parse(ingredientList);
            var allWords = AllWords(allergens);
            var possibleAllergens = PossibleAllergens(allergens);

            var certainAllergens = new HashSet<string>();
            var certainByIngredient = new List<(string, string)>();

            while (possibleAllergens.Any(x => x.Item2.Count > 1))
            {
                var certainSingles = possibleAllergens
                    .Where(x => x.Item2.Count == 1)
                    .Select(x => x.Item2.Single());

                var certainSinglesSet = new HashSet<string>(certainSingles);

                certainAllergens.UnionWith(certainSinglesSet);

                possibleAllergens = possibleAllergens.Select(x =>
                {
                    if (x.Item2.Count > 1)
                    {
                        x.Item2.ExceptWith(certainAllergens);
                    }
                    return (x.Item1, x.Item2);
                }).ToList();
            }

            var orderedByIngredient = possibleAllergens.OrderBy(x => x.Item1).ToList();

            return string.Join(",", orderedByIngredient.Select(x => x.Item2.Single()));
        }

        public long FirstStar()
        {
            var input = ReadLineInput().ToList();

            return CountOccurances(input);
        }

        public string SecondStar()
        {
            var input = ReadLineInput().ToList();

            return CanonicalDangerousIngredients(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(1679, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();
            Assert.Equal("lmxt,rggkbpj,mxf,gpxmf,nmtzlj,dlkxsxg,fvqg,dxzq", result);
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                "trh fvjkl sbzzf mxmxvkd (contains dairy)",
                "sqjhc fvjkl (contains soy)",
                "sqjhc mxmxvkd sbzzf (contains fish)"
            };

            var count = CountOccurances(input);

            Assert.Equal(5, count);
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string>
            {
                "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                "trh fvjkl sbzzf mxmxvkd (contains dairy)",
                "sqjhc fvjkl (contains soy)",
                "sqjhc mxmxvkd sbzzf (contains fish)"
            };

            var list = CanonicalDangerousIngredients(input);

            Assert.Equal("mxmxvkd,sqjhc,fvjkl", list);
        }
    }
}

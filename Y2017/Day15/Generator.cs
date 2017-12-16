using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Y2017.Day15 {
    public class Generator {
       

        public static int Judge(int generatorAstartValue, int generatorBstartValue) {

            const int generatorAFactor = 16807;
            const int generatorBFactor = 48271;
            const int factor = 2147483647;
            const int generations = 40_000_000;

            long[] generatorAValues = Generate(generatorAFactor, factor, generations, generatorAstartValue).ToArray();
            long[] generatorBValues = Generate(generatorBFactor, factor, generations, generatorBstartValue).ToArray();

            return CountMatches(generations, generatorAValues, generatorBValues);
        }

        public static int Judge2(int generatorAstartValue, int generatorBstartValue) {

            const int generatorAFactor = 16807;
            const int generatorBFactor = 48271;
            const int factor = 2147483647;
            const int generations = 5_000_000;

            long generatorA = generatorAstartValue;
            long generatorB = generatorBstartValue;

            List<long> genAmultiples = new List<long>();
            while (genAmultiples.Count < generations) {
                generatorA = generatorA * generatorAFactor % factor;

                if (generatorA % 4 == 0) {
                    genAmultiples.Add(generatorA);
                }
            }
            List<long> genBmultiples = new List<long>();
            while (genBmultiples.Count < generations) {

                generatorB = generatorB * generatorBFactor % factor;

                if (generatorB % 8 == 0) {
                    genBmultiples.Add(generatorB);
                }
            }

            return CountMatches(generations, genAmultiples.ToArray(), genBmultiples.ToArray());
        }

        private static int CountMatches(int generations, long[] generatorAValues, long[] generatorBValues) {
            int count = 0;

            Parallel.For(0,
                generations,
                (i, loopState) => {
                    var genABinary = Convert.ToString(generatorAValues[i], 2).PadLeft(32, '0');
                    var genBBinary = Convert.ToString(generatorBValues[i], 2).PadLeft(32, '0');

                    var genAlast16 = genABinary.Substring(genABinary.Length - 16, 16);
                    var genBlast16 = genBBinary.Substring(genBBinary.Length - 16, 16);
                    if (genAlast16.Equals(genBlast16)) {
                        Interlocked.Increment(ref count);
                    }
                });
            return count;
        }

        private static IEnumerable<long> Generate(int factor, int factor2, int limit, int initialValue) {
            long value = initialValue;
            for (int i = 0; i < limit; i++) {
                value = value * factor % factor2;
                yield return value;
            }
        }
    }
}
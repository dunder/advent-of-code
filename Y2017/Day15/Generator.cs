using System;
using System.Collections.Generic;
using System.Linq;

namespace Y2017.Day15 {
    public class Generator {
        public static int Judge(int generatorAstartValue, int generatorBstartValue) {

            const int generatorAFactor = 16807;
            const int generatorBFactor = 48271;
            const int factor = 2147483647;

            long generatorA = generatorAstartValue;
            long generatorB = generatorBstartValue;
            int count = 0;
            for (int i = 0; i < 40_000_000; i++) {
                generatorA = generatorA * generatorAFactor % factor;
                generatorB = generatorB * generatorBFactor % factor;

                var genABinary = Convert.ToString(generatorA, 2).PadLeft(32, '0');
                var genBBinary = Convert.ToString(generatorB, 2).PadLeft(32, '0');

                var genAlast16 = genABinary.Substring(genABinary.Length - 16, 16);
                var genBlast16 = genBBinary.Substring(genBBinary.Length - 16, 16);
                if (genAlast16.Equals(genBlast16)) {
                    count += 1;
                }
            }
            return count;
        }

        public static int Judge2(int generatorAstartValue, int generatorBstartValue) {

            const int generatorAFactor = 16807;
            const int generatorBFactor = 48271;
            const int factor = 2147483647;

            long generatorA = generatorAstartValue;
            long generatorB = generatorBstartValue;

            List<long> genAmultiples = new List<long>();
            while(genAmultiples.Count < 5_000_000) {
                generatorA = generatorA * generatorAFactor % factor;

                if (generatorA % 4 == 0) {
                    genAmultiples.Add(generatorA);
                }
            }
            List<long> genBmultiples = new List<long>();
            while (genBmultiples.Count < 5_000_000) {

                generatorB = generatorB * generatorBFactor % factor;

                if (generatorB % 8 == 0) {
                    genBmultiples.Add(generatorB);
                }
            }
            int count = 0;

            for (int i = 0; i < 5_000_000; i++) {
                var genABinary = Convert.ToString(genAmultiples[i], 2).PadLeft(32, '0');
                var genBBinary = Convert.ToString(genBmultiples[i], 2).PadLeft(32, '0');

                var genAlast16 = genABinary.Substring(genABinary.Length - 16, 16);
                var genBlast16 = genBBinary.Substring(genBBinary.Length - 16, 16);

                if (genAlast16.Equals(genBlast16)) {
                    count += 1;
                }
            }
            return count;
        }
    }
}
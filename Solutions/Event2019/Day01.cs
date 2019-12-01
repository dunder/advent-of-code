using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 1: The Tyranny of the Rocket Equation ---
    public class Day01
    {
        public static int FuelRequirement(int mass)
        {
            return mass / 3 - 2;
        }

        public static IEnumerable<int> FuelRequirements(int mass)
        {

            var fuelRequirement = FuelRequirement(mass);
            yield return fuelRequirement;
            while (fuelRequirement > 0)
            {
                fuelRequirement = FuelRequirement(fuelRequirement);
                if (fuelRequirement > 0)
                {
                    yield return fuelRequirement;
                }
            }
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return input.Select(int.Parse).Select(FuelRequirement).Sum();
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return input.Select(int.Parse).SelectMany(FuelRequirements).Sum();
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(3401852, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(5099916, SecondStar());
        }

        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 654)]
        [InlineData(100756, 33583)]
        public void FuelRequirementTest(int mass, int expectedFuelRequirement)
        {
            var fuelRequired = FuelRequirement(mass);

            Assert.Equal(expectedFuelRequirement, fuelRequired);
        }

        [Fact]
        public void FuelRequirementsTest()
        {
            var fuelRequired = FuelRequirements(100756);

            Assert.Collection(fuelRequired, 
                x => Assert.Equal(33583, x),
                x => Assert.Equal(11192, x),
                x => Assert.Equal(3728, x),
                x => Assert.Equal(1240, x),
                x => Assert.Equal(411, x),
                x => Assert.Equal(135, x),
                x => Assert.Equal(43, x),
                x => Assert.Equal(12, x),
                x => Assert.Equal(2, x));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Facet.Combinatorics;
using Shared.Extensions;

namespace Solutions.Event2016.Day11.EnumFlagsSolution
{
    public static class FloorExtensions
    {
        public static Floor Remove(this Floor floor, Floor toRemove)
        {
            return floor & ~toRemove;
        }

        public static bool IsChipsOnly(this Floor floor)
        {
            return (floor & FloorSetup.Generators) == Floor.Empty;
        }

        public static bool IsGeneratorsOnly(this Floor floor)
        {
            return (floor & FloorSetup.Chips) == Floor.Empty;
        }

        public static bool HasUnmatchedChip(this Floor floor)
        {
            var chips = floor & FloorSetup.Chips;
            var generators = floor & FloorSetup.Generators;

            Floor chipsGeneratorOverlay = (Floor)((int) chips << FloorSetup.ChipCount);
            Floor mismatch = chipsGeneratorOverlay ^ generators;
            Floor chipOverlay = (Floor)((int) mismatch >> FloorSetup.ChipCount);
            Floor mismatchedChips = chipOverlay & chips;

            return mismatchedChips != Floor.Empty;
        }

        public static bool IsSafe(this Floor floor)
        {
            return floor.IsChipsOnly() || floor.IsGeneratorsOnly() || !floor.HasUnmatchedChip();
        }

        public static IList<Floor> GenerateSafeCombinations(this Floor floor)
        {
            var anyCombined = new Combinations<Floor>(floor.GetFlags().Cast<Floor>().ToList(), 2)
                .Select(c => c.First() | c.Last());

            return anyCombined.Where(f => f.IsSafe()).ToList(); 
        }
    }
}
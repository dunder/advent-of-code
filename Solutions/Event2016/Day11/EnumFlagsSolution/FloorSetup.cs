namespace Solutions.Event2016.Day11.EnumFlagsSolution
{
    public static class FloorSetup
    {
        public static Floor Chips => 
            Floor.HydrogenChip | 
            Floor.LithiumChip | 
            Floor.PlutoniumChip |
            Floor.PromethiumChip |
            Floor.RutheniumChip |
            Floor.StrontiumChip |
            Floor.ThuliumChip;
        public static Floor Generators =>
            Floor.HydrogenGenerator |
            Floor.LithiumGenerator |
            Floor.PlutoniumGenerator |
            Floor.PromethiumGenerator |
            Floor.RutheniumGenerator |
            Floor.StrontiumGenerator |
            Floor.ThuliumGenerator;

        public static int ChipCount => 7;
    }
}
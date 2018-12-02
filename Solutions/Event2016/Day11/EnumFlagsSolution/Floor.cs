using System;

namespace Solutions.Event2016.Day11.EnumFlagsSolution
{
    [Flags]
    public enum Floor
    {
        //IsEmpty = 0,
        //HydrogenChip = 1,
        //LithiumChip = 2,
        //HydrogenGenerator = 4,
        //LithiumGenerator = 8,
        Empty = 0,
        HydrogenChip = 1,
        LithiumChip = 2,
        ThuliumChip = 4,
        PlutoniumChip = 8,
        StrontiumChip = 16,
        PromethiumChip = 32,
        RutheniumChip = 64,
        HydrogenGenerator = 128,
        LithiumGenerator = 256,
        ThuliumGenerator = 512,
        PlutoniumGenerator = 1024,
        StrontiumGenerator = 2048,
        PromethiumGenerator = 4096,
        RutheniumGenerator = 8192
    }
}
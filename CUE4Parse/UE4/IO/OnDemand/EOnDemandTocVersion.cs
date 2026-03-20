namespace CUE4Parse.UE4.IO.OnDemand;

public enum EOnDemandTocMajorVersion : ushort
{
    Invalid = 0,
    One = 1,

    LatestPlusOne,
    Latest = (LatestPlusOne - 1)
}

public enum EOnDemandTocMinorVersion : ushort
{
    Invalid = 0,
    MemoryMapped = 1,

    LatestPlusOne,
    Latest = (LatestPlusOne - 1)
}

using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.IO.OnDemand;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct FOnDemandTocVersion()
{
    public readonly ushort Major = (ushort)EOnDemandTocMajorVersion.Latest;
    public readonly ushort Minor = (ushort)EOnDemandTocMinorVersion.Latest;

    public bool IsValid() => Major > 0 && Major != ushort.MaxValue && Minor > 0 && Minor != ushort.MaxValue;
}

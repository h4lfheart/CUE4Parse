using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Layouts;

public class FBlock
{
    public TIntVector2<ushort> Min;
    public TIntVector2<ushort> Size;
    public ulong Id;
    public int Priority;
    public bool bReduceBothAxes;
    public bool bReduceByTwo;
    
    public FBlock(FArchive Ar)
    {
        Min = Ar.Read<TIntVector2<ushort>>();
        Size = Ar.Read<TIntVector2<ushort>>();

        Id = Ar.Read<ulong>();
        Priority = Ar.Read<int>();
        bReduceBothAxes = Ar.Read<byte>() == 1;
        bReduceByTwo = Ar.Read<byte>() == 1;
    }
}
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Layouts;

public class Layout
{
    public int Version;
    public TIntVector2<ushort> Size;
    public FBlock[] Blocks;
    public TIntVector2<ushort> MaxSize;
    public EPackStrategy Strategy;
    public EReductionMethod ReductionMethod;
    public bool FirstLODToIgnoreWarnings;

    public bool IsBroken;
    
    public Layout(FArchive Ar)
    {
        Version = Ar.Read<int>();
        if (Version == -1)
        {
            IsBroken = true;
            return;
        }
        
        Size = Ar.Read<TIntVector2<ushort>>();
        Blocks = Ar.ReadArray(() => new FBlock(Ar));
        MaxSize = Ar.Read<TIntVector2<ushort>>();

        Strategy = Ar.Read<EPackStrategy>();
        FirstLODToIgnoreWarnings = Ar.ReadBoolean();
        ReductionMethod = Ar.Read<EReductionMethod>();
    }
}

public enum EPackStrategy : uint
{
    RESIZABLE_LAYOUT,
    FIXED_LAYOUT,
    OVERLAY_LAYOUT
}

public enum EReductionMethod : uint
{
    HALVE_REDUCTION,	// Divide axis by 2
    UNITARY_REDUCTION	// Reduces 1 block the axis 
}
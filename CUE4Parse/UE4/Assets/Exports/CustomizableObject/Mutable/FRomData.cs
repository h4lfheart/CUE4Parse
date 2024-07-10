using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable;

public class FRomData
{
    public uint Id;
    public uint Size;
    public uint ResourceIndex;
    public DATATYPE ResourceType; 
    public ERomFlags Flags; // ERomFlags (couldn't find in github repo)
    
    
    public FRomData(FArchive Ar)
    {
        Id = Ar.Read<uint>();
        Size = Ar.Read<uint>();
        ResourceIndex = Ar.Read<uint>();
        ResourceType = Ar.Read<DATATYPE>();
        Flags = Ar.Read<ERomFlags>();
    }
}

public enum ERomFlags : ushort
{
    /** Standard data. */
    None = 0,

    /** Bigger mips and mesh lods that are optional in some devices of a platform. */
    HighRes = 1 << 0,
};

public enum DATATYPE : ushort
{
    DT_NONE,
    DT_BOOL,
    DT_INT,
    DT_SCALAR,
    DT_COLOUR,
    DT_IMAGE,
    DT_VOLUME_DEPRECATED,
    DT_LAYOUT,
    DT_MESH,
    DT_INSTANCE,
    DT_PROJECTOR,
    DT_STRING,
    DT_EXTENSION_DATA,

    // Supporting data types : Never returned as an actual data type for any operation.
    DT_MATRIX,
    DT_SHAPE,
    DT_CURVE,
    DT_SKELETON,
    DT_PHYSICS_ASSET,
		
    DT_COUNT
}
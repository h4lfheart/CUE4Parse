using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Meshes;

public class FMeshBufferChannel
{
    public EMeshBufferSemantic Semantic;
    public EMeshBufferFormat Format;
    public int SemanticIndex;
    public ushort Offset;
    public ushort ComponentCount;
    
    public FMeshBufferChannel(FArchive Ar)
    {
        Semantic = Ar.Read<EMeshBufferSemantic>();
        Format = Ar.Read<EMeshBufferFormat>();
        SemanticIndex = Ar.Read<int>();
        Offset = Ar.Read<ushort>();
        ComponentCount = Ar.Read<ushort>();
    }
}

public enum EMeshBufferSemantic : uint
{

    MBS_NONE,

    //! For index buffers, and mesh morphs
    MBS_VERTEXINDEX,

    //! Standard vertex semantics
    MBS_POSITION,
    MBS_NORMAL,
    MBS_TANGENT,
    MBS_BINORMAL,
    MBS_TEXCOORDS,
    MBS_COLOUR,
    MBS_BONEWEIGHTS,
    MBS_BONEINDICES,

    //! Internal semantic indicating what layout block each vertex belongs to.
    //! It can be safely ignored if present in meshes returned by the system.
    //! It will never be in the same buffer that other vertex semantics.
    MBS_LAYOUTBLOCK,

    MBS_CHART_DEPRECATED,

    //! To let users define channels with semantics unknown to the system.
    //! These channels will never be transformed, and the per-vertex or per-index data will be
    //! simply copied.
    MBS_OTHER,

    //! Sign to define the orientation of the tangent space.
    MBS_TANGENTSIGN_DEPRECATED,

    //! Semantics usefule for mesh binding.
    MBS_TRIANGLEINDEX,
    MBS_BARYCENTRICCOORDS,
    MBS_DISTANCE,

    //! Semantics useful for alternative skin weight profiles.
    MBS_ALTSKINWEIGHT,

    //! Utility
    MBS_COUNT,

    _MBS_FORCE32BITS = 0xFFFFFFFF
}

public enum EMeshBufferFormat : uint
{

    MBF_NONE,
    MBF_FLOAT16,
    MBF_FLOAT32,

    MBF_UINT8,
    MBF_UINT16,
    MBF_UINT32,
    MBF_INT8,
    MBF_INT16,
    MBF_INT32,

    //! Integers interpreted as being in the range 0.0f to 1.0f
    MBF_NUINT8,
    MBF_NUINT16,
    MBF_NUINT32,

    //! Integers interpreted as being in the range -1.0f to 1.0f
    MBF_NINT8,
    MBF_NINT16,
    MBF_NINT32,

    //! Packed 1 to -1 value using multiply+add (128 is almost zero). Use 8-bit unsigned ints.
    MBF_PACKEDDIR8,

    //! Same as MBF_PACKEDDIR8, with the w component replaced with the sign of the determinant
    //! of the vertex basis to define the orientation of the tangent space in UE4 format.
    //! Use 8-bit unsigned ints.
    MBF_PACKEDDIR8_W_TANGENTSIGN,

    //! Packed 1 to -1 value using multiply+add (128 is almost zero). Use 8-bit signed ints.
    MBF_PACKEDDIRS8,

    //! Same as MBF_PACKEDDIRS8, with the w component replaced with the sign of the determinant
    //! of the vertex basis to define the orientation of the tangent space in UE4 format.
    //! Use 8-bit signed ints.
    MBF_PACKEDDIRS8_W_TANGENTSIGN,

    MBF_FLOAT64,
    MBF_UINT64,
    MBF_INT64,
    MBF_NUINT64,
    MBF_NINT64,

    MBF_COUNT,

    _MBF_FORCE32BITS = 0xFFFFFFFF

}
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Images;

public class FImageLODRange
{
    public int FirstIndex = 0;
    public ushort ImageSizeX = 0;
    public ushort ImageSizeY = 0;
    //public ushort _Padding = 0;
    public byte LODCount = 0;
    public EImageFormat ImageFormat = EImageFormat.IF_NONE;
    
    public FImageLODRange(FArchive Ar)
    {
        FirstIndex = Ar.Read<int>();
        ImageSizeX = Ar.Read<ushort>();
        ImageSizeY = Ar.Read<ushort>();
        Ar.Position += sizeof(ushort);
        LODCount = Ar.Read<byte>();
        ImageFormat = Ar.Read<EImageFormat>();
    }
}

public enum EImageFormat : byte
{
    IF_NONE,
    IF_RGB_UBYTE,
    IF_RGBA_UBYTE,
    IF_L_UBYTE,

    //! Deprecated formats
    IF_PVRTC2_DEPRECATED,
    IF_PVRTC4_DEPRECATED,
    IF_ETC1_DEPRECATED,
    IF_ETC2_DEPRECATED,

    IF_L_UBYTE_RLE,
    IF_RGB_UBYTE_RLE,
    IF_RGBA_UBYTE_RLE,
    IF_L_UBIT_RLE,

    //! Common S3TC formats
    IF_BC1,
    IF_BC2,
    IF_BC3,
    IF_BC4,
    IF_BC5,

    //! Not really supported yet
    IF_BC6,
    IF_BC7,

    //! Swizzled versions, engineers be damned.
    IF_BGRA_UBYTE,

    //! The new standard
    IF_ASTC_4x4_RGB_LDR,
    IF_ASTC_4x4_RGBA_LDR,
    IF_ASTC_4x4_RG_LDR,

    IF_ASTC_8x8_RGB_LDR,
    IF_ASTC_8x8_RGBA_LDR,
    IF_ASTC_8x8_RG_LDR,
    IF_ASTC_12x12_RGB_LDR,
    IF_ASTC_12x12_RGBA_LDR,
    IF_ASTC_12x12_RG_LDR,
    IF_ASTC_6x6_RGB_LDR,
    IF_ASTC_6x6_RGBA_LDR,
    IF_ASTC_6x6_RG_LDR,
    IF_ASTC_10x10_RGB_LDR,
    IF_ASTC_10x10_RGBA_LDR,
    IF_ASTC_10x10_RG_LDR,

    IF_COUNT
};
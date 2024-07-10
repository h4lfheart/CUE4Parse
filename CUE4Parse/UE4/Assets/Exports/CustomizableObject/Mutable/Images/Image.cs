using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Images;

public class Image : IMutablePtr
{
    public byte Flags;
    public FImageDataStorage DataStorage;

    public int Version { get; set; }
    
    public void Deserialize(FArchive Ar)
    {
        if (Version > 4)
        {
            throw new ParserException($"Mutable image version {Version} is not supported.");
        }
        
        if (Version <= 3)
        {
            DataStorage = new FImageDataStorage
            {
                ImageSize = Ar.Read<TIntVector2<ushort>>(),
                NumLODs = Ar.Read<byte>(),
                ImageFormat = Ar.Read<EImageFormat>(),
                Buffers = [Ar.ReadArray<byte>()]
            };
        }
        else
        {
            DataStorage = new FImageDataStorage(Ar);
        }

        Flags = Ar.Read<byte>();
    }
}
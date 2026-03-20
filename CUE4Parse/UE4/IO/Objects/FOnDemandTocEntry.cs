using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.Objects;

public class FOnDemandTocEntry : IOnDemandTocEntry
{
    public FSHAHash Hash { get; }
    public FIoChunkId ChunkId { get; }
    public readonly ulong RawSize;
    public readonly ulong EncodedSize;
    public readonly uint BlockOffset;
    public readonly uint BlockCount;

    public FOnDemandTocEntry(FArchive Ar)
    {
        Hash = new FSHAHash(Ar);
        ChunkId = Ar.Read<FIoChunkId>();
        RawSize = Ar.Read<ulong>();
        EncodedSize = Ar.Read<ulong>();
        BlockOffset = Ar.Read<uint>();
        BlockCount = Ar.Read<uint>();
    }
}

using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.OnDemand;

public struct FOnDemandContainerEntry : IOnDemandContainerEntry
{
    public readonly FGuid EncryptionKeyGuid;
    public readonly FIoContainerId ContainerId;
    public string Name { get; }
    public readonly uint ContainerHeaderSize;
    public readonly uint DataOffset;
    public readonly uint DataSize;
    public readonly uint ChunkCount;
    public readonly uint BlockCount;
    public readonly uint BlockSize;
    public readonly uint TagSetCount;
    public readonly uint TagSetIndicesCount;
    public FSHAHash Hash { get; }
    public readonly uint ContainerFlags;
    public readonly uint FileContainerFlags;
    public readonly byte[] Pad;

    public static uint Size =>
        16 + // FGuid
        8 + // FIoContainerId
        sizeof(uint) * 2 + // FOnDemandStringEntry (Offset, Len)
        sizeof(uint) * 10 + // 10 uint fields
        20 + // FSHAHash
        36; // Pad

    public IOnDemandTocEntry[] Entries { get; set; }

    public FOnDemandContainerEntry(FArchive Ar)
    {
        EncryptionKeyGuid = Ar.Read<FGuid>();
        ContainerId = Ar.Read<FIoContainerId>();
        Name = new FOnDemandStringEntry(Ar).ToString();
        ContainerHeaderSize = Ar.Read<uint>();
        DataOffset = Ar.Read<uint>();
        DataSize = Ar.Read<uint>();
        ChunkCount = Ar.Read<uint>();
        BlockCount = Ar.Read<uint>();
        BlockSize = Ar.Read<uint>();
        TagSetCount = Ar.Read<uint>();
        TagSetIndicesCount = Ar.Read<uint>();
        Hash = new FSHAHash(Ar);
        ContainerFlags = Ar.Read<uint>();
        FileContainerFlags = Ar.Read<uint>();
        Pad = Ar.ReadBytes(36);
    }

    public override string ToString() => $"{Name}: {ChunkCount} Chunks, {DataOffset} DataOffset, {DataSize} Size";
}

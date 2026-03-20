using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.OnDemand;

public readonly struct FOnDemandTocHeader2(FArchive Ar) : IOnDemandTocHeader
{
    public readonly FOnDemandTocSignature Signature = new(Ar);
    public readonly FOnDemandTocVersion Version = Ar.Read<FOnDemandTocVersion>();
    public readonly uint Pad = Ar.Read<uint>();
    public readonly long EpochTimestamp = Ar.Read<long>();
    public readonly FOnDemandStringEntry BuildVersion = new(Ar);
    public readonly FOnDemandStringEntry TargetPlatform = new(Ar);
    public string ChunksDirectory { get; } = new FOnDemandStringEntry(Ar).ToString();
    public readonly FOnDemandStringEntry HostGroupName = new(Ar);
    public readonly FOnDemandStringEntry CompressionFormat = new(Ar);
    public readonly uint StringTableLen = Ar.Read<uint>();
    public readonly uint ContainerCount = Ar.Read<uint>();
    public readonly byte[] Pad2 = Ar.ReadBytes(48);

    public static uint Size =>
        FOnDemandTocSignature.Size +
        sizeof(ushort) * 2 + // Version (Major, Minor)
        sizeof(uint) + // Pad
        sizeof(long) + // EpochTimestamp
        sizeof(uint) * 2 * 5 + // 5x FOnDemandStringEntry (Offset, Len)
        sizeof(uint) + // StringTableLen
        sizeof(uint) + // ContainerCount
        48; // Pad2
}

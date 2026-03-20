using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.IO.OnDemand;

public readonly struct FOnDemandTocEntryLight : IOnDemandTocEntry
{
    public FIoChunkId ChunkId { get; init; }
    public FSHAHash Hash { get; init; }
}

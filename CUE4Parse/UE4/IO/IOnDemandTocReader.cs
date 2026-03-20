using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.IO;

public interface IOnDemandTocReader
{
    public IOnDemandTocHeader Header { get; }
    public IOnDemandContainerEntry[] Containers { get; }
}

public interface IOnDemandTocHeader
{
    public string ChunksDirectory { get; }
}

public interface IOnDemandContainerEntry
{
    public string Name { get; }
    public FSHAHash Hash { get; }
    public IOnDemandTocEntry[] Entries { get; }
}

public interface IOnDemandTocEntry
{
    public FIoChunkId ChunkId { get; }
    public FSHAHash Hash { get; }
}

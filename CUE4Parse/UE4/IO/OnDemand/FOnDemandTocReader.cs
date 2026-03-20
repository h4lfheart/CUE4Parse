using System.IO;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.OnDemand;

public class FOnDemandTocReader : IOnDemandTocReader
{
    public IOnDemandTocHeader Header { get; }
    public IOnDemandContainerEntry[] Containers { get; }
    public readonly FOnDemandTocFooter Footer;

    public FOnDemandTocReader(string file) : this(new FileInfo(file)) { }
    public FOnDemandTocReader(FileInfo file) : this(new FByteArchive(file.FullName, File.ReadAllBytes(file.FullName))) { }
    public FOnDemandTocReader(FArchive Ar)
    {
        var header = new FOnDemandTocHeader2(Ar);
        Header = header;

        if (!header.Signature.IsValid())
            throw new ParserException($"Invalid TOC signature found in {Ar.Name}");
        if (!header.Version.IsValid())
            throw new ParserException($"Invalid TOC version found in {Ar.Name}");

        var totalHeaderSize = FOnDemandTocHeader2.Size + header.StringTableLen + FOnDemandContainerEntry.Size * header.ContainerCount;
        if (totalHeaderSize >= Ar.Length)
            throw new ParserException($"TOC header size greater than file size in {Ar.Name}");

        Ar.Seek(FOnDemandTocHeader2.Size + header.StringTableLen, SeekOrigin.Begin);
        Containers = new IOnDemandContainerEntry[header.ContainerCount];
        for (var i = 0; i < Containers.Length; i++)
        {
            var container = new FOnDemandContainerEntry(Ar);

            var containerDataOffset = totalHeaderSize + container.DataOffset;
            var containerDataSize = container.DataSize - container.ContainerHeaderSize;
            if (containerDataSize <= 0)
            {
                throw new ParserException($"Unexpected container data size in {Ar.Name}");
            }

            var buffer = new FByteArchive(container.Name, Ar.ReadBytesAt(containerDataOffset, (int) containerDataSize));

            var chunkIds = new FIoChunkId[container.ChunkCount];
            for (var j = 0; j < chunkIds.Length; j++)
            {
                var chunkId = buffer.Read<FIoChunkId>();
                if (chunkId.ChunkId == 18066810002824497619)
                {

                }

                chunkIds[j] = chunkId;
            }

            var chunkHashes = new FSHAHash[container.ChunkCount];
            for (var j = 0; j < chunkHashes.Length; j++)
            {
                chunkHashes[j] = new FSHAHash(buffer);
                buffer.Position += 16;
            }

            var entries = new IOnDemandTocEntry[container.ChunkCount];
            for (var j = 0; j < entries.Length; j++)
            {
                entries[j] = new FOnDemandTocEntryLight { ChunkId = chunkIds[j], Hash = chunkHashes[j] };
            }

            container.Entries = entries;
            Containers[i] = container;
        }

        Ar.Seek(-FOnDemandTocFooter.Size, SeekOrigin.End);
        Footer = new FOnDemandTocFooter(Ar);

        if (!Footer.Signature.IsValid())
            throw new ParserException($"Invalid TOC footer signature found in {Ar.Name}");
    }
}

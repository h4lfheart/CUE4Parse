using System;
using CUE4Parse.UE4.Writers;
using Ionic.Zlib;
using ZstdSharp;

namespace CUE4Parse_Conversion.UnrealFormat;

public abstract class UnrealFormatExport
{
    protected string Identifier = string.Empty;
    protected readonly FArchiveWriter Ar = new();
    protected FUnrealFormatHeader Header;
    protected ExporterOptions Options;
    
    protected UnrealFormatExport(string name, ExporterOptions options)
    {
        Options = options;
        Header = new FUnrealFormatHeader(Identifier, name, Options.CompressionFormat);
    }

    private const int ZSTD_LEVEL = 6;
    
    public void Save(FArchiveWriter archive)
    {
        var data = Ar.GetBuffer();
        Header.UncompressedSize = data.Length;
        
        var compressedData = Header.CompressionFormat switch
        {
            EFileCompressionFormat.GZIP => GZipStream.CompressBuffer(data),
            EFileCompressionFormat.ZSTD => new Compressor(ZSTD_LEVEL).Wrap(data),
            _ => data
        };
        Header.CompressedSize = compressedData.Length;
        
        Header.Serialize(archive);
        archive.Write(compressedData);
    }
    
    public Span<byte> ToArray()
    {
        using var archive = new FArchiveWriter();
        Save(archive);
        return archive.GetBuffer();
    }
}
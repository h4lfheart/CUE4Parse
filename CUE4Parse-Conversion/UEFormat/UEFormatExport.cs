using System;
using CUE4Parse.UE4.Writers;
using Ionic.Zlib;
using ZstdSharp;

namespace CUE4Parse_Conversion.UEFormat;

public abstract class UEFormatExport
{
    protected virtual string Identifier { get; set; }
    protected readonly FArchiveWriter Ar = new();
    private string ObjectName;
    private ExporterOptions Options;
    
    private const int ZSTD_LEVEL = 6;
    
    protected UEFormatExport(string name, ExporterOptions options)
    {
        ObjectName = name;
        Options = options;
    }

    public void Save(FArchiveWriter archive)
    {
        var header = new FUEFormatHeader(Identifier, ObjectName, Options.CompressionFormat);
        var data = Ar.GetBuffer();
        header.UncompressedSize = data.Length;
        
        var compressedData = header.CompressionFormat switch
        {
            EFileCompressionFormat.GZIP => GZipStream.CompressBuffer(data),
            EFileCompressionFormat.ZSTD => new Compressor(ZSTD_LEVEL).Wrap(data),
            _ => data
        };
        header.CompressedSize = compressedData.Length;
        
        header.Serialize(archive);
        archive.Write(compressedData);
    }
    
    public Span<byte> ToArray()
    {
        using var archive = new FArchiveWriter();
        Save(archive);
        return archive.GetBuffer();
    }
}
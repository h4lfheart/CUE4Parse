using System;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;
using Serilog;
using FImageArray = byte[];

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable;

public class FImageDataStorage
{
    public int Version;
    public TIntVector2<ushort> ImageSize;
    public byte NumLODs;
    public EImageFormat ImageFormat;
    [JsonIgnore] public byte[][] Buffers;
    [JsonIgnore] public ushort[] CompactedTailOffsets;
    
    private const int NumLODsInCompactedTail = 7;
    
    public FImageDataStorage(FArchive Ar)
    {
        Version = Ar.Read<int>();
        ImageSize = Ar.Read<TIntVector2<ushort>>();
        ImageFormat = Ar.Read<EImageFormat>();
        NumLODs = Ar.Read<byte>();

        Ar.Position += 3;

        var buffersNum = Ar.Read<int>();

        Buffers = new FImageArray[buffersNum];
        
        for (var i = 0; i < buffersNum; i++)
        {
            Buffers[i] = Ar.ReadArray<byte>();
        }

        var numTailOffsets = Ar.Read<int>();
        if (numTailOffsets != NumLODsInCompactedTail)
        {
            throw new ParserException("numTailOffsets != NumLODsInCompactedTail)");
        }

        CompactedTailOffsets = Ar.ReadArray<ushort>(numTailOffsets);
    }
}
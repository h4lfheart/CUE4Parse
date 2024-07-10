using System;
using System.Diagnostics;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable;

public class Image : IMutablePtr
{
    public byte Flags;
    
    // Version < 3
    public TIntVector2<ushort> Size;
    public byte LODs;
    public byte Format;
    public byte[] OldData;
    
    // Version >= 4
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
            Size = Ar.Read<TIntVector2<ushort>>();
            LODs = Ar.Read<byte>();
            Format = Ar.Read<byte>();
            OldData = Ar.ReadArray<byte>();
        }
        else
        {
            DataStorage = new FImageDataStorage(Ar);
        }

        Flags = Ar.Read<byte>();
    }
}
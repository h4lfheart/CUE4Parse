using System.Collections.Generic;
using CUE4Parse.UE4.Writers;

namespace CUE4Parse_Conversion.UnrealFormat;

public static class UnrealFormatWriterExtensions
{
    public static void WriteFString(this FArchiveWriter Ar, string str)
    {
        new FString(str).Serialize(Ar);
    }

    public static void WriteList<T>(this FArchiveWriter Ar, List<T> list) where T : ISerializable
    {
        Ar.Write(list.Count);
        list.ForEach(it => it.Serialize(Ar));
    }
}
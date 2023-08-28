using System;
using System.Collections.Generic;
using System.Linq;
using CUE4Parse.UE4.Writers;

namespace CUE4Parse_Conversion.UnrealFormat;

public static class UnrealFormatExtensions
{
    public static void WriteFString(this FArchiveWriter Ar, string str)
    {
        new FString(str).Serialize(Ar);
    }

    public static void WriteArray<T>(this FArchiveWriter Ar, IEnumerable<T> enumerable) where T : ISerializable
    {
        var array = enumerable.ToArray();
        Ar.WriteArray(array, it => it.Serialize(Ar));
    }
    
    public static void WriteArray<T>(this FArchiveWriter Ar, IEnumerable<T> enumerable, Action<T> action)
    {
        var array = enumerable.ToArray();
        Ar.WriteArray(array, action);
    }
    
    public static void WriteArray<T>(this FArchiveWriter Ar, T[] array, Action<T> action)
    {
        Ar.Write(array.Length);
        foreach (var item in array)
        {
            action(item);
        }
    }
}
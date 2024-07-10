using System.Collections.Generic;
using CUE4Parse.MappingsProvider.Usmap;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable;

// uint == OP::ADDRESS
public class FState
{
    public string Name;
    public uint Root = 0;
    public int[] RuntimeParameters;
    public uint[] UpdateCache;
    public KeyValuePair<uint, ulong>[] DynamicResources;
    
    public FState(FArchive Ar)
    {
        Name = UCustomizableObject.ReadMutableFString(Ar);
        Root = Ar.Read<uint>();
        RuntimeParameters = Ar.ReadArray<int>();
        UpdateCache = Ar.ReadArray<uint>();
        DynamicResources = Ar.ReadArray(() => new KeyValuePair<uint, ulong>(Ar.Read<uint>(), Ar.Read<ulong>()));

    }
}
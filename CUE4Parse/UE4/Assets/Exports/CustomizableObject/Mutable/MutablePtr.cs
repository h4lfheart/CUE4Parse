using System.Linq;
using CUE4Parse.UE4.Readers;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable;

// my custom class for handling the ptr based mutable classes, lmk if you got issues with it >:(
public class MutablePtr<T> where T : IMutablePtr, new()
{
    public int Index;
    public bool IsBadPointer;

    public T Object;
    
    private int Version;

    public MutablePtr(FArchive Ar)
    {
        Version = Ar.Read<int>();
        if (Version == -1)
        {
            IsBadPointer = true;
            return;
        }

        Object = new T { Version = Version };
        Object.Deserialize(Ar);
    }
    
    public void Deconstruct(out int index, out T obj)
    {
        index = Index;
        obj = Object;
    }

    public static MutablePtr<T>[] ReadArray(FArchive Ar)
    {
        var count = Ar.Read<int>();
        var array = new MutablePtr<T>[count];
        var nextObjectIndex = 0;
        for (var objectIndex = 0; objectIndex < count; objectIndex++)
        {
            var index = Ar.Read<int>();
            if (index == -1)
            {
                objectIndex--;
                continue;
            }

            var ptr = new MutablePtr<T>(Ar) { Index = index };
            if (ptr.IsBadPointer) continue;

            array[nextObjectIndex] = ptr;
            nextObjectIndex++;
        }

        return array.Where(item => item is not null).ToArray();
    }
}

public interface IMutablePtr
{
    public int Version { get; set; }
    public void Deserialize(FArchive Ar);
}

public static class MutablePtrExtensions 
{
    public static T ReadMutable<T>(this FArchive Ar) where T : IMutablePtr, new()
    {
        var obj = new MutablePtr<T>(Ar);
        return obj.Object;
    }
}
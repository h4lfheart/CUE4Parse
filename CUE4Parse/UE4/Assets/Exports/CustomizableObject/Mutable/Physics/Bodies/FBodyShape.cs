using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Physics.Bodies;

public class FBodyShape
{
    public uint Version;
    public string Name;
    public uint Flags;
    
    public FBodyShape(FArchive Ar)
    {
        Version = Ar.Read<uint>();
        Name = UCustomizableObject.ReadMutableFString(Ar);
        Flags = Ar.Read<uint>();
    }
}
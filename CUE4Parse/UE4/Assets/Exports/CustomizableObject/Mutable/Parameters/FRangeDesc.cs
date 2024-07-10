using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Parameters;

public class FRangeDesc
{
    public uint Version;
    
    public string Name;
    public string Uid;
    public int DimensionParameter;
    
    public FRangeDesc(FArchive Ar)
    {
        Version = Ar.Read<uint>();

        Name = UCustomizableObject.ReadMutableFString(Ar);
        Uid = UCustomizableObject.ReadMutableFString(Ar);
        DimensionParameter = Ar.Read<int>();
    }
}
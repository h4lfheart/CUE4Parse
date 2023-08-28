using CUE4Parse.UE4.Writers;

namespace CUE4Parse_Conversion.UnrealFormat;

public interface ISerializable
{
    public void Serialize(FArchiveWriter Ar);
}
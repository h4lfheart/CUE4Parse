using CUE4Parse_Conversion.UnrealFormat;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Writers;

namespace CUE4Parse_Conversion.Animations.UnrealFormat;

public class FBoneKey : ISerializable
{
    public FVector Location;
    public FQuat Rotation;
    public FVector Scale;

    public FBoneKey(FTransform transform)
    {
        Location = transform.Translation;
        Rotation = transform.Rotation;
        Scale = transform.Scale3D;
    }
    
    public void Serialize(FArchiveWriter Ar)
    {
        Location.Serialize(Ar);
        Rotation.Serialize(Ar);
        Scale.Serialize(Ar);
    }
}
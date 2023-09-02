using CUE4Parse_Conversion.UEFormat;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Writers;

namespace CUE4Parse_Conversion.Animations.UEFormat;

public class FBoneKey : ISerializable
{
    private int Frame;
    public FVector Location;
    public FQuat Rotation;
    public FVector Scale;
    public FTransform Transform => new(Rotation, Location, Scale);

    public FBoneKey(int frame, FTransform transform)
    {
        Frame = frame;
        Location = transform.Translation;
        Rotation = transform.Rotation;
        Scale = transform.Scale3D;
    }
    
    public void Serialize(FArchiveWriter Ar)
    {
        Ar.Write(Frame);
        Location.Serialize(Ar);
        Rotation.Serialize(Ar);
        Scale.Serialize(Ar);
    }
}
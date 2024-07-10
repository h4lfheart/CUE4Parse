using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Math;

public class FProjector
{
    public PROJECTOR_TYPE Type;
    public FVector Position;
    public FVector Direction;
    public FVector Up;
    public FVector Scale;
    public float ProjectionAngle;
    
    public FProjector(FArchive Ar)
    {
        Type = Ar.Read<PROJECTOR_TYPE>();
        Position = Ar.Read<FVector>();
        Direction = Ar.Read<FVector>();
        Up = Ar.Read<FVector>();
        Scale = Ar.Read<FVector>();
        ProjectionAngle = Ar.Read<float>();
    }
}

public enum PROJECTOR_TYPE : uint
{
    //!
    PLANAR,

    //!
    CYLINDRICAL,

    //!
    WRAPPING,

    //! Utility enumeration value, not really a projector type.
    COUNT
};
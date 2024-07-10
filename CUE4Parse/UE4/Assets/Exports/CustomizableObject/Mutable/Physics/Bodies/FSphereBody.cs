using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Physics.Bodies;

public class FSphereBody : FBodyShape
{
    public uint Version;
    public FVector Position;
    public float Radius;
    
    public FSphereBody(FArchive Ar) : base(Ar)
    {
        Version = Ar.Read<uint>();
        Position = Ar.Read<FVector>();
        Radius = Ar.Read<float>();
    }
}
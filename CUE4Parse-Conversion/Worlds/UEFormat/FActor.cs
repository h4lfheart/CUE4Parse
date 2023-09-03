using CUE4Parse_Conversion.UEFormat;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Writers;

namespace CUE4Parse_Conversion.Worlds.UEFormat;

public class FActor : ISerializable
{
    public int MeshHash;
    public string Name;
    public FVector Position;
    public FRotator Rotation;
    public FVector Scale;

    public FActor(int meshHash, string name, FVector position, FRotator rotation, FVector scale)
    {
        MeshHash = meshHash;
        Name = name;
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public void Serialize(FArchiveWriter Ar)
    {
        Ar.Write(MeshHash);
        Ar.WriteFString(Name);
        Position.Serialize(Ar);
        Rotation.Serialize(Ar);
        Scale.Serialize(Ar);
    }
}
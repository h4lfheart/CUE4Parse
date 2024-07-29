using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.Component;

public class USceneComponent : UObject
{
    public FVector RelativeLocation;
    public FRotator RelativeRotation;
    public FVector RelativeScale3D;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        RelativeLocation = GetOrDefault(nameof(RelativeLocation), FVector.ZeroVector);
        RelativeRotation = GetOrDefault(nameof(RelativeRotation), FRotator.ZeroRotator);
        RelativeScale3D = GetOrDefault(nameof(RelativeScale3D), FVector.ZeroVector);
    }
}
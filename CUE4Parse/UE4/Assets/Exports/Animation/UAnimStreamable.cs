using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class UAnimStreamable : UAnimSequenceBase
{
    [UProperty] public int NumFrames;
    [UProperty] public FName RetargetSource;
    [UProperty] public UAnimCurveCompressionSettings CurveCompressionSettings;
    [UProperty] public FStructFallback RawCurveData;
}
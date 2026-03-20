using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.OnDemand;

public readonly struct FOnDemandTocFooter(FArchive Ar)
{
    public readonly FOnDemandTocSignature Signature = new(Ar);

    public static uint Size => FOnDemandTocSignature.Size;
}

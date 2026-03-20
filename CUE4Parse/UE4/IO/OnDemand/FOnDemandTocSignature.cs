using System;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.OnDemand;

public readonly struct FOnDemandTocSignature(FArchive Ar)
{
    public readonly byte[] Signature = Ar.ReadBytes(16);

    private readonly byte[] _expected = "UE ON-DEMAND TOC"u8.ToArray();
    public bool IsValid() => Signature.Length == _expected.Length && Signature.AsSpan().SequenceEqual(_expected);

    public const uint Size = 16;
}

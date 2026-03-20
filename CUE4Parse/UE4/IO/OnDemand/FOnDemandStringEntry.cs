using System.Text;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.OnDemand;

public readonly struct FOnDemandStringEntry
{
    public readonly string Text;

    public FOnDemandStringEntry(FArchive Ar)
    {
        var offset = Ar.Read<uint>();
        var length = Ar.Read<uint>();
        var bytes = Ar.ReadBytesAt(FOnDemandTocHeader2.Size + offset, (int) length);

        Text = Encoding.UTF8.GetString(bytes);
    }

    public override string ToString() => Text;
}

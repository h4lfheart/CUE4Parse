using System.Runtime.InteropServices;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Meshes;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FSurfaceSubMesh
{
    public int VertexBegin;
    public int VertexEnd;
    public int IndexBegin;
    public int IndexEnd;
    public uint ExternalId;
}
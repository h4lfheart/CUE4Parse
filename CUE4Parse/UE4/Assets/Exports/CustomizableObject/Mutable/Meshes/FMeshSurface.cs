using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Meshes;

public class FMeshSurface
{
    public FSurfaceSubMesh[] SubMeshes;
    public uint BoneMapIndex;
    public uint BoneMapCount;
    public uint Id;

    // for organization, mutable devs suck at versioning
    private const int VER_ADD_SURFACE_SUBMESH = 23;
    private const int VER_REMOVE_SURFACE_VERSION = 22;
    
    public FMeshSurface(FArchive Ar, int meshVersion)
    {
        if (meshVersion >= VER_ADD_SURFACE_SUBMESH)
        {
            SubMeshes = Ar.ReadArray<FSurfaceSubMesh>();
            BoneMapIndex = Ar.Read<uint>();
            BoneMapCount = Ar.Read<uint>();
            Id = Ar.Read<uint>();
        }
        else
        {
            var version = meshVersion < VER_REMOVE_SURFACE_VERSION ? Ar.Read<int>() : 1;
            
            var firstVertex = Ar.Read<int>();
            var vertexCount = Ar.Read<int>();
            var firstIndex = Ar.Read<int>();
            var indexCount = Ar.Read<int>();

            SubMeshes =
            [
                new FSurfaceSubMesh
                {
                    VertexBegin = firstVertex,
                    VertexEnd = firstVertex + vertexCount,
                    IndexBegin = firstIndex,
                    IndexEnd = firstIndex + indexCount
                }
            ];
            
            
            BoneMapIndex = Ar.Read<uint>();
            BoneMapCount = Ar.Read<uint>();

            if (version >= 1)
            {
                var bCastShadow = Ar.Read<byte>() == 1;
            }
            
            Id = Ar.Read<uint>();
        }
    }
}
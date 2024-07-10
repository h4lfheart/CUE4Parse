using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Layouts;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Physics;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Skeletons;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Meshes;

public class Mesh : IMutablePtr
{
    public FMeshBufferSet IndexBuffers;
    public FMeshBufferSet VertexBuffers;
    public KeyValuePair<EMeshBufferType, FMeshBufferSet>[] AdditionalBuffers;
    public Layout[] Layouts;
    public uint[] SkeletonIDs;
    public Skeleton Skeleton;
    public PhysicsBody PhysicsBody;
    public EMeshFlags Flags;
    public FMeshSurface[] Surfaces;
    public string[] Tags;
    public ulong[] StreamedResources;
    public FBonePose[] BonePoses;
    public FBoneName[] BoneMap;
    public PhysicsBody[] AdditionalPhysicsBodies;
    public uint MeshIDPrefix;
    public uint ReferenceID;

    public int Version { get; set; }

    public void Deserialize(FArchive Ar)
    {
        if (Version > 23)
        {
            throw new ParserException($"Mutable mesh version {Version} is not supported.");
        }

        IndexBuffers = new FMeshBufferSet(Ar);
        VertexBuffers = new FMeshBufferSet(Ar);
        AdditionalBuffers = Ar.ReadArray(() => new KeyValuePair<EMeshBufferType, FMeshBufferSet>(Ar.Read<EMeshBufferType>(), new FMeshBufferSet(Ar)));
        Layouts = Ar.ReadArray(Ar.ReadMutableObject<Layout>);

        SkeletonIDs = Ar.ReadArray<uint>();

        Skeleton = Ar.ReadMutableObject<Skeleton>();
        PhysicsBody = Ar.ReadMutableObject<PhysicsBody>();

        Flags = Ar.Read<EMeshFlags>();
        
        Surfaces = Ar.ReadArray(() => new FMeshSurface(Ar, Version));
        
        Tags = Ar.ReadArray(() => UCustomizableObject.ReadMutableFString(Ar));
        StreamedResources = Ar.ReadArray<ulong>();

        BonePoses = Ar.ReadArray(() => new FBonePose(Ar, Version));
        BoneMap = Ar.ReadArray(() => new FBoneName(Ar));

        AdditionalPhysicsBodies = Ar.ReadArray(Ar.ReadMutableObject<PhysicsBody>);

        MeshIDPrefix = Ar.Read<uint>();
        
        if (Version >= 23)
            ReferenceID = Ar.Read<uint>();
    }
}

public enum EMeshBufferType
{
    None,
    SkeletonDeformBinding,
    PhysicsBodyDeformBinding,
    PhysicsBodyDeformSelection,
    PhysicsBodyDeformOffsets,
    MeshLaplacianData,
    MeshLaplacianOffsets,
    UniqueVertexMap
};

[Flags]
public enum EMeshFlags : uint
{
    None = 0,

    /** The mesh is formatted to be used for planar and cilyndrical projection */
    ProjectFormat = 1 << 0,

    /** The mesh is formatted to be used for wrapping projection */
    ProjectWrappingFormat = 1 << 1,

    /** The mesh is a reference to an external resource mesh. */
    IsResourceReference = 1 << 2,

    /** The mesh is a reference to an external resource mesh and must be loaded when first referenced. */
    IsResourceForceLoad = 1 << 3,

};
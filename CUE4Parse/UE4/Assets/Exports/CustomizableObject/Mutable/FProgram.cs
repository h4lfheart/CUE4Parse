using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection.Metadata;
using CUE4Parse.MappingsProvider.Usmap;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Layouts;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Meshes;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Parameters;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Physics;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Skeletons;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine.Curves;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable;

public class FProgram
{
    public uint[] OpAddress;
    public byte[] ByteCode;
    public FState[] States;
    public FRomData[] Roms;
    public Dictionary<int, Image> ConstantImageLODs = new();
    public uint[] ConstantImageLODIndices;
    public FImageLODRange[] ConstantImages;
    public Dictionary<int, Mesh> ConstantMeshes = new();
    public ExtensionData[] ConstantExtensionData;
    public string[] ConstantStrings;
    public Dictionary<int, Layout> ConstantLayouts = new();
    public FProjector[] ConstantProjectors;
    public FMatrix[] ConstantMatrices;
    public FShape[] ConstantShapes;
    public FRichCurve[] ConstantCurves;
    public Dictionary<int, Skeleton> ConstantSkeletons = new();
    public Dictionary<int, PhysicsBody> ConstantPhysicsBodies = new();
    public FParameterDesc[] Parameters;
    public FRangeDesc[] Ranges;
    public ushort[][] ParameterLists;
    

    public FProgram(FArchive Ar)
    {
        OpAddress = Ar.ReadArray<uint>();
        ByteCode = Ar.ReadArray<byte>();
        States = Ar.ReadArray(() => new FState(Ar));
        Roms = Ar.ReadArray(() => new FRomData(Ar));

        var constantImageLodCount = Ar.Read<int>();
        for (var imageLodIndex = 0; imageLodIndex < constantImageLodCount; imageLodIndex++)
        {
            var index = Ar.Read<int>();
            if (index == -1)
            {
                imageLodIndex--;
                continue;
            }

            var image = new Image(Ar);
            if (image.IsBroken) continue;

            ConstantImageLODs[index] = image;
        }
        
        ConstantImageLODIndices = Ar.ReadArray<uint>();
        ConstantImages = Ar.ReadArray(() => new FImageLODRange(Ar));

        var constantMeshCount = Ar.Read<int>();
        for (var meshIndex = 0; meshIndex < constantMeshCount; meshIndex++)
        {
            var index = Ar.Read<int>();
            if (index == -1)
            {
                meshIndex--;
                continue;
            }
            
            var mesh = new Mesh(Ar); 
            if (mesh.IsBroken) continue;

            ConstantMeshes[index] = mesh;
        }

        ConstantExtensionData = Ar.ReadArray(() => new ExtensionData(Ar));
        ConstantStrings = Ar.ReadArray(() => UCustomizableObject.ReadMutableFString(Ar));
        
        var constantLayoutCount = Ar.Read<int>();
        for (var layoutIndex = 0; layoutIndex < constantLayoutCount; layoutIndex++)
        {
            var index = Ar.Read<int>();
            if (index == -1)
            {
                layoutIndex--;
                continue;
            }
            
            var layout = new Layout(Ar); 
            if (layout.IsBroken) continue;

            ConstantLayouts[index] = layout;
        }
        
        ConstantProjectors = Ar.ReadArray(() => new FProjector(Ar));
        ConstantMatrices = Ar.ReadArray(() => new FMatrix(Ar));
        ConstantShapes = Ar.ReadArray(() => new FShape(Ar));
        ConstantCurves = Ar.ReadArray(() => new FRichCurve(Ar));
        
        var constantSkeletonCount = Ar.Read<int>();
        for (var skeletonIndex = 0; skeletonIndex < constantSkeletonCount; skeletonIndex++)
        {
            var index = Ar.Read<int>();
            if (index == -1)
            {
                skeletonIndex--;
                continue;
            }
            
            var skeleton = new Skeleton(Ar); 
            if (skeleton.IsBroken) continue;

            ConstantSkeletons[index] = skeleton;
        }
        
        var constantPhysicsBodyCount = Ar.Read<int>();
        for (var physicsBodyIndex = 0; physicsBodyIndex < constantPhysicsBodyCount; physicsBodyIndex++)
        {
            var index = Ar.Read<int>();
            if (index == -1)
            {
                physicsBodyIndex--;
                continue;
            }
            
            var physicsBody = new PhysicsBody(Ar); 
            if (physicsBody.IsBroken) continue;

            ConstantPhysicsBodies[index] = physicsBody;
        }

        Parameters = Ar.ReadArray(() => new FParameterDesc(Ar));
        Ranges = Ar.ReadArray(() => new FRangeDesc(Ar));
        ParameterLists = Ar.ReadArray(Ar.ReadArray<ushort>);
    }
    
}
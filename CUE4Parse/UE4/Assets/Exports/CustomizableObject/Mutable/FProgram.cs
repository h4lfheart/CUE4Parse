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
    public MutablePtr<Image>[] ConstantImageLODs;
    public uint[] ConstantImageLODIndices;
    public FImageLODRange[] ConstantImages;
    public MutablePtr<Mesh>[] ConstantMeshes;
    public ExtensionData[] ConstantExtensionData;
    public string[] ConstantStrings;
    public MutablePtr<Layout>[] ConstantLayouts;
    public FProjector[] ConstantProjectors;
    public FMatrix[] ConstantMatrices;
    public FShape[] ConstantShapes;
    public FRichCurve[] ConstantCurves;
    public MutablePtr<Skeleton>[] ConstantSkeletons;
    public MutablePtr<PhysicsBody>[] ConstantPhysicsBodies;
    public FParameterDesc[] Parameters;
    public FRangeDesc[] Ranges;
    public ushort[][] ParameterLists;
    

    public FProgram(FArchive Ar)
    {
        OpAddress = Ar.ReadArray<uint>();
        ByteCode = Ar.ReadArray<byte>();
        States = Ar.ReadArray(() => new FState(Ar));
        Roms = Ar.ReadArray(() => new FRomData(Ar));

        ConstantImageLODs = MutablePtr<Image>.ReadArray(Ar);
        
        ConstantImageLODIndices = Ar.ReadArray<uint>();
        ConstantImages = Ar.ReadArray(() => new FImageLODRange(Ar));

        ConstantMeshes = MutablePtr<Mesh>.ReadArray(Ar);

        ConstantExtensionData = Ar.ReadArray(() => new ExtensionData(Ar));
        ConstantStrings = Ar.ReadArray(() => UCustomizableObject.ReadMutableFString(Ar));

        ConstantLayouts = MutablePtr<Layout>.ReadArray(Ar);
        
        ConstantProjectors = Ar.ReadArray(() => new FProjector(Ar));
        ConstantMatrices = Ar.ReadArray(() => new FMatrix(Ar));
        ConstantShapes = Ar.ReadArray(() => new FShape(Ar));
        ConstantCurves = Ar.ReadArray(() => new FRichCurve(Ar));
        
        ConstantSkeletons = MutablePtr<Skeleton>.ReadArray(Ar);
        ConstantPhysicsBodies = MutablePtr<PhysicsBody>.ReadArray(Ar);
        
        Parameters = Ar.ReadArray(() => new FParameterDesc(Ar));
        Ranges = Ar.ReadArray(() => new FRangeDesc(Ar));
        ParameterLists = Ar.ReadArray(Ar.ReadArray<ushort>);
    }
    
}
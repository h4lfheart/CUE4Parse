using CUE4Parse.UE4.Assets.Exports.Animation;
using CUE4Parse.UE4.Assets.Exports.SkeletalMesh;
using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.Assets.Exports.Rig;

public class FDNAIndexMapping
{
    FGuid SkeletonGuid;
    
    // FCachedIndexedCurve ControlAttributeCurves;
    // FCachedIndexedCurve NeuralNetworkMaskCurves;
    FMeshPoseBoneControlAttributeMapping[] DriverJointsToControlAttributesMap;
    // FMeshPoseBoneIndex[]JointsMapDNAIndicesToMeshPoseBoneIndices;
    // FCachedIndexedCurve[] MorphTargetCurvesPerLOD;
    // FCachedIndexedCurve[] MaskMultiplierCurvesPerLOD;
    
    struct FMeshPoseBoneControlAttributeMapping
    {
        // FMeshPoseBoneIndex MeshPoseBoneIndex;
        int DNAJointIndex;
        int RotationX;
        int RotationY;
        int RotationZ;
        int RotationW;
    };
    
    void Init(IDNAReader DNAReader, USkeleton Skeleton, USkeletalMesh SkeletalMesh)
    {
        SkeletonGuid = Skeleton.Guid;
        MapControlCurves(DNAReader, Skeleton);
        MapNeuralNetworkMaskCurves(DNAReader, Skeleton);
        MapJoints(DNAReader, SkeletalMesh);
        MapDriverJoints(DNAReader, SkeletalMesh);
        MapMorphTargets(DNAReader, Skeleton, SkeletalMesh);
        MapMaskMultipliers(DNAReader, Skeleton);
    }
    
    void MapControlCurves(IDNAReader DNAReader, USkeleton Skeleton)
    {
        
    }
    void MapNeuralNetworkMaskCurves(IDNAReader DNAReader, USkeleton Skeleton)
    {
        
    }
    void MapJoints(IDNAReader DNAReader, USkeletalMesh SkeletalMesh)
    {
        
    }
    void MapDriverJoints(IDNAReader DNAReader, USkeletalMesh SkeletalMesh)
    {
        
    }
    void MapMorphTargets(IDNAReader DNAReader, USkeleton Skeleton, USkeletalMesh SkeletalMesh)
    {
        
    }
    void MapMaskMultipliers(IDNAReader DNAReader, USkeleton Skeleton)
    {
        
    }
}
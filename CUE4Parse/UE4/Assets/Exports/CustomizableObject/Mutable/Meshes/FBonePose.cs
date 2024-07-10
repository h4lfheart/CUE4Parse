using System;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Skeletons;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Meshes;

public class FBonePose
{
    public FBoneName BoneId;
    public EBoneUsageFlags Flags;
    public FTransform BoneTransform;
    
    private const int VER_REMOVE_BONE_POSE_VERSION = 22;

    public FBonePose(FArchive Ar, int meshVersion)
    {
        if (meshVersion >= VER_REMOVE_BONE_POSE_VERSION)
        {
            BoneId = new FBoneName(Ar);
            Flags = Ar.Read<EBoneUsageFlags>();
            BoneTransform = Ar.Read<FTransform>();
        }
        else
        {
            var version = Ar.Read<int>();

            if (version <= 1)
            {
                var deprecatedBoneName = UCustomizableObject.ReadMutableFString(Ar);
                
                BoneId = new FBoneName(0);
            }
            else
            {
                BoneId = new FBoneName(Ar);
            }

            if (version == 0)
            {
                var skinned = Ar.Read<byte>() == 1;
                Flags = skinned ? EBoneUsageFlags.Skinning : EBoneUsageFlags.None;
            }
            else
            {
                Flags = Ar.Read<EBoneUsageFlags>();
            }
            
            BoneTransform = Ar.Read<FTransform>();
        }
    }
}

[Flags]
public enum EBoneUsageFlags : uint
{
    None		   = 0,
    Root		   = 1 << 1,
    Skinning	   = 1 << 2,
    SkinningParent = 1 << 3,
    Physics	       = 1 << 4,
    PhysicsParent  = 1 << 5,
    Deform         = 1 << 6,
    DeformParent   = 1 << 7,
    Reshaped       = 1 << 8	
}
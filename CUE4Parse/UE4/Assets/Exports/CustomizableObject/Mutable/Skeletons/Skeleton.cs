using System;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Skeletons;

public class Skeleton
{
    public int Version;

    public FBoneName[] BoneIds;
    public short[] BoneParents;
    
    public ushort[] BoneIds_DEPRECATED;
    public int[] BoneIds_DEPRECATED2;
    public FTransform[] BoneTransforms_DEPRECATED;
    

    public bool IsBroken;
    
    public Skeleton(FArchive Ar)
    {
        Version = Ar.Read<int>();
        if (Version == -1)
        {
            IsBroken = true;
            return;
        }

        if (Version >= 7)
        {
            BoneIds = Ar.ReadArray(() => new FBoneName(Ar));
        }
        else if (Version == 6)
        {
            BoneIds_DEPRECATED = Ar.ReadArray<ushort>();
            BoneIds = new FBoneName[BoneIds_DEPRECATED.Length];
            for (var index = 0; index < BoneIds_DEPRECATED.Length; index++)
            {
                BoneIds[index] = new FBoneName(BoneIds_DEPRECATED[index]);
            }
        }
        else
        {
            throw new NotImplementedException();
        }

        if (Version == 3)
        {
            BoneTransforms_DEPRECATED = Ar.ReadArray<FTransform>();
        }

        BoneParents = Ar.ReadArray<short>();

        if (Version < 6)
        {
            short parentIndex = -1;
            for (var index = 0; index < BoneParents.Length; index++)
            {
                BoneParents[index] = parentIndex;
                parentIndex++;
            }
        }

        if (Version <= 4)
        {
            BoneIds_DEPRECATED2 = Ar.ReadArray<int>();
        }

        if (Version == 3)
        {
            var bBoneTransformModified = Ar.ReadBoolean();
        }
    }
}
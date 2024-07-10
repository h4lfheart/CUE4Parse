using System;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Skeletons;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Physics;

public class PhysicsBody : IMutablePtr
{
    public int CustomId;
    public FPhysicsBodyAggregate[] Bodies;
    public FBoneName[] BoneIds;
    public int[] BodiesCustomIds;
    public bool bBodiesModified;
    
    public ushort[] BoneIds_DEPRECATED;
    public int Version { get; set; }
    
    public void Deserialize(FArchive Ar)
    {
        if (Version >= 2)
        {
            CustomId = Ar.Read<int>();
        }

        Bodies = Ar.ReadArray(() => new FPhysicsBodyAggregate(Ar));

        if (Version >= 4)
        {
            BoneIds = Ar.ReadArray(() => new FBoneName(Ar));
        }
        else if (Version == 3)
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

        BodiesCustomIds = Ar.ReadArray<int>();
        
        if (Version >= 1)
        {
            bBodiesModified = Ar.Read<byte>() == 1;
        }
    }
}
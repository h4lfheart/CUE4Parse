using System;
using System.Collections.Generic;
using System.Linq;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public class UInstancedStaticMeshComponent : UStaticMeshComponent
{
    public FInstancedStaticMeshInstanceData[]? PerInstanceSMData;
    public float[]? PerInstanceSMCustomData;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        ReadExtraData(Ar);

        var bCooked = false;
        if (FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.SerializeInstancedStaticMeshRenderData ||
            FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.SerializeInstancedStaticMeshRenderData)
        {
            bCooked = Ar.ReadBoolean();
        }

        var bHasSkipSerializationPropertiesData = FFortniteMainBranchObjectVersion.Get(Ar) < FFortniteMainBranchObjectVersion.Type.ISMComponentEditableWhenInheritedSkipSerialization || Ar.ReadBoolean();
        if (bHasSkipSerializationPropertiesData)
        {
            PerInstanceSMData = Ar.ReadBulkArray(() => new FInstancedStaticMeshInstanceData(Ar));
            if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.PerInstanceCustomData)
            {
                PerInstanceSMCustomData = Ar.ReadBulkArray(Ar.Read<float>);
            }
        }

        if (bCooked && (FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.SerializeInstancedStaticMeshRenderData ||
                        FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.SerializeInstancedStaticMeshRenderData))
        {
            if (Ar.Game >= EGame.GAME_UE5_4)
            {
                var bHasCookedData = Ar.ReadBoolean();
                if (!bHasCookedData) return;

                Ar.SkipBulkArrayData();
                Ar.SkipBulkArrayData();
                return;
            }

            var renderDataSizeBytes = Ar.Read<ulong>();
            Ar.Position += (long) renderDataSizeBytes;
        }
    }

    protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
    {
        base.WriteJson(writer, serializer);

        if (PerInstanceSMData is { Length: > 0 })
        {
            writer.WritePropertyName("PerInstanceSMData");
            serializer.Serialize(writer, PerInstanceSMData);
        }

        if (PerInstanceSMCustomData is { Length: > 0 })
        {
            writer.WritePropertyName("PerInstanceSMCustomData");
            serializer.Serialize(writer, PerInstanceSMCustomData);
        }
    }

    private void ReadExtraData(FAssetArchive Ar)
    {
        if (!Ar.Owner?.Provider?.InternalGameName.Equals("FortniteGame", StringComparison.OrdinalIgnoreCase) ?? true) return;

        Ar.Position += 4;

        var matchData = new List<byte>(); 
        if (FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.SerializeInstancedStaticMeshRenderData || FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.SerializeInstancedStaticMeshRenderData)
            matchData.AddRange([1, 0, 0, 0]);
        if (FFortniteMainBranchObjectVersion.Get(Ar) > FFortniteMainBranchObjectVersion.Type.ISMComponentEditableWhenInheritedSkipSerialization)
            matchData.AddRange([1, 0, 0, 0]);
        matchData.AddRange([128, 0, 0, 0]);

        long refPosition;
        while (true)
        {
            refPosition = Ar.Position;

            var readData = Ar.ReadBytes(matchData.Count);
            if (readData.SequenceEqual(matchData)) break;
            
            Ar.Position = refPosition + 1;
        }

        Ar.Position = refPosition;
    }
}

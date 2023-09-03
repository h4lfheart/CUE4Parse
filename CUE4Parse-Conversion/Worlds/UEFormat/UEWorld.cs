using System.Collections.Generic;
using System.Linq;
using CUE4Parse_Conversion.Meshes;
using CUE4Parse_Conversion.Meshes.UEFormat;
using CUE4Parse_Conversion.UEFormat;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;
using CUE4Parse.UE4.Assets.Exports.StaticMesh;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse_Conversion.Worlds.UEFormat;

public class UEWorld : UEFormatExport
{
    protected override string Identifier { get; set; } = "UEWORLD";
    
    private readonly Dictionary<int, UEModel> MeshMap = new();
    private readonly List<FActor> Actors = new();

    public UEWorld(UWorld world, ExporterOptions options) : base(world.Name, options)
    {
        if (world.PersistentLevel.TryLoad(out ULevel level))
        {
            SerializeLevelData(level);
        }
    }
    
    public UEWorld(ULevel level, ExporterOptions options) : base(level.Name, options)
    {
        SerializeLevelData(level);
    }

    private void SerializeLevelData(ULevel level)
    {
        ProcessLevel(level, FVector.ZeroVector, FRotator.ZeroRotator);
        
        using (var meshChunk = new FDataChunk("MESHES", MeshMap.Count))
        {
            foreach (var (hash, model) in MeshMap)
            {
                meshChunk.Write(hash);
                
                var modelData = model.ToArray();
                meshChunk.Write(modelData.Length);
                meshChunk.Write(modelData);
            }
            
            meshChunk.Serialize(Ar);
        }
        
        using (var actorChunk = new FDataChunk("ACTORS", Actors.Count))
        {
            foreach (var actor in Actors)
            {
                actor.Serialize(actorChunk);
            }
            
            actorChunk.Serialize(Ar);
        }
    }

    private void ProcessLevel(ULevel level, FVector positionOffset, FRotator rotationOffset)
    {
        foreach (var lazyActor in level.Actors)
        {
            var actor = lazyActor.Load();
            if (actor is null) continue;
            if (actor.ExportType is "LODActor") continue;
            if (actor.Name.StartsWith("LF_")) continue;
            
            ProcessMesh(actor, positionOffset, rotationOffset);
            ProcessAdditionalWords(actor, positionOffset, rotationOffset);
        }
    }
    
    private void ProcessMesh(UObject actor, FVector positionOffset, FRotator rotationOffset)
    {
        if (!actor.TryGetValue(out UStaticMeshComponent staticMeshComponent, "StaticMeshComponent", "StaticMesh", "Mesh", "LightMesh")) return;
        if (!staticMeshComponent.GetStaticMesh().TryLoad(out UStaticMesh staticMesh)) return;

        var hash = staticMesh.GetHashCode();
        if (!MeshMap.ContainsKey(hash) && staticMesh.TryConvert(out var convertedMesh))
        {
            MeshMap[hash] = new UEModel(convertedMesh.LODs.First(), staticMesh.Name, default);
        }

        var position = staticMeshComponent.GetOrDefault("RelativeLocation", FVector.ZeroVector) + positionOffset;
        position.Y = -position.Y;
        
        var rotation = (staticMeshComponent.GetOrDefault("RelativeRotation", FRotator.ZeroRotator) + rotationOffset).Quaternion();
        rotation.Y = -rotation.Y;
        rotation.Z = -rotation.Z;
        
        var scale = staticMeshComponent.GetOrDefault("RelativeScale3D", FVector.OneVector);

        Actors.Add(new FActor(hash, actor.Name, position, rotation.Rotator(), scale));
    }
    
    private void ProcessAdditionalWords(UObject actor, FVector positionOffset, FRotator rotationOffset)
    {
        if (!actor.TryGetValue(out FSoftObjectPath[] additionalWorlds, "AdditionalWorlds")) return;
        if (!actor.TryGetValue(out FPackageIndex lazyStaticMeshComponent, "StaticMeshComponent", "Mesh")) return;
        if (!lazyStaticMeshComponent.TryLoad(out var staticMeshComponent)) return;
        if (staticMeshComponent is null) return;
        
        var position = staticMeshComponent.GetOrDefault("RelativeLocation", FVector.ZeroVector) + positionOffset;
        position.Y = -position.Y;
        
        var rotation = (staticMeshComponent.GetOrDefault("RelativeRotation", FRotator.ZeroRotator) + rotationOffset).Quaternion();
        rotation.Y = -rotation.Y;
        rotation.Z = -rotation.Z;
        
        foreach (var additionalWorldPath in additionalWorlds)
        {
            var additionalWorld = additionalWorldPath.Load<UWorld>();
            if (!additionalWorld.PersistentLevel.TryLoad(out ULevel level)) continue;
            
            ProcessLevel(level, position, rotation.Rotator());
        }
    }
}
using System.Diagnostics;
using CUE4Parse.MappingsProvider.Usmap;
using CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Readers;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject;

public class UCustomizableObject : UObject
{
    public ECustomizableObjectVersions Version;
    public Model Model;
    
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        Version = Ar.Read<ECustomizableObjectVersions>();
        Model = new Model(Ar);
    }
    
    public static string ReadMutableFString(FArchive Ar)
    {
        var length = Ar.Read<int>() * 2;
        return Ar.ReadStringUnsafe(length).Replace("\0", string.Empty);
    }
}

public enum ECustomizableObjectVersions : int
{
    FirstEnumeratedVersion = 450,

    DeterminisiticMeshVertexIds,

    NumRuntimeReferencedTextures,
		
    DeterminisiticLayoutBlockIds,

    BackoutDeterminisiticLayoutBlockIds,

    FixWrappingProjectorLayoutBlockId,

    MeshReferenceSupport,

    ImproveMemoryUsageForStreamableBlocks,

    FixClipMeshWithMeshCrash,

    SkeletalMeshLODSettingsSupport,

    RemoveCustomCurve,

    AddEditorGamePlayTags,

    AddedParameterThumbnailsToEditor,

    ComponentsLODsRedesign,

    ComponentsLODsRedesign2,

    LayoutToPOD,

    AddedRomFlags,

    LayoutNodeCleanup,

    AddSurfaceAndMeshMetadata,

    TablesPropertyNameBug,

    DataTablesParamTrackingForCompileOnlySelected,

    // -----<new versions can be added above this line>--------
    LastCustomizableObjectVersion
};
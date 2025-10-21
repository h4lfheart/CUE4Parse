using System;
using System.IO;
using CUE4Parse_Conversion.PoseAsset;
using CUE4Parse_Conversion.PoseAsset.UEFormat;
using CUE4Parse.UE4.Assets.Exports.Rig;
using CUE4Parse.UE4.Writers;
using CUE4Parse.Utils;
using Serilog;

namespace CUE4Parse_Conversion.DNA;

public class DNAExporter : ExporterBase
{
    private readonly UDNAAsset _dnaAsset;

    public DNAExporter(UDNAAsset dnaAsset, ExporterOptions options) : base(dnaAsset, options)
    {
        _dnaAsset = dnaAsset;
    }
    
    public bool TryConvertToPoseAsset(out PoseAsset.PoseAsset poseAsset)
    {
        poseAsset = null;
        if (!_dnaAsset.TryConvert(out var convertedPoseAsset))
        {
            Log.Warning($"PoseAsset '{ExportName}' failed to convert");
            return false;
        }
        
        var exportName = GetExportSavePath().SubstringAfterLast("/").Replace(':', '_');
        
        using var Ar = new FArchiveWriter();
        string ext;
        switch (Options.PoseFormat) // TODO: Separate DNA format option?
        {
            case EPoseFormat.UEFormat:
                ext = "uepose";
                new UEPose(exportName, convertedPoseAsset, Options).Save(Ar);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Options.PoseFormat), Options.PoseFormat, null);
        }
        
        poseAsset = new PoseAsset.PoseAsset($"{GetExportSavePath().Replace(':', '_')}.{ext}", Ar.GetBuffer());
        return true;
    }

    public override bool TryWriteToDir(DirectoryInfo baseDirectory, out string label, out string savedFilePath)
    {
        var exportSavePath = GetExportSavePath();
        if (!string.IsNullOrEmpty(_dnaAsset.DnaFileName))
        {
            var exportName = Path.GetFileNameWithoutExtension(_dnaAsset.DnaFileName);
            exportSavePath = exportSavePath.SubstringBeforeWithLast('/') + exportName;
        }
        else
        {
            exportSavePath = exportSavePath.Replace(':', '_');
        }
        savedFilePath = FixAndCreatePath(baseDirectory, exportSavePath, "dna");
        label = $"DNA export for '{_dnaAsset.GetPathName()}'";

        if (_dnaAsset.DNAData is null || _dnaAsset.DNAData.Value.Length == 0)
        {
            label = "No DNA data to export.";
            return false;
        }

        File.WriteAllBytesAsync(savedFilePath, _dnaAsset.DNAData.Value);
        return File.Exists(savedFilePath);
    }

    public UDNAAsset GetDNAAsset()
    {
        return _dnaAsset;
    }

    public override bool TryWriteToZip(out byte[] zipFile)
    {
        throw new NotImplementedException();
    }

    public override void AppendToZip()
    {
        throw new NotImplementedException();
    }
}

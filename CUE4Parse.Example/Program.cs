using System;
using System.IO;
using CUE4Parse_Conversion;
using CUE4Parse_Conversion.Meshes;
using CUE4Parse.Compression;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets.Exports.SkeletalMesh;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace CUE4Parse.Example
{
    public static class Program
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Literate).CreateLogger();

            OodleHelper.DownloadOodleDll();
            OodleHelper.Initialize(OodleHelper.OODLE_DLL_NAME);
            
            var provider = new DefaultFileProvider(@"C:\Users\Max\Games\Fortnite\Fortnite\FortniteGame\Content\Paks", SearchOption.AllDirectories, new VersionContainer(EGame.GAME_UE5_6), StringComparer.OrdinalIgnoreCase);
         

            provider.Initialize(); 
            provider.SubmitKey(new FGuid(), new FAesKey("0xA43F7FD912C317930F9AABA5075F0ABCF4EE8A7102582636330BECC449D54560"));

            provider.MappingsContainer = new FileUsmapTypeMappingsProvider(
                @"C:\Users\Max\RiderProjects\FortnitePorting\FortnitePorting\bin\Debug\net8.0-windows10.0.17763.0\win-x64\.data\++Fortnite+Release-36.10-CL-43486998-Windows_oo.usmap");
            provider.PostMount();

            var mesh = provider.LoadPackageObject<USkeletalMesh>(
                "FortniteGame/Content/Characters/Player/Male/Medium/Bodies/M_Med_Soldier_04/Meshes/SK_M_Med_Soldier_04");

            var exporter = new MeshExporter(mesh, new ExporterOptions
            {
                MeshFormat = EMeshFormat.UEFormat,
                ExportMaterials = false
            });
            
            File.WriteAllBytes("C:/Art/bonescale/scale.uemodel", exporter.MeshLods[0].FileData);
        }
    }
}
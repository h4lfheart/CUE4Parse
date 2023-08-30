using System.Collections.Generic;
using CUE4Parse_Conversion.Animations.PSA;
using CUE4Parse_Conversion.UnrealFormat;

namespace CUE4Parse_Conversion.Animations.UnrealFormat;

public class UnrealAnim : UnrealFormatExport
{
    public UnrealAnim(string name, CAnimSet animSet, int sequenceIndex, ExporterOptions options) : base(name, options)
    {
        var sequence = animSet.Sequences[sequenceIndex];
        Ar.Write(sequence.NumFrames);
        Ar.Write(sequence.FramesPerSecond);

        var refSkeleton = animSet.Skeleton.ReferenceSkeleton;
        using (var trackChunk = new FDataChunk("TRACKS", sequence.Tracks.Count))
        {
            for (var i = 0; i < sequence.Tracks.Count; i++)
            {
                var boneName = refSkeleton.FinalRefBoneInfo[i].Name.Text;
                trackChunk.WriteFString(boneName);
                
                var track = sequence.Tracks[i];
                var boneTransform = refSkeleton.FinalRefBonePose[i];
                
                var keys = new List<FBoneKey>();
                for (var f = 0; f < sequence.NumFrames; f++)
                {
                    var key = new FBoneKey(boneTransform);
                    if (sequence.OriginalSequence.FindTrackForBoneIndex(i) >= 0)
                    {
                        track.GetBoneTransform(f, sequence.NumFrames, ref key.Rotation, ref key.Location, ref key.Scale);
                    }
                    
                    key.Rotation.Y = -key.Rotation.Y;
                    key.Rotation.W = -key.Rotation.W;
                    key.Location.Y = -key.Location.Y;
                    keys.Add(key);
                }
                
                trackChunk.WriteArray(keys);
            }
            
            trackChunk.Serialize(Ar);
        }
    }
}
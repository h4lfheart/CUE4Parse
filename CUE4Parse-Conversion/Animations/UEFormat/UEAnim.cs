using System.Collections.Generic;
using CUE4Parse_Conversion.Animations.PSA;
using CUE4Parse_Conversion.UEFormat;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse_Conversion.Animations.UEFormat;

public class UEAnim : UEFormatExport
{
    
    protected override string Identifier { get; set; } = "UEANIM";
    
    public UEAnim(string name, CAnimSet animSet, int sequenceIndex, ExporterOptions options) : base(name, options)
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
                FTransform? lastValidKeyTransform = null;
                for (var frame = 0; frame < sequence.NumFrames; frame++)
                {
                    var key = new FBoneKey(frame, boneTransform);
                    
                    if (sequence.OriginalSequence.FindTrackForBoneIndex(i) >= 0)
                    {
                        track.GetBoneTransform(frame, sequence.NumFrames, ref key.Rotation, ref key.Location, ref key.Scale);
                    }
                    
                    if (lastValidKeyTransform is not null && lastValidKeyTransform.Equals(key.Transform)) continue;
                    lastValidKeyTransform = key.Transform;
                    
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
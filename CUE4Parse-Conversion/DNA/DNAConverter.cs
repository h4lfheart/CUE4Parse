using System;
using System.Collections.Generic;
using System.Linq;
using CUE4Parse_Conversion.PoseAsset.Conversion;
using CUE4Parse.UE4.Assets.Exports.Rig;
using CUE4Parse.UE4.Objects.Core.Math;
using Serilog;

namespace CUE4Parse_Conversion.DNA;

public static class DNAConverter
{
    public static bool TryConvert(this UDNAAsset dna, out CPoseAsset convertedPoseAsset)
    {
        convertedPoseAsset = new CPoseAsset();
        convertedPoseAsset.CurveNames = [..dna.GetRawControlNames().Select(name => name.Replace(".", "_"))];
        
        for (var i = 0; i < dna.GetRawControlCount(); i++)
        {
            var controlName = dna.GetRawControlName(i).Replace(".", "_");
            CPoseData poseData = new()
            {
                PoseName = controlName,
                CurveData = []
            };
            var jointOutputs = CalculateForControlIndex(dna, i);
            
            var n = 0;
            for (var jointIndex = 0; jointIndex < dna.GetJointCount(); ++jointIndex)
            {
                CPoseKey key = new
                (
                    dna.GetJointName(jointIndex),
                    new FVector(
                        jointOutputs[n],
                        jointOutputs[n + 1] * -1,
                        jointOutputs[n + 2]
                    ),
                    new FQuat(new FRotator(
                        jointOutputs[n + 4] * -1,
                        jointOutputs[n + 5] * -1,
                        jointOutputs[n + 3]
                    )),
                    new FVector(
                        jointOutputs[n + 6],
                        jointOutputs[n + 7],
                        jointOutputs[n + 8]
                    )
                );
                
                n += 9;
                
                if (!key.Location.IsZero() || !(key.Rotation.IsIdentity() || key.Rotation.IsVectorZero()) || !key.Scale.IsZero())
                    poseData.Keys.Add(key);
            }

            convertedPoseAsset.Poses.Add(poseData);
        }
        
        return true;
    }
    
    /*
     * Joint group loop from BPCMJointsEvaluator.calculate()
     * https://github.com/EpicGames/UnrealEngine/blob/6978b63c8951e57d97048d8424a0bebd637dde1d/Engine/Plugins/Animation/RigLogic/Source/RigLogicLib/Private/riglogic/joints/cpu/bpcm/BPCMJointsEvaluator.h#L51
    */
    private static List<float> CalculateForControlIndex(UDNAAsset dna, int activeInputIndex)
    {
        // Each joint has 9 values, Position(xyz), Rotation(xyz), Scale(xyz)
        var outputs = new float[dna.GetJointCount() * 9];
        
        foreach (var jointGroup in dna.Behavior.Joints.JointGroups)
        {
            ProcessJointGroup(jointGroup, activeInputIndex, outputs);
        }
        
        return outputs.ToList();
    }
    
    /*
     * Heavily modified version of CalculationStrategy.processJointGroupBlock4()
     * Can remove SIMD/matrix logic since we only ever have 1 control enabled
     * https://github.com/EpicGames/UnrealEngine/blob/6978b63c8951e57d97048d8424a0bebd637dde1d/Engine/Plugins/Animation/RigLogic/Source/RigLogicLib/Private/riglogic/joints/cpu/bpcm/CalculationStrategy.h#L272
     */
    private static void ProcessJointGroup(RawJointGroup jointGroup, int controlIndex, float[] outputs)
    {
        var outputCount = jointGroup.LODs[0];
        if (outputCount == 0)  return;
        
        // Find offset for the current control index in this joint group
        var offset = Array.IndexOf(jointGroup.InputIndices, (ushort)controlIndex);
        if (offset == -1) return; // This joint group doesn't contain data for the current control
        
        // Copy values to output following value storage pattern
        // ex:
        // inputIndices = [a, b, c, d]
        // outputCount = 2
        // values = [a1, b1, c1, d1, a2, b2, c2, d2]
        var values = jointGroup.Values;
        for (var outIdx = 0; outIdx < outputCount && outIdx < jointGroup.OutputIndices.Length; outIdx++)
        {
            var valueIndex = offset + (outIdx * jointGroup.InputIndices.Length);
            if (valueIndex >= values.Length) continue;
            
            var outputIndex = jointGroup.OutputIndices[outIdx];
            outputs[outputIndex] = values[valueIndex];
        }
    }
}
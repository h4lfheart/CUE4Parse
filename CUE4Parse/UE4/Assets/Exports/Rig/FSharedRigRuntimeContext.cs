using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.Rig;

public class FSharedRigRuntimeContext
{
    private IDNAReader BehaviorReader;
    private FRigLogic RigLogic;
    private uint[][] VariableJointIndicesPerLOD;
    private FQuat[] InverseNeutralJointRotations;
}
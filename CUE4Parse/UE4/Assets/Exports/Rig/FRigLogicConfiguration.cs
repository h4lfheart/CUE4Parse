namespace CUE4Parse.UE4.Assets.Exports.Rig;

public class FRigLogic
{
    // Shared memory resource?
    //public FMemoryResource MemoryResource;
    public FRigLogicConfiguration Configuration;
}

public class FRigLogicConfiguration
{
    [UProperty] public ERigLogicCalculationType CalculationType;
    [UProperty] public bool LoadJoints;
    [UProperty] public bool LoadBlendShapes;
    [UProperty] public bool LoadAnimatedMaps;
    [UProperty] public bool LoadMachineLearnedBehavior;
    [UProperty] public bool LoadRBFBehavior;
    [UProperty] public bool LoadTwistSwingBehavior;
    [UProperty] public ERigLogicTranslationType TranslationType;
    [UProperty] public ERigLogicRotationType RotationType;
    [UProperty] public ERigLogicRotationOrder RotationOrder;
    [UProperty] public ERigLogicScaleType ScaleType;
    [UProperty] public float TranslationPruningThreshold;
    [UProperty] public float RotationPruningThreshold;
    [UProperty] public float ScalePruningThreshold;
}

public enum ERigLogicCalculationType : uint
{
    Scalar,
    SSE,
    AVX,
    NEON,
    AnyVector
};

public enum ERigLogicTranslationType : uint
{
    None,
    Vector = 3
};

public enum ERigLogicRotationType : uint
{
    None,
    EulerAngles = 3,
    Quaternions = 4
};

public enum ERigLogicRotationOrder : uint
{
    XYZ,
    XZY,
    YXZ,
    YZX,
    ZXY,
    ZYX
};

public enum ERigLogicScaleType : uint
{
    None,
    Vector = 3
};
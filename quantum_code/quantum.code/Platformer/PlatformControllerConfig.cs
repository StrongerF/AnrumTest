using Photon.Deterministic;

namespace Quantum
{
    public partial class PlatformControllerConfig
    {
        public bool ApplyRotationInertia = true;
        
        public FP PlayerHeadPenetrationCorrectionFactor = FP._0_10 + FP._0_05;
        public FP HeadOffset = 1 + FP._0_50;

        // what axis' we should apply inertia to 
        public PlatformAxis PlatformAxisInertia = PlatformAxis.X | PlatformAxis.Y | PlatformAxis.Z;
    }
}
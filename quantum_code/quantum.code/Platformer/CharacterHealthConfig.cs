using Photon.Deterministic;
using Quantum.Inspector;

namespace Quantum
{
    public partial class CharacterHealthConfig : AssetObject
    {
        [Header("Health Settings")]
        public FP MaxHealth = FP._100;

        [Header("Fall Settings")]
        public FP FallDamageThreshold = 20;        // meters per second
        public FP FallDamageMultiplier = FP._5;
    }
}

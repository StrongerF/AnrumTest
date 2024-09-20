using Photon.Deterministic;
using Quantum.Physics3D;

namespace Quantum.Platformer
{
    public unsafe class FallDamageSystem : SystemMainThreadFilter<FallDamageSystem.Filter>, ISignalOnCharacterFallen
    {
        public struct Filter
        {
            public EntityRef Entity;
            public CharacterController3D* CharacterController;
            public CharacterHealth* CharacterHealth;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (!filter.CharacterController->Grounded)
            {
                filter.CharacterHealth->PreviousVelocity = filter.CharacterController->Velocity;
            }
        }

        public void OnCharacterFallen(Frame f, EntityRef character, Hit3D hit)
        {
            var characterHealth = f.Unsafe.GetPointer<CharacterHealth>(character);
            if (characterHealth->PreviousVelocity != FPVector3.Zero)
            {
                // true = ground
                if (IsCollisionGrounded(f, character, hit))
                {
                    var damageSpeed = CalculateDamageSpeed(f, character, hit);

                    if (damageSpeed > FP._0)
                    {
                        CharacterFallDamage(f, character, damageSpeed);
                    }
                }

                characterHealth->PreviousVelocity = FPVector3.Zero;
            }
        }

        public bool IsCollisionGrounded(Frame f, EntityRef character, Hit3D hit)
        {
            var characterController = f.Unsafe.GetPointer<CharacterController3D>(character);
            CharacterController3DConfig config = f.FindAsset(characterController->Config);
            var transform = f.Unsafe.GetPointer<Transform3D>(character);

            FPVector3 contactToCenter = (transform->Position + config.Offset - hit.Point).Normalized;

            FP angle = FPVector3.Angle(-config.GravityNormalized, contactToCenter);

            return angle <= config.MaxSlope;
        }

        public FP CalculateDamageSpeed(Frame f, EntityRef character, Hit3D hit)
        {
            var healthComponent = f.Unsafe.GetPointer<CharacterHealth>(character);
            var healthConfig = f.FindAsset<CharacterHealthConfig>(healthComponent->Config.Id);
            FPVector3 relativeVelocity = healthComponent->PreviousVelocity;
            if (f.Unsafe.TryGetPointer(hit.Entity, out Platform* platform))
            {
                // softening the fall if the platform moves down
                relativeVelocity += platform->PositionDelta;
            }

            // velocity Y is negative when falling
            FP fallSpeed = -relativeVelocity.Y - healthConfig.FallDamageThreshold;

            return fallSpeed;
        }

        public void CharacterFallDamage(Frame f, EntityRef character, FP damageSpeed)
        {
            var healthComponent = f.Unsafe.GetPointer<CharacterHealth>(character);
            var healthConfig = f.FindAsset<CharacterHealthConfig>(healthComponent->Config.Id);

            FP damage = damageSpeed * healthConfig.FallDamageMultiplier;

            f.Signals.OnCharacterDamaged(character, damage);
        }

    }
}

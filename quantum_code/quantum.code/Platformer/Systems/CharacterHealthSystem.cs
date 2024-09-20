using Photon.Deterministic;
using Quantum.Core;
using Quantum.Physics3D;

namespace Quantum.Platformer
{
    public unsafe class CharacterHealthSystem : SystemSignalsOnly, ISignalOnComponentAdded<CharacterHealth>, ISignalOnCharacterDamaged
    {

        public void OnAdded(Frame f, EntityRef entity, CharacterHealth* component)
        {
            component->UpdateStats(f, entity);
        }

        public void OnCharacterDamaged(Frame f, EntityRef character, FP damage)
        {
            CharacterHealth* component = f.Unsafe.GetPointer<CharacterHealth>(character);

            component->ApplyDamage(f, character, damage);
        }
    }
}

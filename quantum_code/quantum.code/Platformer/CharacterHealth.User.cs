using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public partial struct CharacterHealth
    {
        public void UpdateStats(Frame f, EntityRef character)
        {
            var config = f.FindAsset<CharacterHealthConfig>(Config.Id);

            CurrentHealth = config.MaxHealth;
            f.Events.HealthUpdated(character, CurrentHealth);
        }

        public void ApplyDamage(Frame f, EntityRef character, FP damage)
        {
            CurrentHealth = FPMath.Max(CurrentHealth - damage, FP._0);

            f.Events.HealthUpdated(character, CurrentHealth);

            if (!IsDead && CurrentHealth <= 0)
            {
                IsDead = true;
                f.Events.PlayerDied(character);
                f.Destroy(character);
            }
        }
    }
}

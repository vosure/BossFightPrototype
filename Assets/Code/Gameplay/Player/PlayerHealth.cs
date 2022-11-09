using Code.Gameplay.DamageSystem;
using UnityEngine;

namespace Code.Gameplay.Player
{
    public class PlayerHealth : LivingEntity
    {
        private const float TimeToDie = 2.0f;
        protected override void OnDie() => 
            Destroy(gameObject, TimeToDie);
    }
}

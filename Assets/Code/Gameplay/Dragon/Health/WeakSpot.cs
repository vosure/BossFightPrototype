using System;
using Code.Gameplay.DamageSystem;
using UnityEngine;

namespace Code.Gameplay.Dragon.Health
{
	public class WeakSpot : MonoBehaviour, IDamageable
	{
		private int _health;

		public event Action<WeakSpot> OnWeakSpotEliminated;
		
		public void Initialize(int startingHealth) => 
			_health = startingHealth;

		public void TakeDamage(int damage)
		{
			_health -= damage;
			if (_health <= 0)
				OnDestroy();
		}

		public void Enable() => 
			gameObject.SetActive(true);

		public void Disable() => 
			gameObject.SetActive(false);

		private void OnDestroy()
		{
			OnWeakSpotEliminated?.Invoke(this);
			
			Disable();
			Destroy(gameObject);
		}
	}
}
using System;
using Code.Audio.EnumTypes;
using Code.Gameplay.DamageSystem;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Dragon.Health
{
	public class DragonHealth : LivingEntity
	{
		[SerializeField] private float timeToDie;
		public event Action OnHalfHealthLost;
		
		private bool _halfHealthOrLessRemaining;

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);

			ServiceLocator.Instance.AudioService.PlaySound2D(SoundType.DragonTakeDamage);
			CheckRemainingHealth();
		}

		private void CheckRemainingHealth()
		{
			if (!_halfHealthOrLessRemaining)
			{
				if (CurrentHealth < (StartingHealth / 2))
				{
					_halfHealthOrLessRemaining = true;
					OnHalfHealthLost?.Invoke();
				}
			}
		}

		protected override void OnDie()
		{
			ServiceLocator.Instance.AudioService.PlaySound2D(SoundType.DragonDie);
			Destroy(gameObject, timeToDie);
		}
	}
}
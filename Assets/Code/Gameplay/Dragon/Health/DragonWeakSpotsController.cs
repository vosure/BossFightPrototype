using System;
using System.Collections.Generic;
using Code.Gameplay.DamageSystem;
using Code.Infrastructure.Data.Vfx;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Dragon.Health
{
	public class DragonWeakSpotsController : MonoBehaviour
	{
		[SerializeField] private List<WeakSpot> weakSpots;
		[SerializeField] private int weakSpotHealth;

		private int _weakSpotsCount;
		private int _damagePerWeakSpot;
		private int _currentWeakSpotIndex;

		private LivingEntity _dragonHealth;
		private VFXFactory _vfxFactory;

		private void Awake()
		{
			_dragonHealth = GetComponent<DragonHealth>();
			_vfxFactory = ServiceLocator.Instance.VfxFactory;
		}

		private void Start() =>
			Initialize();

		private void OnDisable() =>
			Unsubscribe();

		private void Initialize()
		{
			_weakSpotsCount = weakSpots.Count;

			int dragonHealth = _dragonHealth.StartingHealth;
			_damagePerWeakSpot = (int) Math.Ceiling((double) dragonHealth / _weakSpotsCount);

			SubscribeAndInitializeWeakSpots();
		}

		public void EnableWeakSpot()
		{
			if (IndexInRange())
				weakSpots[_currentWeakSpotIndex].Enable();
		}

		public void DisableWeakSpot()
		{
			if (IndexInRange())
				weakSpots[_currentWeakSpotIndex].Disable();
		}

		private void SubscribeAndInitializeWeakSpots()
		{
			foreach (WeakSpot weakSpot in weakSpots)
			{
				weakSpot.OnWeakSpotEliminated += OnWeakSpotEliminated;
				weakSpot.Initialize(weakSpotHealth);
				weakSpot.Disable();
			}
		}

		private void Unsubscribe()
		{
			foreach (WeakSpot weakSpot in weakSpots)
				weakSpot.OnWeakSpotEliminated -= OnWeakSpotEliminated;
		}

		private bool IndexInRange() =>
			_currentWeakSpotIndex < weakSpots.Count;

		private void OnWeakSpotEliminated(WeakSpot weakSpot)
		{
			weakSpot.OnWeakSpotEliminated -= OnWeakSpotEliminated;

			_vfxFactory.SpawnVfx(VFXType.WeakSpotExplosion, weakSpot.transform.position, Quaternion.identity);
			_dragonHealth.TakeDamage(_damagePerWeakSpot);
			_currentWeakSpotIndex++;
		}
	}
}
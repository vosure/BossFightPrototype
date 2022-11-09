using System;
using System.Collections;
using Code.Audio.EnumTypes;
using Code.Gameplay.Dragon.Health;
using Code.Gameplay.Player;
using Code.Infrastructure.Data.StaticData.Weapons;
using Code.Infrastructure.Data.Vfx;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services;
using Code.Utility;
using UnityEngine;

namespace Code.Gameplay.WeaponSystem
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private Transform projectileSpawnPoint;
		[SerializeField] private Transform muzzleFlashSpawnPoint;
		[SerializeField] private Transform shellSpawnPoint;

		private FireMode _fireMode;
		private int _bulletsPerMagazine;
		private float _fireRate;
		private float _reloadTime;
		private float _accuracyErrorValue;
		private int _damage;

		private bool _triggerReleasedSinceLastShoot = true;
		private float _nextShotTime;
		int _bulletsRemainingInMagazine;
		bool _isReloading;

		private PlayerAim _playerAim;
		private VFXFactory _vfxFactory;
		private GameFactory _gameFactory;

		public event Action<int> OnBulletsInMagazineChanged;
		public int GetBulletsPerMagazine() => _bulletsPerMagazine;

		private int BulletsRemainingInMagazine
		{
			get => _bulletsRemainingInMagazine;
			set
			{
				_bulletsRemainingInMagazine = value;
				OnBulletsInMagazineChanged?.Invoke(_bulletsRemainingInMagazine);
			}
		}

		private void LateUpdate()
		{
			if (!_isReloading && MagazineIsEmpty())
				Reload();
		}

		public void Initialize(WeaponStaticData weaponData, GameFactory gameFactory, PlayerAim playerAim,
			VFXFactory vfxFactory)
		{
			_playerAim = playerAim;
			_vfxFactory = vfxFactory;
			_gameFactory = gameFactory;

			InitializeWeaponData(weaponData);
		}

		private void InitializeWeaponData(WeaponStaticData data)
		{
			_fireMode = data.FireMode;
			_bulletsPerMagazine = data.BulletsPerMagazine;
			_fireRate = data.FireRate;
			_reloadTime = data.ReloadTime;
			_accuracyErrorValue = data.AccuracyErrorValue;
			_damage = data.Damage;

			_bulletsRemainingInMagazine = _bulletsPerMagazine;
		}


		private void Shoot()
		{
			if (CanShoot())
			{
				if (_fireMode == FireMode.Single)
				{
					if (!_triggerReleasedSinceLastShoot)
						return;
				}

				BulletsRemainingInMagazine--;
				_nextShotTime = Time.time + _fireRate / 1000.0f;

				TryHit();
				PlayOnShootVFX();
				PlayShootSound();
			}
		}

		private void PlayOnShootVFX()
		{
			_vfxFactory.SpawnVfx(VFXType.MuzzleFlash, muzzleFlashSpawnPoint);
			_vfxFactory.SpawnVfx(VFXType.BulletShell, shellSpawnPoint);
		}

		private void PlayShootSound() =>
			ServiceLocator.Instance.AudioService.PlaySound2D(SoundType.Shoot);

		private void TryHit()
		{
			var projectile = SpawnProjectile(_playerAim.GetMousePosition());
			if (_playerAim.TryGetHitTarget(out RaycastHit hit))
			{
				bool hitDamageableTarget = TryApplyDamage(hit, projectile);
				SpawnVfx(hitDamageableTarget, hit, projectile);
			}
		}

		private void SpawnVfx(bool hitDamageableTarget, RaycastHit hit, Projectile projectile)
		{
			if (hitDamageableTarget)
				SpawnDamageImpactVfx(hit, projectile);
			else
				SpawnImpactVfx(hit, projectile);
		}

		private bool TryApplyDamage(RaycastHit hit, Projectile projectile)
		{
			if (hit.collider.TryGetComponent(out WeakSpot weakPoint))
			{
				weakPoint.TakeDamage(_damage);
				return true;
			}

			return false;
		}

		private void SpawnImpactVfx(RaycastHit hit, Projectile projectile) =>
			_vfxFactory.SpawnVfx(VFXType.WallImpact, hit.point, GetImpactRotation(projectile), hit.transform);

		private void SpawnDamageImpactVfx(RaycastHit hit, Projectile projectile) =>
			_vfxFactory.SpawnVfx(VFXType.DamageImpact, hit.point, GetImpactRotation(projectile), hit.transform);

		private Projectile SpawnProjectile(Vector3 hitPoint)
		{
			Vector3 targetPosition =
				hitPoint.AddRandomDistortion(_accuracyErrorValue, _accuracyErrorValue, _accuracyErrorValue);
			Vector3 projectilePosition = projectileSpawnPoint.position;

			Vector3 flyDirection = (targetPosition - projectilePosition).normalized;
			Projectile projectile = _gameFactory.SpawnBulletProjectile(projectilePosition,
				GetProjectileRotation(flyDirection));

			return projectile;
		}

		public void Reload()
		{
			if (!_isReloading && MagazineIsNotFull())
			{
				StartCoroutine(AnimateReload());
				ServiceLocator.Instance.AudioService.PlaySound2D(SoundType.Reload);
			}
		}

		IEnumerator AnimateReload()
		{
			_isReloading = true;

			//TODO(vlad): Add Animation

			yield return new WaitForSeconds(_reloadTime);

			_isReloading = false;
			BulletsRemainingInMagazine = _bulletsPerMagazine;
		}

		public void OnTriggerHold()
		{
			Shoot();
			_triggerReleasedSinceLastShoot = false;
		}

		public void OnTriggerRelease() =>
			_triggerReleasedSinceLastShoot = true;

		private static Quaternion GetProjectileRotation(Vector3 dir) =>
			Quaternion.LookRotation(dir, Vector3.up);

		private static Quaternion GetImpactRotation(Projectile projectile) =>
			Quaternion.FromToRotation(Vector3.forward, projectile.transform.forward);

		private bool MagazineIsEmpty() =>
			BulletsRemainingInMagazine == 0;

		private bool MagazineIsNotFull() =>
			BulletsRemainingInMagazine != _bulletsPerMagazine;

		private bool CanShoot() =>
			!_isReloading && Time.time > _nextShotTime && !MagazineIsEmpty();
	}
}
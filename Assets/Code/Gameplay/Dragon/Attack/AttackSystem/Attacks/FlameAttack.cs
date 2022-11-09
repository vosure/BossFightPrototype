using System.Collections;
using Code.Audio.EnumTypes;
using Code.Gameplay.Dragon.Attack.AttackSystem.Projectiles;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Dragon.Attack.AttackSystem.Attacks
{
	public class FlameAttack : AttackBase
	{
		private const float TimeBetweenProjectiles = 0.15f;

		[SerializeField] private Transform projectileSpawnPosition;
		[SerializeField] private int projectilesToSpawnPerCast;
		[SerializeField] private int castPerAttack;

		private DragonController _dragonController;
		private GameFactory _gameFactory;

		protected override void Awake()
		{
			base.Awake();

			_dragonController = GetComponent<DragonController>();
		}

		private void Start() =>
			SetUp();

		private void SetUp()
		{
			_gameFactory = ServiceLocator.Instance.GameFactory;

			_attackAnimationTime = _animator.GetFlameAttackAnimationLength();
		}

		public override float GetAttackTime() =>
			_attackAnimationTime * castPerAttack;

		public override void PerformAttack() =>
			StartCoroutine(SpawnProjectilesCoroutine());

		private IEnumerator SpawnProjectilesCoroutine()
		{
			SetOnAttackStateChanged(true);

			for (int castCounter = 0; castCounter < castPerAttack; castCounter++)
			{
				_animator.StartFlameAttackAnimation();
				
				yield return new WaitForSeconds(GetHalfFlameAnimationTime());
				
				ServiceLocator.Instance.AudioService.PlaySound2D(SoundType.DragonFireBreath);
				for (int projectileCounter = 0; projectileCounter < projectilesToSpawnPerCast; projectileCounter++)
				{
					SpawnProjectile();
					yield return new WaitForSeconds(TimeBetweenProjectiles);
				}

				yield return new WaitForSeconds(GetHalfFlameAnimationTime());
			}
			
			SetOnAttackStateChanged(false);
		}

		private void SpawnProjectile()
		{
			var spawnPosition = projectileSpawnPosition.position;
			Vector3 flyDirection = (_dragonController.PlayerTransform.position - spawnPosition)
				.normalized;
			SelfDirectedDragonProjectile newProjectile = _gameFactory.SpawnSelfDirectedFireballProjectile(spawnPosition,
				Quaternion.LookRotation(flyDirection, Vector3.up));

			newProjectile.SetTarget(_dragonController.PlayerTransform);
		}

		private float GetHalfFlameAnimationTime() =>
			_attackAnimationTime / 2;
	}
}
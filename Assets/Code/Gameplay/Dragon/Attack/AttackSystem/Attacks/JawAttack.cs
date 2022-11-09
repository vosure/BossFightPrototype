using System.Collections;
using Code.Audio.EnumTypes;
using Code.Gameplay.Dragon.Attack.AttackSystem.Projectiles;
using Code.Gameplay.Player;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Dragon.Attack.AttackSystem.Attacks
{
	public class JawAttack : AttackBase
	{
		private const string PlayerTag = "Player";

		[SerializeField] private Transform projectileSpawnPosition;
		[SerializeField] private Transform jawBitePoint;
		[SerializeField] private int castsPerAttack;
		[SerializeField] private int biteDamage;
		[SerializeField] private float biteDistance;

		private GameFactory _gameFactory;
		private Transform _playerTransform;

		private void Start() =>
			SetUp();
		
		private void SetUp()
		{
			_gameFactory = ServiceLocator.Instance.GameFactory;
			_playerTransform =  GameObject.FindGameObjectWithTag(PlayerTag).transform;

			_attackAnimationTime = _animator.GetJawAttackAnimationLength();
		}

		public override void PerformAttack() => 
			StartCoroutine(BiteAttackCoroutine());

		public override float GetAttackTime() => 
			_attackAnimationTime * castsPerAttack;

		private IEnumerator BiteAttackCoroutine()
		{
			SetOnAttackStateChanged(true);

			for (int castCounter = 0; castCounter < castsPerAttack; castCounter++)
			{
				_animator.StartJawAttackAnimation();
				yield return new WaitForSeconds(_attackAnimationTime / 1.5f);
				
				ServiceLocator.Instance.AudioService.PlaySound2D(SoundType.DragonBite);
				SpawnFastFireball();
				TryBite();
				
				yield return new WaitForSeconds(_attackAnimationTime / 2);
			}
			SetOnAttackStateChanged(false);
		}

		private void SpawnFastFireball()
		{
			var spawnPosition = projectileSpawnPosition.position;
			FastFireball newProjectile = _gameFactory.SpawnFastFireball(spawnPosition, Quaternion.identity);
			newProjectile.SetTarget(_playerTransform);
		}

		private void TryBite()
		{
			if (GetDistance() < biteDistance) 
				_playerTransform.GetComponent<PlayerHealth>().TakeDamage(biteDamage);
		}

		private float GetDistance() => 
			Vector3.Distance(_playerTransform.position, jawBitePoint.position);
	}
}
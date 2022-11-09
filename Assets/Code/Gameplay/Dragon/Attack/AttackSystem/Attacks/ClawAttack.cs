using System.Collections;
using Code.Audio.EnumTypes;
using Code.Gameplay.Player;
using Code.Gameplay.Player.Movement;
using Code.Infrastructure.Data.Vfx;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Dragon.Attack.AttackSystem.Attacks
{
	public class ClawAttack : AttackBase
	{
		private const string PlayerTag = "Player";

		[SerializeField] private int hitsPerAttack;
		[SerializeField] private int damage;
		[SerializeField] private float distanceToHit;

		private GameObject _player;
		private Transform _playerTransform;
		private PlayerJump _playerJump;
		private PlayerHealth _playerHealth;

		private VFXFactory _vfxFactory;

		private void Start() =>
			SetUp();

		private void SetUp()
		{
			_vfxFactory = ServiceLocator.Instance.VfxFactory;
			
			GetPlayerComponents();
			
			_attackAnimationTime = _animator.GetClawAttackAnimationLength();
		}

		private void GetPlayerComponents()
		{
			_player = GameObject.FindGameObjectWithTag(PlayerTag);
			_playerJump = _player.GetComponent<PlayerJump>();
			_playerHealth = _player.GetComponent<PlayerHealth>();
			_playerTransform = _player.transform;
		}

		public override float GetAttackTime() =>
			_attackAnimationTime * hitsPerAttack;

		public override void PerformAttack() =>
			StartCoroutine(ClawAttackCoroutine());

		private IEnumerator ClawAttackCoroutine()
		{
			SetOnAttackStateChanged(true);

			for (int hit = 0; hit < hitsPerAttack; hit++)
			{
				if (_playerTransform != null)
				{
					_animator.StartClawAttackAnimation();
					yield return new WaitForSeconds(GetHalfAnimationTime());
					ServiceLocator.Instance.AudioService.PlaySound2D(SoundType.DragonClawAttack);
					SpawnVfx();
					yield return new WaitForSeconds(_attackAnimationTime);
				}
			}

			SetOnAttackStateChanged(false);
		}

		private void SpawnVfx()
		{
			if (_playerTransform != null)
			{
				Transform groundSlam = _vfxFactory.SpawnVfx(VFXType.GroundSlam,
					GetMiddlePointBetweenDragonAndPlayer(transform, _playerTransform), Quaternion.identity);

				CheckPlayerInRange(groundSlam);
			}
		}

		private void CheckPlayerInRange(Transform hitTransform)
		{
			float distance = GetDistance(hitTransform);
			if (IsPlayerInRangeOrGrounded(distance)) 
				_playerHealth.TakeDamage(damage);
		}

		private float GetDistance(Transform hitTransform) => 
			Vector3.Distance(_playerTransform.position, hitTransform.position);

		private float GetHalfAnimationTime() => 
			_attackAnimationTime / 2;

		private bool IsPlayerInRangeOrGrounded(float distance) => 
			distance < distanceToHit || _playerJump.Grounded;

		private Vector3 GetMiddlePointBetweenDragonAndPlayer(Transform dragon, Transform player) =>
			Vector3.Lerp(dragon.position, player.position, 0.5f);
	}
}
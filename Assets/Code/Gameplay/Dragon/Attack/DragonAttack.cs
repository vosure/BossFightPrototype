using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Dragon.Attack.AttackSystem;
using Code.Gameplay.Dragon.Health;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Gameplay.Dragon.Attack
{
	public class DragonAttack : MonoBehaviour
	{
		[SerializeField] private List<AttackBase> attacksList;
		
		[SerializeField] private float attackCooldown = 5.5f;
		
		private bool _isAttacking;
		private float _nextAttackTime;
		
		private DragonHealth _dragonHealth;
		private DragonAnimator _dragonAnimator;
		private DragonWeakSpotsController _weakSpotsController;

		private AttackBase _currentAttack;
		private bool _isNewAttackUnlocked = false;

		private void Awake()
		{
			GetDragonComponents();
			SetUpInitialAttack();
			Subscribe();
		}

		private void Update()
		{
			if (CanAttack()) 
				StartNewAttack();
		}

		private void OnDisable() => 
			Unsubscribe();

		private void Subscribe()
		{
			_dragonHealth.OnHalfHealthLost += EnableSecondStage;
			_dragonHealth.OnHealthChanged += IncreaseIdleTime; 
			
			foreach (AttackBase attackBase in attacksList)
			{
				attackBase.OnAttackStarted += EnableAttack;
				attackBase.OnAttackEnded += DisableAttack;
			}
		}

		private void GetDragonComponents()
		{
			_dragonHealth = GetComponent<DragonHealth>();
			_dragonAnimator = GetComponent<DragonAnimator>();
			_weakSpotsController = GetComponent<DragonWeakSpotsController>();
		}

		private void SetUpInitialAttack()
		{
			_currentAttack = attacksList.First(a => a.AttackType == AttackType.ClawAttack);
			_nextAttackTime = attackCooldown;
		}

		private void EnableAttack()
		{
			_isAttacking = true;
			_weakSpotsController.DisableWeakSpot();
		}

		private void DisableAttack()
		{
			_isAttacking = false;
			_weakSpotsController.EnableWeakSpot();
		}

		private AttackBase GetNextAttack()
		{
			AttackType currentAttackType = _currentAttack.AttackType;
			AttackType nextAttackType;

			if (_isNewAttackUnlocked)
			{
				if (currentAttackType == AttackType.JawAttack)
					nextAttackType = Random.value > 0.5f ? AttackType.FlameAttack : AttackType.ClawAttack;
				else
					nextAttackType = AttackType.JawAttack;
			}
			else
			{
				if (currentAttackType == AttackType.ClawAttack)
					nextAttackType = AttackType.FlameAttack;
				else
					nextAttackType = AttackType.ClawAttack;
			}

			return GetAttackByType(nextAttackType);
		}

		private AttackBase GetAttackByType(AttackType attackType) => 
			attacksList.First(a => a.AttackType == attackType);

		private float GetAttackTime() => 
			_currentAttack.GetAttackTime();

		private bool CanAttack() => 
			Time.time > _nextAttackTime && !_isAttacking && !_dragonHealth.IsDead;

		private void StartNewAttack()
		{
			_currentAttack = GetNextAttack();
			_nextAttackTime = Time.time + GetAttackTime() + attackCooldown;
			_currentAttack.PerformAttack();
		}

		private void Unsubscribe()
		{
			_dragonHealth.OnHalfHealthLost -= EnableSecondStage; 
			_dragonHealth.OnHealthChanged -= IncreaseIdleTime; 
			
			foreach (AttackBase attackBase in attacksList)
			{
				attackBase.OnAttackStarted -= EnableAttack;
				attackBase.OnAttackEnded -= DisableAttack;
			}
		}

		private void IncreaseIdleTime(int health) => 
			_nextAttackTime += _dragonAnimator.GetTakeDamageAnimationLength();

		private void EnableSecondStage() => 
			_isNewAttackUnlocked = true;
	}
}
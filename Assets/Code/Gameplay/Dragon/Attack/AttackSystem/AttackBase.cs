using System;
using UnityEngine;

namespace Code.Gameplay.Dragon.Attack.AttackSystem
{
	public abstract class AttackBase : MonoBehaviour, IAttack
	{
		[SerializeField] private AttackType attackType;

		public AttackType AttackType => attackType;
		
		public event Action OnAttackEnded;
		public event Action OnAttackStarted;
		
		protected float _attackAnimationTime;

		protected DragonAnimator _animator;
		
		protected virtual void Awake()
		{
			_animator = GetComponent<DragonAnimator>();
		}

		protected void SetOnAttackStateChanged(bool newState)
		{
			if (newState)
				OnAttackStarted?.Invoke();
			else
				OnAttackEnded?.Invoke();
		}

		public abstract float GetAttackTime();

		public abstract void PerformAttack();
	}
}
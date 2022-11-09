using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Dragon.Health;
using UnityEngine;

namespace Code.Gameplay.Dragon
{
	public class DragonAnimator : MonoBehaviour
	{
		private const int FirstIdleVariationValue = 0;
		private const int SecondIdleVariationValue = 1;
		
		private readonly int FlameAttackAnimationID = Animator.StringToHash("FlameAttack");
		private const string FlameAttackAnimationName = "Flame Attack";
		
		private readonly int ClawAttackAnimationID = Animator.StringToHash("ClawAttack");
		private const string ClawAttackAnimationName = "Claw Attack";
		
		private readonly int JawAttackAnimationID = Animator.StringToHash("JawAttack");
		private const string JawAttackAnimationName = "Basic Attack";

		private readonly int TakeDamageAnimationID = Animator.StringToHash("TakeDamage");
		private const string TakeDamageAnimationName = "Scream";
		
		private readonly int IdleAnimationID = Animator.StringToHash("IdleVariationBlend");
		private readonly int DieAnimationID = Animator.StringToHash("Die");
		
		private Animator _animator;
		private DragonHealth _dragonHealth;

		private List<AnimationClip> _animationClips;

		private void Awake()
		{
			GetComponents();

			Subscribe();
			GetAnimationClips();
		}

		private void GetComponents()
		{
			_animator = GetComponent<Animator>();
			_dragonHealth = GetComponent<DragonHealth>();
		}

		private void OnDisable() => 
			Unsubscribe();
		
		public void StartFlameAttackAnimation() =>
			_animator.SetTrigger(FlameAttackAnimationID);

		public float GetFlameAttackAnimationLength() => 
			GetClipLength(FlameAttackAnimationName);

		public void StartClawAttackAnimation() =>
			_animator.SetTrigger(ClawAttackAnimationID);

		public float GetClawAttackAnimationLength() => 
			GetClipLength(ClawAttackAnimationName);
		
		public void StartJawAttackAnimation() =>
			_animator.SetTrigger(JawAttackAnimationID);

		public float GetJawAttackAnimationLength() => 
			GetClipLength(JawAttackAnimationName);

		private void PlayOnTakeDamageAnimation(int health) => 
			_animator.SetTrigger(TakeDamageAnimationID);

		public float GetTakeDamageAnimationLength() => 
			GetClipLength(TakeDamageAnimationName);

		private void PlayDieAnimation() => 
			_animator.SetTrigger(DieAnimationID);

		private void ChangeIdleVariation() => 
			_animator.SetFloat(IdleAnimationID, SecondIdleVariationValue);


		private void Subscribe()
		{
			_dragonHealth.OnHealthChanged += PlayOnTakeDamageAnimation;
			_dragonHealth.OnHalfHealthLost += ChangeIdleVariation;
			_dragonHealth.OnDeath += PlayDieAnimation;
		}

		private void GetAnimationClips() => 
			_animationClips = _animator.runtimeAnimatorController.animationClips.ToList();

		private float GetClipLength(string clipName)
		{
			AnimationClip current = _animationClips.FirstOrDefault(clip => clip.name == clipName);

			if (current == null)
				return 0.0f;

			return current.length;
		}

		private void Unsubscribe()
		{
			_dragonHealth.OnHealthChanged -= PlayOnTakeDamageAnimation;
			_dragonHealth.OnHalfHealthLost -= ChangeIdleVariation;
			_dragonHealth.OnDeath -= PlayDieAnimation;
		}
	}
}

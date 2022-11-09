using System;
using System.Collections;
using Code.Audio;
using Code.Audio.EnumTypes;
using Code.Gameplay.DamageSystem;
using Code.Infrastructure;
using Code.Infrastructure.Services;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

namespace Code.Gameplay.Player
{
	public class PlayerAnimator : MonoBehaviour
	{
		private readonly int SpeedAnimationID = Animator.StringToHash("Speed");
		private readonly int GroundedAnimationID = Animator.StringToHash("Grounded");
		private readonly int JumpAnimationID = Animator.StringToHash("Jump");
		private readonly int FreeFallAnimationID = Animator.StringToHash("FreeFall");
		private readonly int MotionSpeedAnimationID = Animator.StringToHash("MotionSpeed");
		private readonly int DieAnimationID = Animator.StringToHash("Die");
		
		private const int AimingAnimationLayerIndex = 1;

		[SerializeField] private Rig aimingRig;
		[SerializeField] private float aimingAnimationTransitionSpeed = 5.0f;
		
		private Animator _animator;
		private CharacterController _controller;
		private AudioService _audioService;
		private LivingEntity _health;

		public float AimingAnimationTransitionSpeed => aimingAnimationTransitionSpeed;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_controller = GetComponent<CharacterController>();
			_health = GetComponent<PlayerHealth>();
			
			Subscribe();
		}

		private void Start() => 
			_audioService = ServiceLocator.Instance.AudioService;

		private void OnDisable() => 
			Unsubscribe();

		private void Subscribe() => 
			_health.OnDeath += PlayDieAnimation;

		private void PlayDieAnimation() => 
			_animator.SetTrigger(DieAnimationID);

		public void SetGrounded(bool value) =>
			_animator.SetBool(GroundedAnimationID, value);

		public void SetJump(bool value) =>
			_animator.SetBool(JumpAnimationID, value);

		public void SetFreeFall(bool value) =>
			_animator.SetBool(FreeFallAnimationID, value);

		public void SetSpeed(float value) =>
			_animator.SetFloat(SpeedAnimationID, value);

		public void SetMotionSpeed(float value) =>
			_animator.SetFloat(MotionSpeedAnimationID, value);

		public void SetAimingAnimationLayerWeight(float value) =>
			_animator.SetLayerWeight(AimingAnimationLayerIndex, value);

		public void SetAimingRigWeight(float value) =>
			aimingRig.weight = value;

		public float GetCurrentAimingLayerWeight() =>
			_animator.GetLayerWeight(AimingAnimationLayerIndex);

		private void Unsubscribe() => 
			_health.OnDeath -= PlayDieAnimation;

		// NOTE(vlad): Animation Event
		private void OnFootstep(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight > 0.5f)
				_audioService.PlaySound(SoundType.Footstep, transform.TransformPoint(_controller.center), 0.3f);
		}

		// NOTE(vlad): Animation Event
		private void OnLand(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight > 0.5f)
				_audioService.PlaySound(SoundType.OnLand, transform.TransformPoint(_controller.center), 0.3f);
		}
	}
}
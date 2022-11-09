using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Player.Movement
{
	public class PlayerJump : MonoBehaviour
	{
		[SerializeField] private float jumpHeight;
		[SerializeField] private float gravity;
		[SerializeField] private float jumpTimeout;
		[SerializeField] private float fallTimeout;
		
		[SerializeField] private float groundedOffset;
		[SerializeField] private float groundedRadius;

		[SerializeField] private LayerMask groundLayers;

		private bool _grounded = true;
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		private PlayerAnimator _animator;
		private InputService _input;
		private PlayerController _playerController;

		public bool Grounded => _grounded;
		
		private void Awake()
		{
			_playerController = GetComponent<PlayerController>();
			_animator = GetComponent<PlayerAnimator>();
		}

		private void Start() => 
			_input = ServiceLocator.Instance.InputService;

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
		}
		
		private void GroundedCheck()
		{
			_grounded = IsGrounded();
			_animator.SetGrounded(_grounded);
		}

		private bool IsGrounded()
		{
			var playerPosition = transform.position;
			Vector3 spherePosition = new Vector3(playerPosition.x, playerPosition.y - groundedOffset,
				playerPosition.z);
			 return Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
				QueryTriggerInteraction.Ignore);
		}

		private void JumpAndGravity()
		{
			if (_grounded)
			{
				ResetGrounded();
				ClampVelocity();
				HandleJump();
				SetJumpTimeout();
			}
			else
			{
				ResetJumpTimeout();
				_input.IsJumped = false;
			}
			
			ApplyGravity();
		}

		private void ResetJumpTimeout()
		{
			_jumpTimeoutDelta = jumpTimeout;
			if (_fallTimeoutDelta >= 0.0f)
				_fallTimeoutDelta -= Time.deltaTime;
			else
				_animator.SetFreeFall(true);
		}

		private void SetJumpTimeout()
		{
			if (_jumpTimeoutDelta >= 0.0f) 
				_jumpTimeoutDelta -= Time.deltaTime;
		}

		private void HandleJump()
		{
			if (IsJumpPressed())
			{
				// NOTE(vlad): the square root of H * -2 * G = how much velocity needed to reach desired height
				_playerController.VerticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
				_animator.SetJump(true);
			}
		}

		private bool IsJumpPressed() => 
			_input.IsJumped && _jumpTimeoutDelta <= 0.0f;

		private void ApplyGravity()
		{
			if (_playerController.VerticalVelocity < _playerController.TerminalVelocity)
				_playerController.VerticalVelocity += gravity * Time.deltaTime;
		}

		private void ClampVelocity()
		{
			if (_playerController.VerticalVelocity < 0.0f)
				_playerController.VerticalVelocity = -2f;
		}

		private void ResetGrounded()
		{
			_fallTimeoutDelta = fallTimeout;

			_animator.SetJump(false);
			_animator.SetFreeFall(false);
		}
	}
}
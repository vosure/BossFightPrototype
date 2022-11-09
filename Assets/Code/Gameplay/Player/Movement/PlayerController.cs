using Code.Infrastructure;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Player.Movement
{
	public class PlayerController : MonoBehaviour
	{
		private const float SpeedOffset = 0.1f;

		[SerializeField] private Transform targetToFollow;

		[SerializeField] private float movementSpeed;
		[SerializeField] private float sprintSpeed;
		[SerializeField] private float rotationSmoothTime;
		[SerializeField] private float speedChangeRate;
		[SerializeField] private float terminalVelocity;

		private float _currentMovementSpeed;
		private float _animationBlend;
		private float _targetRotation;
		private float _rotationVelocity;
		private Transform _mainCamera;

		private InputService _input;

		private CharacterController _controller;
		private PlayerAim _playerAim;
		private PlayerAnimator _animator;

		public Transform TargetToFollow => targetToFollow;
		public float VerticalVelocity { get; set; }
		public float TerminalVelocity => terminalVelocity;

		private void Awake()
		{
			_playerAim = GetComponent<PlayerAim>();
			_controller = GetComponent<CharacterController>();
			_animator = GetComponent<PlayerAnimator>();

			SetTargetCamera();
		}

		private void Start()
		{
			_input = ServiceLocator.Instance.InputService;

			SetTargetCamera();
		}

		private void Update() =>
			HandleMovementAndRotation();

		private void SetTargetCamera()
		{
			if (Camera.main is not null)
				_mainCamera = Camera.main.transform;
		}

		private void HandleMovementAndRotation()
		{
			var targetSpeed = CalculateTargetValues(out var inputMagnitude);

			MovePlayer();
			RotatePlayer();

			SetAnimationValues(targetSpeed, inputMagnitude);
		}

		private float CalculateTargetValues(out float inputMagnitude)
		{
			float targetSpeed = GetTargetSpeed();
			float currentHorizontalSpeed = GetHorizontalVelocity();
			inputMagnitude = GetInputMagnitude();

			if (IsSpeedChanged(currentHorizontalSpeed, targetSpeed))
				_currentMovementSpeed = GetCurrentSpeed(currentHorizontalSpeed, targetSpeed, inputMagnitude);
			else
				_currentMovementSpeed = targetSpeed;
			return targetSpeed;
		}

		private void MovePlayer()
		{
			Vector3 targetDirection = GetMoveDirection();

			_controller.Move(targetDirection.normalized * (_currentMovementSpeed * Time.deltaTime) +
			                 new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void RotatePlayer()
		{
			if (HasInput())
				HandleMovementRotation();

			if (_input.IsAiming)
				HandleAimingRation();
		}

		private Vector3 GetMoveDirection() =>
			Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

		private void HandleAimingRation()
		{
			Vector3 aimingPoint = _playerAim.AimingWorldPoint;
			aimingPoint.y = transform.position.y;
			Vector3 aimingDirection = (aimingPoint - transform.position).normalized;

			transform.forward = Vector3.Lerp(transform.forward, aimingDirection, Time.deltaTime * 20.0f);
		}

		private void HandleMovementRotation()
		{
			Vector3 inputDirection = GetNormalizedInput();

			_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
			                  _mainCamera.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
				ref _rotationVelocity,
				rotationSmoothTime);

			if (!_input.IsAiming)
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}

		private bool HasInput() =>
			_input.MoveDirection != Vector2.zero;

		private Vector3 GetNormalizedInput() =>
			new Vector3(_input.MoveDirection.x, 0.0f, _input.MoveDirection.y).normalized;

		private float GetSpeedAnimationBlend(float targetSpeed)
		{
			var blendValue = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

			return blendValue < 0.01f ? 0f : blendValue;
		}

		private float GetCurrentSpeed(float currentHorizontalSpeed, float targetSpeed, float inputMagnitude)
		{
			float speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
				Time.deltaTime * speedChangeRate);
			return Mathf.Round(speed * 1000f) / 1000f;
		}

		private static bool IsSpeedChanged(float currentHorizontalSpeed, float targetSpeed)
		{
			return currentHorizontalSpeed < targetSpeed - SpeedOffset ||
			       currentHorizontalSpeed > targetSpeed + SpeedOffset;
		}

		private float GetInputMagnitude() =>
			_input.AnalogMovement ? _input.MoveDirection.magnitude : 1f;

		private float GetHorizontalVelocity() =>
			new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

		private float GetTargetSpeed()
		{
			if (_input.MoveDirection == Vector2.zero)
				return 0.0f;

			return _input.IsSprinting ? sprintSpeed : movementSpeed;
		}

		private void SetAnimationValues(float targetSpeed, float inputMagnitude)
		{
			_animationBlend = GetSpeedAnimationBlend(targetSpeed);
			_animator.SetSpeed(_animationBlend);
			_animator.SetMotionSpeed(inputMagnitude);
		}
	}
}
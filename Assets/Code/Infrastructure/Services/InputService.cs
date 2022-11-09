using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Infrastructure.Services
{
	public class InputService : MonoBehaviour
	{
		private const string KeyboardDeviceName = "KeyboardMouse";

		[SerializeField] private bool analogMovement;

		[SerializeField] private bool cursorLocked = true;
		[SerializeField] private bool cursorInputForLook = true;

		public Vector2 MoveDirection { get; private set; }
		public Vector2 LookDirection { get; private set; }
		public bool IsJumped { get; set; }
		public bool IsSprinting { get; private set; }
		public bool IsAiming { get; private set; }
		private bool IsShot { get; set; }

		public event Action OnAimStarted;
		public event Action OnAimEnded;

		public event Action OnTriggerHold;
		public event Action OnTriggerReleased;

		public event Action OnReloaded;

		public bool AnalogMovement => analogMovement;

		private PlayerInput _playerInput;

		public bool IsCurrentDeviceMouse() => _playerInput.currentControlScheme == KeyboardDeviceName;

		private bool _isDisabled = false;

		private void Awake() =>
			_playerInput = GetComponent<PlayerInput>();

		public void EnableInput()
		{
			_playerInput.ActivateInput();
			_isDisabled = false;
			SetCursorState(true);
		}

		public void DisableInput()
		{
			_playerInput.DeactivateInput();
			_isDisabled = true;
			SetCursorState(false);
		}

		public void OnMove(InputValue value) =>
			HandleMoveInput(value.Get<Vector2>());

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
				HandleLookInput(value.Get<Vector2>());
		}

		public void OnJump(InputValue value) =>
			HandleJumpInput(value.isPressed);

		public void OnSprint(InputValue value) =>
			HandleSprintInput(value.isPressed);

		public void OnAim(InputValue value) =>
			HandleAimInput(value.isPressed);

		public void OnShoot(InputValue value) =>
			HandleShootInput(value.isPressed);

		public void OnReload(InputValue value) =>
			HandleReloadInput(value.isPressed);


		private void HandleMoveInput(Vector2 newMoveDirection) =>
			MoveDirection = newMoveDirection;

		private void HandleLookInput(Vector2 newLookDirection) =>
			LookDirection = newLookDirection;

		private void HandleJumpInput(bool newJumpState) =>
			IsJumped = newJumpState;

		private void HandleSprintInput(bool newSprintState) =>
			IsSprinting = newSprintState;

		private void HandleAimInput(bool newAimState)
		{
			IsAiming = newAimState;

			if (IsAiming)
				OnAimStarted?.Invoke();
			else
				OnAimEnded?.Invoke();
		}

		private void HandleShootInput(bool newShotState)
		{
			IsShot = newShotState;

			if (IsShot)
				OnTriggerHold?.Invoke();
			else
				OnTriggerReleased?.Invoke();
		}

		private void HandleReloadInput(bool newReloadState)
		{
			OnReloaded?.Invoke();
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if (_isDisabled)
				return;
				
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState) =>
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Player.Movement
{
	public class PlayerCamera : MonoBehaviour
	{
		private const float Threshold = 0.01f;

		[SerializeField] private Transform cameraRoot;

		[SerializeField] private float topClamp;
		[SerializeField] private float bottomClamp;
		[SerializeField] private float cameraAngleOverride;
		[SerializeField] bool lockCameraPosition;

		private float _cameraTargetYaw;
		private float _cameraTargetPitch;

		private InputService _input;

		private void Start()
		{
			_input = ServiceLocator.Instance.InputService;

			Initialize();
		}

		private void Initialize() => 
			_cameraTargetYaw = cameraRoot.rotation.eulerAngles.y;

		private void LateUpdate() =>
			UpdateCameraRotation();

		private void UpdateCameraRotation()
		{
			if (IsInputPerformed())
				UpdateCameraAngles();

			ClampCameraAngles();
			SetCameraRotation();
		}

		private void SetCameraRotation()
		{
			cameraRoot.rotation = Quaternion.Euler(_cameraTargetPitch + cameraAngleOverride,
				_cameraTargetYaw, 0.0f);
		}

		private void ClampCameraAngles()
		{
			_cameraTargetYaw = ClampCameraAngle(_cameraTargetYaw, float.MinValue, float.MaxValue);
			_cameraTargetPitch = ClampCameraAngle(_cameraTargetPitch, bottomClamp, topClamp);
		}

		private void UpdateCameraAngles()
		{
			float deltaTimeMultiplier = _input.IsCurrentDeviceMouse() ? 1.0f : Time.deltaTime;

			_cameraTargetYaw += _input.LookDirection.x * deltaTimeMultiplier;
			_cameraTargetPitch += _input.LookDirection.y * deltaTimeMultiplier;
		}

		private static float ClampCameraAngle(float angle, float min, float max)
		{
			if (angle < -360f) angle += 360f;
			if (angle > 360f) angle -= 360f;
			return Mathf.Clamp(angle, min, max);
		}

		private bool IsInputPerformed() =>
			_input.LookDirection.sqrMagnitude >= Threshold && !lockCameraPosition;
	}
}
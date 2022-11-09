using Code.Infrastructure;
using Code.Infrastructure.Services;
using UnityEngine;

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	public class ThirdPersonController : MonoBehaviour
	{
		public GameObject CinemachineCameraTarget;
		public float TopClamp = 70.0f;

		public float BottomClamp = -30.0f;

		public float CameraAngleOverride = 0.0f;

		public bool LockCameraPosition = false;
		
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		private InputService _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private void Awake()
		{
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

			_input = ServiceLocator.Instance.InputService;

		}
		
		private void LateUpdate()
		{
			CameraRotation();
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (_input.LookDirection.sqrMagnitude >= _threshold && !LockCameraPosition)
			{
				//Don't multiply mouse input by Time.deltaTime;
				float deltaTimeMultiplier = _input.IsCurrentDeviceMouse() ? 1.0f : Time.deltaTime;

				_cinemachineTargetYaw += _input.LookDirection.x * deltaTimeMultiplier;
				_cinemachineTargetPitch += _input.LookDirection.y * deltaTimeMultiplier;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
				_cinemachineTargetYaw, 0.0f);
		}

		
		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

	}
}
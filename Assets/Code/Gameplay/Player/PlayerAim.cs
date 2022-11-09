using Code.Infrastructure.Services;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Code.Gameplay.Player
{
	public class PlayerAim : MonoBehaviour
	{
		private const float AimingSpeed = 20.0f;

		[SerializeField] private LayerMask aimLayerMask;
		[SerializeField] private LayerMask shootLayerMask;
		[SerializeField] private Transform aimingTarget;

		private InputService _input;
		private PlayerAnimator _playerAnimator;

		private Camera _mainCamera; 
		
		public Vector3 AimingWorldPoint { get; private set; }

		private void Awake()
		{
			_playerAnimator = GetComponent<PlayerAnimator>();
			_mainCamera = Camera.main;
		}

		private void Start() =>
			_input = ServiceLocator.Instance.InputService;

		private void Update()
		{
			HandleAimingAnimationSwitch();
			UpdateAimingPoint();
		}

		private void HandleAimingAnimationSwitch()
		{
			float currentAnimationLayerWeight = _playerAnimator.GetCurrentAimingLayerWeight();
			float targetWeight = _input.IsAiming ? 1.0f : 0.0f;

			currentAnimationLayerWeight = Mathf.Lerp(currentAnimationLayerWeight, targetWeight,
				Time.deltaTime * _playerAnimator.AimingAnimationTransitionSpeed);

			_playerAnimator.SetAimingAnimationLayerWeight(currentAnimationLayerWeight);
			_playerAnimator.SetAimingRigWeight(targetWeight);
		}

		private void UpdateAimingPoint()
		{
			AimingWorldPoint = Vector3.Lerp(AimingWorldPoint, GetMousePosition(), Time.deltaTime * AimingSpeed);
			aimingTarget.transform.position = AimingWorldPoint;
		}

		public Vector3 GetMousePosition()
		{
			if (Physics.Raycast(GetAimingRay(), out RaycastHit hit, float.MaxValue, aimLayerMask | shootLayerMask))
				return hit.point;

			return Vector3.zero;
		}

		public bool TryGetHitTarget(out RaycastHit hit)
		{
			if (Physics.Raycast(GetAimingRay(), out hit, float.MaxValue, shootLayerMask))
				return true;
			
			return false;
		}

		private Ray GetAimingRay()
		{
			Vector2 screenCenterPoint = GetScreenCenterPoint();
			return _mainCamera.ScreenPointToRay(screenCenterPoint);
		}

		private static Vector2 GetScreenCenterPoint() =>
			new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
	}
}
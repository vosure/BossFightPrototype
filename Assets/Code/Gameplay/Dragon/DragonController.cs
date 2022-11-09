using Code.Gameplay.Player.Movement;
using UnityEngine;

namespace Code.Gameplay.Dragon
{
	public class DragonController : MonoBehaviour
	{
		[SerializeField] private float rotationSpeed;
		public Transform PlayerTransform { get; private set; }

		private void Start() =>
			GetTargetTransform();

		private void LateUpdate() =>
			RotateToTarget();

		private void RotateToTarget()
		{
			if (PlayerTransform != null)
			{
				var targetRotation = GetTargetRotation();
				if (CheckMinAngle(targetRotation))
					RotateTowardsWithDelta(targetRotation, Time.deltaTime * rotationSpeed);
			}
		}

		private void RotateTowardsWithDelta(Quaternion targetRotation, float delta)
		{
			transform.rotation =
				Quaternion.RotateTowards(transform.rotation, targetRotation, delta);
		}

		private Quaternion GetTargetRotation()
		{
			Vector3 targetDirection = (PlayerTransform.position - transform.position).normalized;
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
			return targetRotation;
		}

		private void GetTargetTransform() =>
			PlayerTransform = FindObjectOfType<PlayerController>().TargetToFollow;

		private bool CheckMinAngle(Quaternion targetRotation) =>
			Quaternion.Angle(transform.rotation, targetRotation) > 0.01f;
	}
}
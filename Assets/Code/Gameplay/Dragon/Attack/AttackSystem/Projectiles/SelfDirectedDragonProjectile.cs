using Code.Gameplay.Player;
using UnityEngine;

namespace Code.Gameplay.Dragon.Attack.AttackSystem.Projectiles
{
	public class SelfDirectedDragonProjectile : MonoBehaviour
	{
		[SerializeField] private float movementSpeed = 2.5f;
		[SerializeField] private int damage = 10;

		private Transform _followTarget;

		public void SetTarget(Transform target) => 
			_followTarget = target;

		private void Update()
		{
			if (_followTarget != null) 
				Move();
		}

		private void Move() => 
			transform.position = GetTargetPosition();

		private Vector3 GetTargetPosition()
		{
			return Vector3.Lerp(transform.position, _followTarget.position,
				Time.deltaTime * movementSpeed);
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
			{
				playerHealth.TakeDamage(damage);
				Destroy(gameObject);
			}
		}
	}
}
using Code.Audio.EnumTypes;
using Code.Infrastructure;
using UnityEngine;

namespace Code.Gameplay.WeaponSystem
{
	public class Projectile : MonoBehaviour
	{
		private const float MovementSpeed = 50.0f;
		private void Update() => 
			transform.Translate(Vector3.forward * (Time.deltaTime * MovementSpeed));

		private void OnTriggerEnter(Collider other) => 
			Destroy(gameObject);
	}
}
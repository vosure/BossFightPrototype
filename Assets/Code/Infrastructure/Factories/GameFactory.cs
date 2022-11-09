using Code.Core.Reward;
using Code.Gameplay.Dragon.Attack.AttackSystem.Projectiles;
using Code.Gameplay.WeaponSystem;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
	// TODO(vlad): Load From Resources
	public class GameFactory : MonoBehaviour
	{
		[SerializeField] private Projectile projectilePrefab;
		[SerializeField] private SelfDirectedDragonProjectile selfDirectedProjectilePrefab;
		[SerializeField] private FastFireball fastFireballPrefab;
		[SerializeField] private Chest chestPrefab;

		public Projectile SpawnBulletProjectile(Vector3 position, Quaternion rotation) => 
			Instantiate(projectilePrefab, position, rotation);
		
		public SelfDirectedDragonProjectile SpawnSelfDirectedFireballProjectile(Vector3 position, Quaternion rotation) => 
			Instantiate(selfDirectedProjectilePrefab, position, rotation);
		
		public FastFireball SpawnFastFireball(Vector3 position, Quaternion rotation) => 
			Instantiate(fastFireballPrefab, position, rotation);

		public Chest SpawnChest(Vector3 position, Quaternion rotation) => 
			Instantiate(chestPrefab, position, rotation);
	}
}
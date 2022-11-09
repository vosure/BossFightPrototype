using System.Linq;
using Code.Infrastructure.Data.Vfx;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
	public class VFXFactory : MonoBehaviour
	{
		[SerializeField] private VfxData[] vfxDatas; // TODO(vlad): Load From Resources

		public void SpawnVfx(VFXType type, Vector3 position, Quaternion rotation, Transform parent)
		{
			ParticleSystem ps = Instantiate(Get(type), position, rotation, parent);
			ps.Play();
		}

		public Transform SpawnVfx(VFXType type, Vector3 position, Quaternion rotation)
		{
			ParticleSystem ps = Instantiate(Get(type), position, rotation);
			ps.Play();
			return ps.transform;
		}

		public void SpawnVfx(VFXType type, Transform targetTransform) => 
			SpawnVfx(type, targetTransform.position, targetTransform.rotation, targetTransform);

		private ParticleSystem Get(VFXType type) =>
			vfxDatas.First(x => x.VfxType == type).Vfx;
	}
}
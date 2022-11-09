using UnityEngine;

namespace Code.Utility
{
	public class DestroyAfterTime : MonoBehaviour
	{
		[SerializeField] private float timeToDestroy;

		private void Awake() => 
			Destroy(gameObject, timeToDestroy);
	}
}
using UnityEngine;

namespace Code.Utility
{
	public static class Vector3Extensions
	{
		public static Vector3 AddRandomDistortion(this Vector3 vector, float x, float y, float z)
		{
			float targetX = Random.Range(-x, x);
			float targetY = Random.Range(-y, y);
			float targetZ = Random.Range(-z, z);

			return new Vector3(vector.x + targetX, vector.y + targetY, vector.z + targetZ);
		}
	}
}
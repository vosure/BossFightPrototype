using UnityEngine;

namespace Code.Utility
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		[SerializeField] private bool dontDestroy;

		protected static T instance;

		public static bool HasInstance() => instance != null;
		
		public static T Instance
		{
			get
			{
				if (!instance )
				{
					instance = FindObjectOfType<T>();

					if (!instance)
					{
						GameObject singleton = new GameObject(typeof(T).Name);
						instance = singleton.AddComponent<T>();
					}
				}

				return instance;
			}
		}

		protected virtual void Awake()
		{
			if (!instance)
			{
				instance = this as T;

				if (dontDestroy)
				{
					transform.parent = null;
					DontDestroyOnLoad(gameObject);
				}
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
		}
	}
}
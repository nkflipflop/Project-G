using UnityEngine;

namespace Pooling.Interfaces
{
	public interface IPoolable
	{
		ObjectType Type { get; set; }
		GameObject GameObject { get; set; }

		void Initialize(GameObject gameObject)
		{
			GameObject = gameObject;
		}
	}
	
	public static class IPoolableExtensions
	{
		public static bool ResetObject(this IPoolable poolable)
		{
			return PoolFactory.instance.ResetObject(poolable);
		}

		public static GameObject GameObject(this IPoolable poolable)
		{
			return ((MonoBehaviour)poolable).gameObject;
		}
	}
}
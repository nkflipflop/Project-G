using UnityEngine;

namespace Pooling.Interfaces
{
	public interface IPoolable
	{
		ObjectType Type { get; set; }
		GameObject GameObject => ((MonoBehaviour)this).gameObject;

		void OnSpawn();
		void OnReset();
	}
	
	public static class IPoolableExtensions
	{
		public static bool ResetObject(this IPoolable poolable, bool removeFromActiveObjects = true)
		{
			return PoolFactory.instance.ResetObject(poolable, removeFromActiveObjects);
		}
	}
}
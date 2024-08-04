using UnityEngine;

namespace LootSystem
{
	[CreateAssetMenu(fileName = "PoolObjectLootDropInfo", menuName = "Gameplay/Objects/PoolObjectLootDropInfo", order = 0)]
	public class PoolObjectLootDropInfo : ScriptableObject
	{
		public GenericLootDropTablePoolObject dropTable;
		public int numItemsToDrop;
	}
	
		
	[System.Serializable]
	public class GenericLootDropTablePoolObject : GenericLootDropTable<GenericLootDropItemPoolObject, PoolObjectItem>
	{
	}
}
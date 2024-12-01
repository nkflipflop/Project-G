using Pooling;
using UnityEngine;

namespace Gameplay.Runtime.Dungeon
{
	[CreateAssetMenu(fileName = "DungeonConfig", menuName = "Gameplay/Dungeon/Config", order = 0)]
	public class DungeonConfig : SingletonScriptableObject<DungeonConfig>
	{
		public int dungeonPadding;
		public int dungeonRows, dungeonColumns;
		public int minRoomSize, maxRoomSize;

		public ObjectType[] bridgeTilesMapping;
		public ObjectType[] wallTilesMapping;
	}
}
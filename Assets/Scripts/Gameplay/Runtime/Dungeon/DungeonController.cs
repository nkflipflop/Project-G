using System;
using System.Collections.Generic;
using Pooling;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Gameplay.Runtime.Dungeon
{
	[Serializable]
	public partial class DungeonController
	{
		private struct SpawnData
		{
			public SpawnData(int max, int current)
			{
				Max = max;
				Current = current;
			}

			public int Max { get; }
			public int Current { get; set; }
		}
		
		private GameObject[,] dungeonFloorPositions;
		private int[,] objectSpawnPos;
		private int[,] enemyIndexes;
		private List<Vector2Int> bridgeTilesPos;
		private SpawnData enemySpawnData = new(25, 0);
		private SpawnData trapSpawnData = new(12, 0);
		private Vector3 randomPos;
		private SubDungeon rootSubDungeon;
		private readonly Vector3 invalidPos = new(0, 0, 1);

		public int[,] DungeonMap { get; private set; }
		
		public void CreateDungeon()
		{
			rootSubDungeon = new SubDungeon(new Rect(DungeonConfig.instance.dungeonPadding,
				DungeonConfig.instance.dungeonPadding, DungeonConfig.instance.dungeonRows,
				DungeonConfig.instance.dungeonColumns));
			CreateBSP(rootSubDungeon);
			rootSubDungeon.CreateRoom();

			dungeonFloorPositions =
				new GameObject[DungeonConfig.instance.dungeonRows + (2 * DungeonConfig.instance.dungeonPadding),
					DungeonConfig.instance.dungeonColumns + (2 * DungeonConfig.instance.dungeonPadding)];
			DungeonMap = new int[DungeonConfig.instance.dungeonRows + (2 * DungeonConfig.instance.dungeonPadding),
				DungeonConfig.instance.dungeonColumns + (2 * DungeonConfig.instance.dungeonPadding)];
			objectSpawnPos = new int[DungeonConfig.instance.dungeonRows + (2 * DungeonConfig.instance.dungeonPadding),
				DungeonConfig.instance.dungeonColumns + (2 * DungeonConfig.instance.dungeonPadding)];
			bridgeTilesPos = new List<Vector2Int>();

			DrawRooms(rootSubDungeon);
			DrawCorridors(rootSubDungeon);
			DetermineBridges(rootSubDungeon);
			DrawBridges();
			DrawWaters();
			bridgeTilesPos.Clear(); // deleting the list since it completes its purpose
			DrawWalls();
			PlaceLamps(DungeonMap);
			enemyIndexes = new[,]
			{
				{ 0, 1 }, { 0, 2 }, { 1, 2 }, { 0, 3 }, { 1, 3 }, { 1, 4 }, { 1, 5 }
			}; // start and end indexes of Enemies array according to the dungeon level
		}

		private void CreateBSP(SubDungeon subDungeon)
		{
			//Debug.Log("Splitting sub-dungeon " + subDungeon.debugId + ": " + subDungeon.rect);
			if (subDungeon.IAmLeaf())
			{
				// if the subdungeon is too large
				if (subDungeon.rect.width > DungeonConfig.instance.maxRoomSize ||
				    subDungeon.rect.height > DungeonConfig.instance.maxRoomSize ||
				    Random.Range(0.0f, 1.0f) > 0.25)
				{
					if (subDungeon.Split(DungeonConfig.instance.minRoomSize, DungeonConfig.instance.maxRoomSize))
					{
						//Debug.Log ("Splitted sub-dungeon " + subDungeon.debugId + " in " + subDungeon.left.debugId + ": " + subDungeon.left.rect + ", "
						//+ subDungeon.right.debugId + ": " + subDungeon.right.rect);

						CreateBSP(subDungeon.left);
						CreateBSP(subDungeon.right);
					}
				}
			}
		}

		private void DrawRooms(SubDungeon subDungeon)
		{
			if (subDungeon == null)
			{
				return;
			}

			if (subDungeon.IAmLeaf())
			{
				for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
				{
					for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
					{
						if (!(i >= subDungeon.removedPiece.x && i <= subDungeon.removedPiece.xMax &&
						      j >= subDungeon.removedPiece.y && j <= subDungeon.removedPiece.yMax))
						{
							PoolObject floorObj = PoolFactory.instance.GetObject<PoolObject>(
								(ObjectType)Random.Range((int)ObjectType.DungeonFloor1,
									(int)ObjectType.DungeonFloor5 + 1), position: new Vector3(i, j, 0f));
							dungeonFloorPositions[i, j] = floorObj.gameObject;
							DungeonMap[i, j] = 1;
						}
					}
				}
			}
			else
			{
				DrawRooms(subDungeon.left);
				DrawRooms(subDungeon.right);
			}
		}

		private void DrawCorridors(SubDungeon subDungeon)
		{
			if (subDungeon == null)
				return;

			DrawCorridors(subDungeon.left);
			DrawCorridors(subDungeon.right);

			foreach (Rect corridor in subDungeon.corridors)
			{
				for (int i = (int)corridor.x; i < corridor.xMax; i++)
				{
					for (int j = (int)corridor.y; j < corridor.yMax; j++)
					{
						if (dungeonFloorPositions[i, j] == null)
						{
							PoolObject floorObj = PoolFactory.instance.GetObject<PoolObject>(
								(ObjectType)Random.Range((int)ObjectType.DungeonFloor1,
									(int)ObjectType.DungeonFloor5 + 1), position: new Vector3(i, j, 0f));
							dungeonFloorPositions[i, j] = floorObj.gameObject;
							DungeonMap[i, j] = 1;
						}
					}
				}
			}
		}

		private void DetermineBridges(SubDungeon subDungeon)
		{
			if (subDungeon == null)
			{
				return;
			}

			DetermineBridges(subDungeon.left);
			DetermineBridges(subDungeon.right);

			foreach (Rect bridge in subDungeon.bridges)
			{
				for (int i = (int)bridge.x; i < bridge.xMax; i++)
				{
					for (int j = (int)bridge.y; j < bridge.yMax; j++)
					{
						if (dungeonFloorPositions[i, j] == null)
						{
							bridgeTilesPos.Add(new Vector2Int(i, j));
							DungeonMap[i, j] = -1;
						}
					}
				}
			}
		}

		private void DrawBridges()
		{
			int[,] kernelMatrix = { { 0, 1, 0 }, { 8, 0, 2 }, { 0, 4, 0 } };

			foreach (var bridgePos in bridgeTilesPos)
			{
				int index = 0;
				for (int j = 1; j >= -1; j--)
				{
					for (int i = -1; i <= 1; i++)
					{
						index += Mathf.Abs(DungeonMap[bridgePos.x + i, bridgePos.y + j]) * kernelMatrix[1 - j, i + 1];
					}
				}

				if (dungeonFloorPositions[bridgePos.x, bridgePos.y] == null)
				{
					PoolObject instance = PoolFactory.instance.GetObject<PoolObject>(
						DungeonConfig.instance.bridgeTilesMapping[index],
						position: new Vector3(bridgePos.x, bridgePos.y, 0f));
					dungeonFloorPositions[bridgePos.x, bridgePos.y] = instance.gameObject;
				}
			}
		}

		private void DrawWalls()
		{
			int matrixSize = 3;

			int[,] kernelMatrix = { { 4, 64, 2 }, { 128, 0, 32 }, { 8, 16, 1 } };

			for (int j = DungeonConfig.instance.dungeonColumns + (2 * DungeonConfig.instance.dungeonPadding) -
			             matrixSize;
			     j >= 0;
			     j--)
			{
				for (int i = 0;
				     i <= DungeonConfig.instance.dungeonRows + (2 * DungeonConfig.instance.dungeonPadding) - matrixSize;
				     i++)
				{
					int index = 0;
					for (int l = 0; l < matrixSize; l++)
					{
						for (int k = 0; k < matrixSize; k++)
						{
							index += Mathf.Abs(DungeonMap[i + k, j + l]) * kernelMatrix[l, k];
						}
					}

					int wallPosX = i + 1, wallPosY = j + 1;
					if (dungeonFloorPositions[wallPosX, wallPosY] == null && DungeonMap[wallPosX, wallPosY] == 0)
					{
						PoolObject instance = PoolFactory.instance.GetObject<PoolObject>(
							DungeonConfig.instance.wallTilesMapping[index],
							position: new Vector3(wallPosX, wallPosY, 0f));
						dungeonFloorPositions[wallPosX, wallPosY] = instance.gameObject;

						if (index != 0)
						{
							// placing floor tile under the walls
							PoolFactory.instance.GetObject<PoolObject>(
								(ObjectType)Random.Range((int)ObjectType.DungeonFloor1,
									(int)ObjectType.DungeonFloor5 + 1), position: new Vector3(wallPosX, wallPosY, 0f));
						}
					}
				}
			}
		}

		private void DrawWaters()
		{
			bridgeTilesPos.SortBy((tile1, tile2) => tile2.y.CompareTo(tile1.y));
			foreach (var bridgePos in bridgeTilesPos)
			{
				for (int j = 1; j >= -1; j--)
				{
					for (int i = -1; i <= 1; i++)
					{
						if (dungeonFloorPositions[bridgePos.x + i, bridgePos.y + j] == null || (i == 0 && j == 0))
						{
							ObjectType waterTileType = DungeonMap[bridgePos.x + i, bridgePos.y + j + 1] != -1
								? ObjectType.WaterTile1
								: ObjectType.WaterTile2;
							PoolObject waterTileObj = PoolFactory.instance.GetObject<PoolObject>(waterTileType,
								position: new Vector3(bridgePos.x + i, bridgePos.y + j, 0f));
							if (i == 0 && j == 0)
							{
								waterTileObj.gameObject.GetComponent<BoxCollider2D>().enabled = false;
							}

							dungeonFloorPositions[bridgePos.x + i, bridgePos.y + j] = waterTileObj.gameObject;
							DungeonMap[bridgePos.x + i, bridgePos.y + j] = -1;
						}
					}
				}
			}

			foreach (Vector2Int bridgePos in bridgeTilesPos)
			{
				DungeonMap[bridgePos.x, bridgePos.y] = 1;
			}
		}

		private void PlaceLamps(int[,] dungeonTiles)
		{
			int[,] lightTiles = dungeonTiles.Clone() as int[,];
			int matrixSize = 3;
			int[,] kernelMatrix = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };

			for (int j = DungeonConfig.instance.dungeonColumns + (2 * DungeonConfig.instance.dungeonPadding) -
			             matrixSize;
			     j >= 0;
			     j--)
			{
				for (int i = 0;
				     i <= DungeonConfig.instance.dungeonRows + (2 * DungeonConfig.instance.dungeonPadding) - matrixSize;
				     i++)
				{
					int mulResult = 0;
					for (int l = 0; l < matrixSize; l++)
					{
						for (int k = 0; k < matrixSize; k++)
						{
							mulResult += Mathf.Abs(lightTiles[i + k, j + l]) * kernelMatrix[l, k];
						}
					}

					if (mulResult >= 6)
					{
						PoolFactory.instance.GetObject(ObjectType.Lamp, position: new Vector3(i + 1, j + 1, 0f));

						for (int l = 0; l < matrixSize; l++)
						{
							for (int k = 0; k < matrixSize; k++)
							{
								lightTiles[i + k, j + l] = 0;
							}
						}
					}
				}
			}
		}
	}
}
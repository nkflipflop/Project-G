using Gameplay.Runtime.Controllers;
using LootSystem;
using Pooling;
using UnityEngine;
using Utilities;

namespace Gameplay.Runtime.Dungeon
{
	public partial class DungeonController
	{
		public void SpawnEverything(int dungeonLevel)
		{
			SpawnPlayer();
			SpawnEnemies(dungeonLevel);
			SpawnTraps(dungeonLevel);
			SpawnChests(dungeonLevel);

			objectSpawnPos = null; // after the spawning everything, set it to null
		}

		private void SpawnPlayer()
		{
			GetRandomPos(rootSubDungeon); // getting random position in the dungeon for the player
			GameManager.instance.Player.transform.position = randomPos;
			objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
			GameManager.instance.Player.gameObject.SetActive(true);

			GetRandomPos(rootSubDungeon); // getting random position in the dungeon for the exit
			PoolFactory.instance.GetObject<EscapeDoor>(ObjectType.EscapeDoor,
				position: new Vector3(randomPos.x, randomPos.y, 0f));
			objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;

			GetRandomPos(rootSubDungeon); // getting random position in the dungeon for the object
			PoolFactory.instance.GetObject<PoolObject>(ObjectType.Key,
				position: new Vector3(randomPos.x, randomPos.y, 0f));
			objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
		}

		private void SpawnChests(int dungeonLevel)
		{
			// spawning barrels
			for (int i = 0; i < 3; i++)
			{
				GetRandomPos(rootSubDungeon); // getting random position in the dungeon
				PoolFactory.instance.GetObject<Chest>(ObjectType.Barrel,
					position: new Vector3(randomPos.x, randomPos.y, 0f));
				objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
			}

			//spawning weapon chests if the level is the multiple of 2
			if (dungeonLevel % 2 == 1)
			{
				GetRandomPos(rootSubDungeon); // getting random position in the dungeon
				PoolFactory.instance.GetObject<Chest>(ObjectType.WeaponCrate,
					position: new Vector3(randomPos.x, randomPos.y, 0f));
				objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
			}
		}

		private void SpawnEnemies(int dungeonLevel)
		{
			SpawnEnemies(rootSubDungeon, dungeonLevel);
		}

		private void SpawnEnemies(SubDungeon subDungeon, int dungeonLevel)
		{
			if (subDungeon == null)
			{
				return;
			}

			if (subDungeon.IAmLeaf())
			{
				if (enemySpawnData.Current <= enemySpawnData.Max)
				{
					int minEnemyNumber = (int)((subDungeon.room.width * subDungeon.room.height) / 8);
					int enemyNumberForThisRoom = Random.Range(minEnemyNumber, minEnemyNumber + 1);
					for (int i = 0; i < enemyNumberForThisRoom; i++)
					{
						randomPos = GetRandomPosInRoom(subDungeon.room);
						// if the randomPos != invalidPos, then spawn the object
						if (Vector3.Distance(randomPos, invalidPos) != 0)
						{
							int enemyIndex;
							do
							{
								// make sure that there is only one turret in a room
								int enemyIndexRangeMin =
									(dungeonLevel < 7) ? enemyIndexes[dungeonLevel, 0] : enemyIndexes[6, 0];
								int enemyIndexRangeMax =
									(dungeonLevel < 7) ? enemyIndexes[dungeonLevel, 1] : enemyIndexes[6, 1];
								enemyIndex = Random.Range(enemyIndexRangeMin, enemyIndexRangeMax + 1);
							} while
								(subDungeon.hasTurret &&
								 enemyIndex == 3); // check if the room has a turret and new enemy is turret

							Enemy enemy =
								PoolFactory.instance.GetObject<Enemy>((ObjectType)((int)ObjectType.Beetle + enemyIndex),
									position: randomPos);
							if (enemy is IPathfinder pathfinderEnemy)
							{
								pathfinderEnemy.SetupPathfinding(GameManager.instance.Player.transform, DungeonMap);
							}

							enemySpawnData.Current++;
							objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
							if (enemyIndex == 3)
							{
								subDungeon.hasTurret = true;
							}
						}
					}
				}
			}
			else
			{
				SpawnEnemies(subDungeon.left, dungeonLevel);
				SpawnEnemies(subDungeon.right, dungeonLevel);
			}
		}

		private void SpawnTraps(int dungeonLevel)
		{
			SpawnTraps(rootSubDungeon, dungeonLevel);
		}

		private void SpawnTraps(SubDungeon subDungeon, int dungeonLevel)
		{
			if (subDungeon == null)
				return;

			if (subDungeon.IAmLeaf())
			{
				if (trapSpawnData.Current <= trapSpawnData.Max)
				{
					int minTrapNumber = (int)((subDungeon.room.width * subDungeon.room.height) / 12);
					int trapNumberForThisRoom = Random.Range(minTrapNumber, minTrapNumber + 1);
					for (int i = 0; i < trapNumberForThisRoom; i++)
					{
						randomPos = GetRandomPosInRoom(subDungeon.room);
						// if the randomPos != invalidPos, then spawn the object
						if (Vector3.Distance(randomPos, invalidPos) != 0)
						{
							ObjectType trapType = Random.Range(0, 2) == 0 ? ObjectType.SpikeTrap : ObjectType.FireTrap;

							PoolFactory.instance.GetObject<Trap>(trapType, position: randomPos);
							trapSpawnData.Current++;
							objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
						}
					}
				}
			}
			else
			{
				SpawnTraps(subDungeon.left, dungeonLevel);
				SpawnTraps(subDungeon.right, dungeonLevel);
			}
		}

		// Utility functions
		/// <summary> Gets a random position in the whole dungeon </summary>
		private void GetRandomPos(SubDungeon subDungeon)
		{
			if (subDungeon == null)
			{
				return;
			}

			if (subDungeon.IAmLeaf())
			{
				int randPosX, randPosY, findingPosAttempt = 0, maxAttemptLimit = 500;
				do
				{
					randPosX = Random.Range((int)subDungeon.room.x, (int)subDungeon.room.xMax);
					randPosY = Random.Range((int)subDungeon.room.y, (int)subDungeon.room.yMax);
					findingPosAttempt++;
				} while ((DungeonMap[randPosX, randPosY] != 1 || objectSpawnPos[randPosX, randPosY] == 1) &&
				         findingPosAttempt <= maxAttemptLimit);

				randomPos = new Vector3(randPosX, randPosY, 0);
			}
			else
			{
				GetRandomPos(Random.Range(0, 2) == 0 ? subDungeon.left : subDungeon.right);
			}
		}

		/// <summary> Gets a random position in the given room </summary>
		private Vector3 GetRandomPosInRoom(Rect room)
		{
			int randPosX, randPosY, findingPosAttempt = 0, maxAttemptLimit = 500;
			do
			{
				randPosX = Random.Range((int)room.x, (int)room.xMax);
				randPosY = Random.Range((int)room.y, (int)room.yMax);
				findingPosAttempt++;
			} while ((DungeonMap[randPosX, randPosY] != 1 || objectSpawnPos[randPosX, randPosY] == 1) &&
			         findingPosAttempt <= maxAttemptLimit);

			if (findingPosAttempt > maxAttemptLimit)
			{
				Log.Debug("Could not find a pos in the room.");
			}

			return (findingPosAttempt <= maxAttemptLimit) ? new Vector3(randPosX, randPosY, 0) : invalidPos;
		}
	}
}
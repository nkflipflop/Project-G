using System.Collections.Generic;
using LootSystem;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;
using Utilities;

public class DungeonManager : MonoBehaviour
{
    private enum Tiles
    {
        Bridges,
        Corridors,
        Floors,
        Walls,
        Waters
    }

    private enum Objects
    {
        Enemies = 5,
        Traps,
        Lamps,
        Chests
    }

    private enum LampObjectsTypes
    {
        Lamp,
        CableH,
        LampCableH,
        CableV,
        LampCableV
    }

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

    private GameManager gameManager;
    [SerializeField] private PlayerManager player;
    [SerializeField] private GameObject dungeon;
    [SerializeField] private GameObject[] enemies;
    private GameObject[,] dungeonFloorPositions;
    private int[,] objectSpawnPos;
    private int[,] enemyIndexes;
    private EscapeDoor escapeDoor;
    private List<Vector2Int> bridgeTilesPos;
    private SpawnData enemySpawnData = new(25, 0);
    private SpawnData trapSpawnData = new(12, 0);
    private Vector3 playerSpawnPos;
    private Vector3 randomPos;
    private SubDungeon rootSubDungeon;
    private readonly Vector3 invalidPos = new(0, 0, 1);

    public int[,] DungeonMap { get; private set; }


    public class SubDungeon
    {
        public SubDungeon left, right;
        public Rect rect;
        public Rect room = new(-1, -1, 0, 0); // null
        public Rect removedPiece = new(-1, -1, 0, 0); // null
        public List<Rect> corridors = new();
        public List<Rect> bridges = new();
        public int debugId;
        public bool hasTurret;

        private static int debugCounter = 0;

        public SubDungeon(Rect mrect)
        {
            rect = mrect;
            debugId = debugCounter;
            debugCounter++;
            hasTurret = false;
        }

        public void CreateRoom()
        {
            left?.CreateRoom();
            right?.CreateRoom();
            if (left != null && right != null)
            {
                CreateCorridorBetween(left, right);
            }

            if (IAmLeaf())
            {
                int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
                int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
                int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);

                // room position will be absolute in the board, not relative to the sub-deungeon
                room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
                //Debug.Log("Created room " + room + " in sub-dungeon " + debugId + " " + rect);

                int shouldEditRoom = Random.Range(0, 4); // 25% chance for editing the shape of the room
                if (shouldEditRoom == 0)
                {
                    removedPiece = GetPieceToRemove();
                }
            }
        }

        private Rect GetPieceToRemove()
        {
            int x = 0, y = 0, xMax = 0, yMax = 0;
            int randWidth = Random.Range(1, (int)(room.width / 2));
            int randHeight = Random.Range(1, (int)(room.height / 2));
            int randCorner = Random.Range(0, 4);
            switch (randCorner)
            {
                case 0: // bottom left
                    x = (int)room.x;
                    y = (int)room.y;
                    break;
                case 1: // bottom right
                    x = (int)room.xMax - randWidth;
                    y = (int)room.y;
                    break;
                case 2: // top left
                    x = (int)room.x;
                    y = (int)room.yMax - randHeight;
                    break;
                case 3: // top right
                    x = (int)room.xMax - randWidth;
                    y = (int)room.yMax - randHeight;
                    break;
            }

            xMax = x + randWidth;
            if (xMax > room.xMax)
                xMax = (int)room.xMax;
            yMax = y + randHeight;
            if (yMax > room.yMax)
                yMax = (int)room.yMax;

            return new Rect(x, y, xMax - x, yMax - y);
        }

        private void CreateCorridorBetween(SubDungeon left, SubDungeon right)
        {
            Rect lroom = left.GetRoom();
            Rect rroom = right.GetRoom();

            //Debug.Log("Creating corridor(s) between " + left.debugId + "(" + lroom + ") and " + right.debugId + " (" + rroom + ")");

            // attach the corridor to a random point in each room
            Vector2 lpoint, rpoint;

            do
            {
                lpoint = new Vector2((int)Random.Range(lroom.x + 1, lroom.xMax - 1),
                    (int)Random.Range(lroom.y + 1, lroom.yMax - 1));
            } while (lpoint.x >= left.removedPiece.x && lpoint.x <= left.removedPiece.xMax &&
                     lpoint.y >= left.removedPiece.y && lpoint.y <= left.removedPiece.yMax);

            do
            {
                rpoint = new Vector2((int)Random.Range(rroom.x + 1, rroom.xMax - 1),
                    (int)Random.Range(rroom.y + 1, rroom.yMax - 1));
            } while (rpoint.x >= right.removedPiece.x && rpoint.x <= right.removedPiece.xMax &&
                     rpoint.y >= right.removedPiece.y && rpoint.y <= right.removedPiece.yMax);

            // always be sure that left point is on the left to simplyfy code
            if (lpoint.x > rpoint.x)
            {
                (lpoint, rpoint) = (rpoint, lpoint);
            }

            int w = (int)(lpoint.x - rpoint.x);
            int h = (int)(lpoint.y - rpoint.y);

            int thickness = Random.Range(1, 4); // getting a random thickness
            List<Rect>
                connections =
                    (thickness > 1)
                        ? corridors
                        : bridges; // if the tickness > 1, it is a corridor; otherwise, it's a bridge

            //Debug.Log("lpoint: " + lpoint + ", rpoint: " + rpoint + ", w: " + w + ", h: " + h);

            // if the points are not aligned horizontally
            if (w != 0)
            {
                if (Random.Range(0, 2) > 0)
                {
                    // add a corridor to the right
                    connections.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, thickness));

                    // if left point is below right point go up otherwise go down
                    connections.Add(h < 0
                        ? new Rect(rpoint.x, lpoint.y, thickness, Mathf.Abs(h))
                        : new Rect(rpoint.x, rpoint.y, thickness, Mathf.Abs(h)));
                }
                else
                {
                    // go up or down
                    connections.Add(h < 0
                        ? new Rect(lpoint.x, lpoint.y, thickness, Mathf.Abs(h))
                        : new Rect(lpoint.x, rpoint.y, thickness, Mathf.Abs(h)));

                    // then go right
                    connections.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, thickness));
                }
            }
            else
            {
                // if the points are aligned horizontally go up or down depending on the positions
                connections.Add(h < 0
                    ? new Rect((int)lpoint.x, (int)lpoint.y, thickness, Mathf.Abs(h))
                    : new Rect((int)rpoint.x, (int)rpoint.y, thickness, Mathf.Abs(h)));
            }

            /*Debug.Log("Corridors: ");
            foreach(Rect corridor in corridors) {
                Debug.Log("corridor: " + corridor);
            }*/
        }

        private Rect GetRoom()
        {
            if (IAmLeaf())
            {
                return room;
            }

            if (left != null)
            {
                Rect lroom = left.GetRoom();
                if (lroom.x != -1)
                {
                    return lroom;
                }
            }

            if (right != null)
            {
                Rect rroom = right.GetRoom();
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }

            // workaround non nullable structs
            return new Rect(-1, -1, 0, 0);
        }

        public bool IAmLeaf() => left == null && right == null;

        /*
        choose a vertical or horizontal split depending on the proportion
        i.e. if too wide split vertically, or too long horizontally,
        or if nearly square choose vertical or horizontal at random
        */
        public bool Split(int minRoomSize, int maxRoomSize)
        {
            if (!IAmLeaf())
            {
                return false;
            }

            bool splitH;
            if (rect.width / rect.height >= 1.25)
            {
                splitH = false;
            }
            else if (rect.height / rect.width >= 1.25)
            {
                splitH = true;
            }
            else
            {
                splitH = Random.Range(0.0f, 1.0f) > 0.5;
            }

            if (Mathf.Min(rect.height, rect.width) / 2 < minRoomSize)
            {
                //Debug.Log("Sub-dungeon " + debugId + " will be a leaf");
                return false;
            }

            if (splitH)
            {
                // split so that the resulting sub-dungeons widths are not too small
                // (since we are splitting horizontally)
                int split = Random.Range(minRoomSize, (int)(rect.width - minRoomSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, rect.width, split));
                right = new SubDungeon(new Rect(rect.x, rect.y + split, rect.width, rect.height - split));
            }
            else
            {
                int split = Random.Range(minRoomSize, (int)(rect.height - minRoomSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, split, rect.height));
                right = new SubDungeon(new Rect(rect.x + split, rect.y, rect.width - split, rect.height));
            }

            return true;
        }
    }


    private void Update()
    {
        if (player.HasKey && !escapeDoor.IsDoorOpen)
        {
            escapeDoor.OpenTheDoor();
        }
    }

    private void CreateBSP(SubDungeon subDungeon)
    {
        //Debug.Log("Splitting sub-dungeon " + subDungeon.debugId + ": " + subDungeon.rect);
        if (subDungeon.IAmLeaf())
        {
            // if the subdungeon is too large
            if (subDungeon.rect.width > GameConfigData.Instance.MaxRoomSize ||
                subDungeon.rect.height > GameConfigData.Instance.MaxRoomSize || Random.Range(0.0f, 1.0f) > 0.25)
            {
                if (subDungeon.Split(GameConfigData.Instance.MinRoomSize, GameConfigData.Instance.MaxRoomSize))
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
                        GameObject instance =
                            Instantiate(
                                GameConfigData.Instance.FloorTiles[
                                    (int)Random.Range(0, GameConfigData.Instance.FloorTiles.Length)],
                                new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(dungeon.transform.GetChild((int)Tiles.Floors).gameObject
                            .transform);
                        dungeonFloorPositions[i, j] = instance;
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
                        GameObject instance =
                            Instantiate(
                                GameConfigData.Instance.FloorTiles[
                                    (int)Random.Range(0, GameConfigData.Instance.FloorTiles.Length)],
                                new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(dungeon.transform.GetChild((int)Tiles.Corridors).gameObject
                            .transform);
                        dungeonFloorPositions[i, j] = instance;
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
                GameObject instance = Instantiate(GameConfigData.Instance.BridgeTiles[index],
                    new Vector3(bridgePos.x, bridgePos.y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(dungeon.transform.GetChild((int)Tiles.Bridges).gameObject.transform);
                dungeonFloorPositions[bridgePos.x, bridgePos.y] = instance;
            }
        }
    }

    private void DrawWalls()
    {
        int matrixSize = 3;

        int[,] kernelMatrix = { { 4, 64, 2 }, { 128, 0, 32 }, { 8, 16, 1 } };

        for (int j = GameConfigData.Instance.DungeonColumns + (2 * GameConfigData.Instance.DungeonPadding) - matrixSize;
             j >= 0;
             j--)
        {
            for (int i = 0;
                 i <= GameConfigData.Instance.DungeonRows + (2 * GameConfigData.Instance.DungeonPadding) - matrixSize;
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

                GameObject instance = null;
                int wallPosX = i + 1, wallPosY = j + 1;
                if (dungeonFloorPositions[wallPosX, wallPosY] == null && DungeonMap[wallPosX, wallPosY] == 0)
                {
                    instance = Instantiate(GameConfigData.Instance.WallTiles[index],
                        new Vector3(wallPosX, wallPosY, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(dungeon.transform.GetChild((int)Tiles.Walls).gameObject.transform);
                    dungeonFloorPositions[wallPosX, wallPosY] = instance;

                    if (index != 0)
                    {
                        // placing floor tile under the walls
                        instance = Instantiate(
                            GameConfigData.Instance.FloorTiles[
                                (int)Random.Range(0, GameConfigData.Instance.FloorTiles.Length)],
                            new Vector3(wallPosX, wallPosY, 0f), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(dungeon.transform.GetChild((int)Tiles.Floors).gameObject
                            .transform);
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
                        GameObject instance = Instantiate(
                            DungeonMap[bridgePos.x + i, bridgePos.y + j + 1] != -1
                                ? GameConfigData.Instance.WaterTiles[0]
                                : GameConfigData.Instance.WaterTiles[1],
                            new Vector3(bridgePos.x + i, bridgePos.y + j, 0f), Quaternion.identity);
                        instance.transform.SetParent(dungeon.transform.GetChild((int)Tiles.Waters).gameObject
                            .transform);
                        if (i == 0 && j == 0)
                        {
                            instance.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        }
                        dungeonFloorPositions[bridgePos.x + i, bridgePos.y + j] = instance;
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

        for (int j = GameConfigData.Instance.DungeonColumns + (2 * GameConfigData.Instance.DungeonPadding) - matrixSize;
             j >= 0;
             j--)
        {
            for (int i = 0;
                 i <= GameConfigData.Instance.DungeonRows + (2 * GameConfigData.Instance.DungeonPadding) - matrixSize;
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
                    PoolFactory.instance.GetObject(ObjectType.Lamp, new Vector3(i + 1, j + 1, 0f), Quaternion.identity,
                        dungeon.transform.GetChild((int)Objects.Lamps).gameObject.transform);

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

    public void CreateDungeon(GameManager gameManager)
    {
        //Debug.Log("Creating dungeon...");
        this.gameManager = gameManager; // assigning Game Manager
        rootSubDungeon = new SubDungeon(new Rect(GameConfigData.Instance.DungeonPadding,
            GameConfigData.Instance.DungeonPadding, GameConfigData.Instance.DungeonRows,
            GameConfigData.Instance.DungeonColumns));
        CreateBSP(rootSubDungeon);
        rootSubDungeon.CreateRoom();

        dungeonFloorPositions =
            new GameObject[GameConfigData.Instance.DungeonRows + (2 * GameConfigData.Instance.DungeonPadding),
                GameConfigData.Instance.DungeonColumns + (2 * GameConfigData.Instance.DungeonPadding)];
        DungeonMap = new int[GameConfigData.Instance.DungeonRows + (2 * GameConfigData.Instance.DungeonPadding),
            GameConfigData.Instance.DungeonColumns + (2 * GameConfigData.Instance.DungeonPadding)];
        objectSpawnPos = new int[GameConfigData.Instance.DungeonRows + (2 * GameConfigData.Instance.DungeonPadding),
            GameConfigData.Instance.DungeonColumns + (2 * GameConfigData.Instance.DungeonPadding)];
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
        //Debug.Log("Dungeon creation ended.");
    }

    public void SpawnEverything(int dungeonLevel)
    {
        PlayerSpawner();
        RandomEnemySpawner(dungeonLevel);
        RandomTrapSpawner(dungeonLevel);
        RandomChestSpawner(dungeonLevel);

        objectSpawnPos = null; // after the spawning everything, set it to null
    }

    private void PlayerSpawner()
    {
        //Debug.Log("Spawning player...");
        GetRandomPos(rootSubDungeon); // getting random position in the dungeon for the player
        player.transform.position = randomPos;
        playerSpawnPos = randomPos;
        objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;

        GetRandomPos(rootSubDungeon); // getting random position in the dungeon for the exit
        escapeDoor = Instantiate(GameConfigData.Instance.ExitTile, new Vector3(randomPos.x, randomPos.y, 0f),
            Quaternion.identity).GetComponent<EscapeDoor>();
        escapeDoor.transform.SetParent(dungeon.transform);
        escapeDoor.gameManager = gameManager;
        objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;

        GetRandomPos(rootSubDungeon); // getting random position in the dungeon for the object
        GameObject key = Instantiate(GameConfigData.Instance.Key, new Vector3(randomPos.x, randomPos.y, 0f),
            Quaternion.identity) as GameObject;
        key.transform.SetParent(dungeon.transform);
        objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
        //Debug.Log("Player spawn ended.");
    }

    private void RandomChestSpawner(int dungeonLevel)
    {
        //Debug.Log("Spawning chests...");
        // spawning barrels
        for (int i = 0; i < 3; i++)
        {
            GetRandomPos(rootSubDungeon); // getting random position in the dungeon
            PoolFactory.instance.GetObject<Chest>(ObjectType.Barrel, new Vector3(randomPos.x, randomPos.y, 0f),
                Quaternion.identity, dungeon.transform.GetChild((int)Objects.Chests).gameObject.transform);
            objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
        }

        //spawning weapon chests if the level is the multiple of 2
        if (dungeonLevel % 2 == 1)
        {
            GetRandomPos(rootSubDungeon); // getting random position in the dungeon
            PoolFactory.instance.GetObject<Chest>(ObjectType.WeaponCrate, new Vector3(randomPos.x, randomPos.y, 0f),
                Quaternion.identity, dungeon.transform.GetChild((int)Objects.Chests).gameObject.transform);
            objectSpawnPos[(int)randomPos.x, (int)randomPos.y] = 1;
        }
        //Debug.Log("Chest spawn ended.");
    }

    private void RandomEnemySpawner(int dungeonLevel)
    {
        //Debug.Log("Spawning enemies...");
        SpawnEnemies(rootSubDungeon, dungeonLevel);
        // after creating copies, disable the original ones
        foreach (var enemy in enemies)
        {
            enemy.SetActive(false);
        }
        //Debug.Log("Enemy spawn ended.");
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
                            int enemyIndexRangeMin = (dungeonLevel < 7) ? enemyIndexes[dungeonLevel, 0] : enemyIndexes[6, 0];
                            int enemyIndexRangeMax = (dungeonLevel < 7) ? enemyIndexes[dungeonLevel, 1] : enemyIndexes[6, 1];
                            enemyIndex = Random.Range(enemyIndexRangeMin, enemyIndexRangeMax + 1);
                        } while (subDungeon.hasTurret && enemyIndex == 3); // check if the room has a turret and new enemy is turret
                        
                        Enemy enemy = PoolFactory.instance.GetObject<Enemy>(
                            (ObjectType)((int)ObjectType.Beetle + enemyIndex), randomPos, Quaternion.identity,
                            dungeon.transform.GetChild((int)Objects.Enemies).gameObject.transform);
                        if (enemy is IPathfinder pathfinderEnemy)
                        {
                            pathfinderEnemy.SetupPathfinding(player.transform, DungeonMap);
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

    private void RandomTrapSpawner(int dungeonLevel)
    {
        //Debug.Log("Spawning traps...");
        SpawnTraps(rootSubDungeon, dungeonLevel);
        //Debug.Log("Trap spawn ended.");
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

                        PoolFactory.instance.GetObject<Trap>(trapType, randomPos, Quaternion.identity,
                            dungeon.transform.GetChild((int)Objects.Traps).gameObject.transform);
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
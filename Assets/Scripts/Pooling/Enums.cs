using UnityEngine;

namespace Pooling
{
    public enum ObjectType
    {
        [InspectorName("Bullet/HitEffect")] HitEffect = 0,
        [InspectorName("Bullet/FireEffect")] FireEffect = 1,
        [InspectorName("Bullet/Light")] LightBullet = 2,
        [InspectorName("Bullet/Medium")] MediumBullet = 3,
        [InspectorName("Bullet/Heavy")] HeavyBullet = 4,
        [InspectorName("Bullet/Sniper")] SniperBullet = 5,
        [InspectorName("Bullet/Mini")] MiniBullet = 6,
        
        [InspectorName("Enemy/Beetle")] Beetle = 20,
        [InspectorName("Enemy/Mantis")] Mantis = 21,
        [InspectorName("Enemy/Soldier")] Soldier = 22,
        [InspectorName("Enemy/TowerPistol")] TowerPistol = 23,
        [InspectorName("Enemy/TowerShotgun")] TowerShotgun = 24,
        [InspectorName("Enemy/ExplosiveRobot")] ExplosiveRobot = 25,
        
        [InspectorName("Object/Chest/Barrel")] Barrel = 50,
        [InspectorName("Object/Chest/WeaponCrate")] WeaponCrate = 51,
        [InspectorName("Object/Trap/Spike")] SpikeTrap = 52,
        [InspectorName("Object/Trap/Fire")] FireTrap = 53,
        [InspectorName("Object/Prop/Lamp")] Lamp = 54,
        [InspectorName("Object/Collectible/Key")] Key = 55,
        
        #region Weapons
        
        [InspectorName("Object/Weapon/M1")] M1 = 100,
        [InspectorName("Object/Weapon/Pbz39")] Pbz39 = 101,
        [InspectorName("Object/Weapon/PPS43")] PPS43 = 102,
        [InspectorName("Object/Weapon/PPSh41")] PPSh41 = 103,
        [InspectorName("Object/Weapon/Sterling")] Sterling = 104,
        [InspectorName("Object/Weapon/Thompson")] Thompson = 105,
        [InspectorName("Object/Weapon/WinchesterM12")] WinchesterM12 = 106,
        
        [InspectorName("Object/Weapon/EnemySTG44")] EnemySTG44 = 130,
        [InspectorName("Object/Weapon/EnemyTowerPistol")] EnemyTowerPistol = 131,
        [InspectorName("Object/Weapon/EnemyTowerShotgun")] EnemyTowerShotgun = 132,
        
        #endregion
        
        #region Dungeon
        
        [InspectorName("Dungeon/Floor/Type1")] DungeonFloor1 = 1000,
        [InspectorName("Dungeon/Floor/Type2")] DungeonFloor2 = 1001,
        [InspectorName("Dungeon/Floor/Type3")] DungeonFloor3 = 1002,
        [InspectorName("Dungeon/Floor/Type4")] DungeonFloor4 = 1003,
        [InspectorName("Dungeon/Floor/Type5")] DungeonFloor5 = 1004,
        
        [InspectorName("Dungeon/Door/EscapeDoor")] EscapeDoor = 1020,
        
        [InspectorName("Dungeon/WaterTile/Type1")] WaterTile1 = 1040,
        [InspectorName("Dungeon/WaterTile/Type2")] WaterTile2 = 1041,
        
        [InspectorName("Dungeon/BridgeTile/Horizontal")] HorizontalBridgeTile = 1100,
        [InspectorName("Dungeon/BridgeTile/Vertical")] VerticalBridgeTile = 1101,
        [InspectorName("Dungeon/BridgeTile/DownLeft")] DownLeftBridgeTile = 1102,
        [InspectorName("Dungeon/BridgeTile/DownRight")] DownRightBridgeTile = 1103,
        [InspectorName("Dungeon/BridgeTile/TopLeft")] TopLeftBridgeTile = 1104,
        [InspectorName("Dungeon/BridgeTile/TopRight")] TopRightBridgeTile = 1105,
        [InspectorName("Dungeon/BridgeTile/TRight")] TRightBridgeTile = 1106,
        [InspectorName("Dungeon/BridgeTile/TLeft")] TLeftBridgeTile = 1107,
        [InspectorName("Dungeon/BridgeTile/TUp")] TUpBridgeTile = 1108,
        [InspectorName("Dungeon/BridgeTile/TDown")] TDownBridgeTile = 1109,
        [InspectorName("Dungeon/BridgeTile/Plus")] PlusBridgeTile = 1110,
        
        [InspectorName("Dungeon/WallTile/None")] NoneWallTile = 1200,
        [InspectorName("Dungeon/WallTile/Outer/MiddleMiddle")] OuterMiddleMiddleWallTile = 1201,
        [InspectorName("Dungeon/WallTile/Inner/BottomLeft")] InnerBottomLeftWallTile = 1202,
        [InspectorName("Dungeon/WallTile/Inner/TopLeft")] InnerTopLeftWallTile = 1203,
        [InspectorName("Dungeon/WallTile/Outer/MiddleRight")] OuterMiddleRightWallTile = 1204,
        [InspectorName("Dungeon/WallTile/Inner/TopRight")] InnerTopRightWallTile = 1205,
        [InspectorName("Dungeon/WallTile/Special/1331")] Special_1331WallTile = 1206,
        [InspectorName("Dungeon/WallTile/Inner/BottomRight")] InnerBottomRightWallTile = 1207,
        [InspectorName("Dungeon/WallTile/Special/T_1113")] Special_T1113WallTile = 1208,
        [InspectorName("Dungeon/WallTile/Special/1133")] Special_1133WallTile = 1209,
        [InspectorName("Dungeon/WallTile/Outer/MiddleLeft")] OuterMiddleLeftWallTile = 1210,
        [InspectorName("Dungeon/WallTile/Special/1321")] Special_1321WallTile = 1211,
        [InspectorName("Dungeon/WallTile/Special/2133")] Special_2133WallTile = 1212,
        [InspectorName("Dungeon/WallTile/Vertical/Middle")] VerticalMiddleWallTile = 1213,
        [InspectorName("Dungeon/WallTile/Inner/BottomMiddle")] InnerBottomMiddleWallTile = 1214,
        [InspectorName("Dungeon/WallTile/Outer/TopMiddle")] OuterTopMiddleWallTile = 1215,
        [InspectorName("Dungeon/WallTile/Special/1233")] Special_1233WallTile = 1216,
        [InspectorName("Dungeon/WallTile/Special/1231")] Special_1231WallTile = 1217,
        [InspectorName("Dungeon/WallTile/Vertical/TopWithLight")] VerticalTopWithLightWallTile = 1218,
        [InspectorName("Dungeon/WallTile/Special/2331")] Special_2331WallTile = 1219,
        [InspectorName("Dungeon/WallTile/Special/1123")] Special_1123WallTile = 1220,
        [InspectorName("Dungeon/WallTile/Outer/TopRight")] OuterTopRightWallTile = 1221,
        [InspectorName("Dungeon/WallTile/InOut/TopRight")] InOutTopRightWallTile = 1222,
        [InspectorName("Dungeon/WallTile/Inner/TopMiddle")] InnerTopMiddleWallTile = 1223,
        [InspectorName("Dungeon/WallTile/Special/WithLight")] Special_WithLightWallTile = 1224,
        [InspectorName("Dungeon/WallTile/Outer/BottomMiddle")] OuterBottomMiddleWallTile = 1225,
        [InspectorName("Dungeon/WallTile/Special/T_13")] Special_T13WallTile = 1226,
        [InspectorName("Dungeon/WallTile/Special/T_11")] Special_T11WallTile = 1227,
        [InspectorName("Dungeon/WallTile/InOut/TopMiddle")] InOutTopMiddleWallTile = 1228,
        [InspectorName("Dungeon/WallTile/Outer/BottomRightWithLight")] OuterBottomRightWithLightWallTile = 1229,
        [InspectorName("Dungeon/WallTile/InOut/BottomRight")] InOutBottomRightWallTile = 1230,
        [InspectorName("Dungeon/WallTile/Horizontal/Right")] HorizontalRightWallTile = 1231,
        [InspectorName("Dungeon/WallTile/Outer/TopLeftWithLight")] OuterTopLeftWithLightWallTile = 1232,
        [InspectorName("Dungeon/WallTile/InOut/TopLeft")] InOutTopLeftWallTile = 1233,
        [InspectorName("Dungeon/WallTile/Outer/BottomLeft")] OuterBottomLeftWallTile = 1234,
        [InspectorName("Dungeon/WallTile/InOut/BottomLeft")] InOutBottomLeftWallTile = 1235,
        [InspectorName("Dungeon/WallTile/Horizontal/LeftWithLight")] HorizontalLeftWithLightWallTile = 1236,
        [InspectorName("Dungeon/WallTile/Vertical/BottomWithLight")] VerticalBottomWithLightWallTile = 1237,
        [InspectorName("Dungeon/WallTile/Inner/MiddleLeft")] InnerMiddleLeft = 1238,
        [InspectorName("Dungeon/WallTile/Inner/MiddleRight")] InnerMiddleRight = 1239,
        [InspectorName("Dungeon/WallTile/Single")] SingleWallTile = 1240,
        [InspectorName("Dungeon/WallTile/InOut/BottomMiddle")] InOutBottomMiddleWallTile = 1241,
        [InspectorName("Dungeon/WallTile/InOut/MiddleLeft")] InOutMiddleLeftWallTile = 1242,
        [InspectorName("Dungeon/WallTile/InOut/MiddleRight")] InOutMiddleRightWallTile = 1243,
        
        #endregion
    }
}
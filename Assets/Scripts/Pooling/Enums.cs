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
        
        #region Dungeon
        
        [InspectorName("Dungeon/Floor/Type1")] DungeonFloor1 = 1000,
        [InspectorName("Dungeon/Floor/Type2")] DungeonFloor2 = 1001,
        [InspectorName("Dungeon/Floor/Type3")] DungeonFloor3 = 1002,
        [InspectorName("Dungeon/Floor/Type4")] DungeonFloor4 = 1003,
        [InspectorName("Dungeon/Floor/Type5")] DungeonFloor5 = 1004,
        
        [InspectorName("Dungeon/Door/EscapeDoor")] EscapeDoor = 1020,
        
        [InspectorName("Dungeon/WaterTile/Type1")] WaterTile1 = 1040,
        [InspectorName("Dungeon/WaterTile/Type2")] WaterTile2 = 1041,
        
        #endregion
    }
}
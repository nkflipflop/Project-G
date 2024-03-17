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
        [InspectorName("Enemy/ExplosiveRobot")] ExplosiveRobot = 24,
    }
}
using Pooling;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "WeaponInfo", menuName = "Gameplay/Weapon/Info", order = 0)]
    public class WeaponInfo : ScriptableObject
    {
        public Type type;
        public new string name;
        public float reloadTime;
        public float fireRate;
        public bool hasRecoil;
        public bool automatic;
        public int ammoCapacity;
        public int bulletPerShot;
        public ObjectType bulletType;
        public Sprite bulletIcon;
        public SoundManager.Sound fireSound;
    }
}
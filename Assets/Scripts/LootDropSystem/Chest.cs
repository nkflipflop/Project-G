using General;
using NaughtyAttributes;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;

namespace LootSystem
{
    [System.Serializable]
    public class Chest : MonoBehaviour, IHealthInteractable, IPoolable
    {
        [field: SerializeField] public ObjectType Type { get; set; }
        public enum ChestType
        {
            Consumable,
            Weapon
        }

        [Foldout("Chest Properties"), SerializeField] private ChestType type;
        [Foldout("Chest Properties"), SerializeField] private int numItemsToDrop; // How many items treasure will spawn

        [Foldout("Loot Drop Tables")] public GenericLootDropTableConsumable consumableDropTable;
        [Foldout("Loot Drop Tables")] public GenericLootDropTableGameObject weaponDropTable;

        private void Start()
        {
            switch (type)
            {
                case ChestType.Consumable:
                    consumableDropTable.ValidateTable();
                    break;
                case ChestType.Weapon:
                    weaponDropTable.ValidateTable();
                    break;
            }
        }

        private void Update()
        {
            if ((this as IHealthInteractable).IsDead)
            {
                switch (type)
                {
                    case ChestType.Consumable:
                        DropConsumableNearChest(numItemsToDrop);
                        break;
                    case ChestType.Weapon:
                        DropWeaponNearChest();
                        break;
                }
            }
        }

        private void OnValidate()
        {
            switch (type)
            {
                // Validate table and notify the programmer / designer if something went wrong.
                case ChestType.Consumable:
                    consumableDropTable.ValidateTable();
                    break;
                case ChestType.Weapon:
                    weaponDropTable.ValidateTable();
                    break;
            }
        }

        /// <summary> Spawning consumable objects around the chest </summary>
        private void DropConsumableNearChest(int numberOfItems)
        {
            // TODO: consumable items should be pool objects
            for (int i = 0; i < numberOfItems; i++)
            {
                GenericLootDropItemConsumable selectedItem = consumableDropTable.PickLootDropItem();
                GameObject collectibleObject = Instantiate(GameConfigData.Instance.Consumable);
                collectibleObject.GetComponent<ConsumableObject>().Item = selectedItem.item;
                collectibleObject.transform.position = new Vector2(transform.position.x, transform.position.y) +
                                                       Random.insideUnitCircle / 2.8f;
            }
            enabled = false;
        }

        /// <summary> Spawning weapon around the chest </summary>
        private void DropWeaponNearChest()
        {
            // TODO: consumable items should be pool objects
            GenericLootDropItemGameObject selectedItem = weaponDropTable.PickLootDropItem();
            GameObject weaponObject = Instantiate(selectedItem.item);
            weaponObject.transform.position =
                new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle / 2.8f;
            enabled = false;
        }

        #region Health Operations

        [field: SerializeField, Foldout("Health")] public int CurrentHealth { get; set; }

        [field: SerializeField, Foldout("Health")] public int MaxHealth { get; set; }
        [field: SerializeField, Foldout("Health")] public Dissolve DissolveEffect { get; set; }
        [field: SerializeField, Foldout("Health")] public SpriteRenderer HealthEffectRenderer { get; set; }
        [field: SerializeField, Foldout("Health")] public CapsuleCollider2D HitBoxCollider { get; set; }
        [field: SerializeField, Foldout("Health")] public SoundManager.Sound HitSound { get; set; }

        #endregion

        #region Pooling
    
        public void OnSpawn()
        {
            CurrentHealth = MaxHealth;
        }

        public void OnReset() { }
    
        #endregion
    }
}
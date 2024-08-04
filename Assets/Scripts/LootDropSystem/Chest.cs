﻿using General;
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
        
        [SerializeField] private PoolObjectLootDropInfo lootDropInfo;
        
        [Foldout("Chest Properties"), SerializeField] private int numItemsToDrop; // How many items treasure will spawn

        private void Start()
        {
            lootDropInfo.dropTable.ValidateTable();
        }

        private void Update()
        {
            if ((this as IHealthInteractable).IsDead)
            {
                DropItems();
            }
        }

        private void OnValidate()
        {
            lootDropInfo?.dropTable.ValidateTable();
        }

        private void DropItems()
        {
            for (int i = 0; i < lootDropInfo.numItemsToDrop; i++)
            {
                GenericLootDropItemPoolObject selectedItem = lootDropInfo.dropTable.PickLootDropItem();
                IPoolable collectibleObject = PoolFactory.instance.GetObject(selectedItem.item.type,
                    new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle / 2.8f,
                    Quaternion.identity);
                if (collectibleObject.GameObject.TryGetComponent(out ConsumableObject consumableObject))
                {
                    consumableObject.SetItem(selectedItem.item.consumable);
                }
            }
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
            DissolveEffect.Reset();
        }

        public void OnReset() { }
    
        #endregion
    }
}
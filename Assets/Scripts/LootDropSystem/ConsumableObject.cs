using Gameplay.Runtime.Player;
using General;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;

namespace LootDropSystem
{
    public class ConsumableObject : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public ObjectType Type { get; set; }
    
        private Consumable Item { get; set; }

        [SerializeField] private SpriteRenderer spriteRenderer;
        private Player player;

        private void Start()
        {
            spriteRenderer.sprite = Item.Sprite;
        }

        public void SetItem(Consumable item)
        {
            Item = item;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // If collide with an Player, Subscribe the playerManager
            if (other.gameObject.CompareTag("Player") && !player)
            {
                if (other.gameObject.TryGetComponent(out player))
                {
                    player.CollectPUB += CollectSUB;
                }
            }
        }
    
        private void CollectSUB(Inventory[] inventory, IHealthInteractable healthInteractable)
        {
            bool collected = false;

            if (Item.Type == GameConfigData.CollectibleType.Snack)
            {
                if (healthInteractable.GainHealth(Item.Value))      // Snacks
                {
                    collected = true;
                }
            }
            else if (inventory[(int)Item.Type].Count < 3)
            {
                inventory[(int)Item.Type].Count += 1;       // MedKit or Shield, If inventory is not full
                collected = true;
            }
        
            if (collected)      // If object is collected, destroy it
            {
                player.CollectPUB -= CollectSUB;
                this.ResetObject();
            }
        }
        
        public virtual void OnSpawn() { }

        public virtual void OnReset() { }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableObject : MonoBehaviour
{
    public Consumable Item;

    [SerializeField] private  SpriteRenderer _spriteRenderer = null;
    private PlayerManager _playerManager;

    void Start() {
        _spriteRenderer.sprite = Item.Sprite;
    }

    // Collision Controller
    private void OnTriggerEnter2D(Collider2D other) {
        // If collide with an Player
		if (other.gameObject.CompareTag("Player")){
            _playerManager = other.gameObject.GetComponent<PlayerManager>();
            _playerManager.CollectPUB += CollectSUB;
        }
	}

    // Collection Subscriber
    private void CollectSUB(Inventory[] inventory, HealthController playerHealthController) {
        if(Item.Type == GameConfigData.CollectibleType.Medkit){
            inventory[0].Count += 1;    // Medkit
        }
        else if(Item.Type == GameConfigData.CollectibleType.Shield){
            inventory[1].Count += 1;    // Shield
        }
        else {
            playerHealthController.Heal(Item.RestoreHealthValue);   // Snacks
        }

        _playerManager.CollectPUB -= CollectSUB;
         Destroy(gameObject);
    }
}

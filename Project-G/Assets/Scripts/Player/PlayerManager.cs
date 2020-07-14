using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {
    public GameConfigData.CollectibleType Type;
    public int Count;
};

public class PlayerManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public PlayerHandController PlayerHandController;
    public HealthController HealthController;

    [SerializeField]
    private Inventory[] _inventory;     // Inventory
    public event Action<Inventory[], HealthController> CollectPUB;     // Item collection Publisher
    private bool _key = false;


    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown (KeyCode.Alpha1))
            UseMedkit();
        
        if (Input.GetKeyDown (KeyCode.Alpha2))
            UseShield();   
    }

    // Using Medkit
    private void UseMedkit(){
        if (_inventory[0].Count > 0){
            _inventory[0].Count -= 1;
            HealthController.Heal(30);
        }
    }

    // Using Medkit
    private void UseShield(){
        if (_inventory[1].Count > 0){
            _inventory[1].Count -= 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // If collide with an consumable item
		if (other.gameObject.CompareTag("Item")){
            // If there are items
            CollectPUB?.Invoke(_inventory, HealthController);
        }
        else if (other.gameObject.CompareTag("Key")){
            // If there are Key
            _key = true;
            Destroy(other.gameObject);
        }
	}
}

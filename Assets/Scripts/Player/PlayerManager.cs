using System;
using UnityEngine;

[Serializable]
public class Inventory
{
    public GameConfigData.CollectibleType Type;
    public Consumable Item;
    public int Count;
}

public class PlayerManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public PlayerHandController PlayerHandController;
    public HealthController HealthController;
    [SerializeField] private GameObject _shield = null;

    [SerializeField] private Inventory[] _inventory = null; // Inventory
    public event Action<Inventory[], HealthController> CollectPUB; // Item collection Publisher
    private bool hasKey;
    private float shieldTime;

    public Inventory[] Inventory => _inventory;

    public bool HasKey => hasKey;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseMedKit();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !_shield.activeSelf)
        {
            UseShield();
        }

        if (_shield.activeSelf)
        {
            shieldTime -= Time.deltaTime;
            if (shieldTime <= 0)
            {
                _shield.SetActive(false);
            }
        }
    }
    
    private void UseMedKit()
    {
        Inventory inventory = _inventory[(int)GameConfigData.CollectibleType.Medkit];
        if (inventory.Count > 0 && HealthController.Health < 100)
        {
            inventory.Count -= 1;
            HealthController.Heal(inventory.Item.Value);
        }
    }
    
    private void UseShield()
    {
        Inventory inventory = _inventory[(int)GameConfigData.CollectibleType.Shield];
        if (inventory.Count > 0)
        {
            inventory.Count -= 1;
            _shield.SetActive(true);
            shieldTime = inventory.Item.Value;
        }
    }

    public void LoadPlayerData()
    {
        HealthController.Health = DataManager.instance.Health;
        _inventory[(int)GameConfigData.CollectibleType.Medkit].Count = DataManager.instance.MedKits;
        _inventory[(int)GameConfigData.CollectibleType.Shield].Count = DataManager.instance.Shields;

        GameObject weapon = Instantiate(GameConfigData.Instance.Weapons[(int)DataManager.instance.WeaponType]); // instantiating player's weapon
        PlayerHandController.EquipWeapon(weapon.GetComponent<WeaponBase>());
    }

    public void SavePlayerData()
    {
        DataManager.instance.Health = HealthController.Health; // storing player's health
        DataManager.instance.MedKits = _inventory[(int)GameConfigData.CollectibleType.Medkit].Count;
        DataManager.instance.Shields = _inventory[(int)GameConfigData.CollectibleType.Shield].Count;
        DataManager.instance.WeaponType = PlayerHandController.CurrentWeapon.WeaponType; // storing player's weapon
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If collide with an consumable item
        if (other.gameObject.CompareTag("Item"))
        {
            // If there are items
            CollectPUB?.Invoke(_inventory, HealthController);
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            // If there are Key
            hasKey = true;
            other.gameObject.SetActive(false);
        }
    }
}
using System;
using General;
using NaughtyAttributes;
using Pooling;
using UnityEngine;

[Serializable]
public class Inventory
{
    public GameConfigData.CollectibleType Type;
    public Consumable Item;
    public int Count;
}

public class PlayerManager : MonoBehaviour, IHealthInteractable
{
    public PlayerController PlayerController;
    public PlayerHandController PlayerHandController;
    [SerializeField] private GameObject _shield = null;

    [SerializeField] private Inventory[] _inventory = null; // Inventory
    public event Action<Inventory[], IHealthInteractable> CollectPUB; // Item collection Publisher
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
        if (inventory.Count > 0 && (this as IHealthInteractable).GainHealth(inventory.Item.Value))
        {
            inventory.Count -= 1;
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
        (this as IHealthInteractable).CurrentHealth = DataManager.instance.Health;
        _inventory[(int)GameConfigData.CollectibleType.Medkit].Count = DataManager.instance.MedKits;
        _inventory[(int)GameConfigData.CollectibleType.Shield].Count = DataManager.instance.Shields;

        WeaponBase weapon =
            PoolFactory.instance.GetObject<WeaponBase>(ObjectType.M1 + (int)DataManager.instance.WeaponType);
        PlayerHandController.EquipWeapon(weapon);
    }

    public void SavePlayerData()
    {
        DataManager.instance.Health = (this as IHealthInteractable).CurrentHealth; // storing player's health
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
            CollectPUB?.Invoke(_inventory, this);
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            // If there are Key
            hasKey = true;
            other.gameObject.SetActive(false);
        }
    }
    
    #region Health Operations
    
    [field: SerializeField, Foldout("Health")] public int CurrentHealth { get; set; }
    [field: SerializeField, Foldout("Health")] public int MaxHealth { get; set; }
    [field: SerializeField, Foldout("Health")] public Dissolve DissolveEffect { get; set; }
    [field: SerializeField, Foldout("Health")] public SpriteRenderer HealthEffectRenderer { get; set; }
    [field: SerializeField, Foldout("Health")] public CapsuleCollider2D HitBoxCollider { get; set; }
    [field: SerializeField, Foldout("Health")] public SoundManager.Sound HitSound { get; set; }
    
    #endregion
}
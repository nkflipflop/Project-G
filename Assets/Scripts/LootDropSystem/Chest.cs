using UnityEngine;

[System.Serializable]
public class Chest : MonoBehaviour
{
    public enum ChestType
    {
        Consumable,
        Weapon
    }

    public HealthController HealthController;

    [Header("Chest Properties")] public ChestType Type;
    public int NumItemsToDrop; // How many items treasure will spawn

    [Header("Loot Drop Tables")] public GenericLootDropTableConsumable ConsumableDropTable;
    public GenericLootDropTableGameObject WeaponDropTable;
    private bool _lootDropped = false;

    private void Start()
    {
        switch (Type)
        {
            case ChestType.Consumable:
                ConsumableDropTable.ValidateTable();
                break;
            case ChestType.Weapon:
                WeaponDropTable.ValidateTable();
                break;
        }
    }

    private void Update()
    {
        if (HealthController.IsDead && !_lootDropped)
        {
            switch (Type)
            {
                case ChestType.Consumable:
                    DropConsumableNearChest(NumItemsToDrop);
                    break;
                case ChestType.Weapon:
                    DropWeaponNearChest();
                    break;
            }
        }
    }

    private void OnValidate()
    {
        switch (Type)
        {
            // Validate table and notify the programmer / designer if something went wrong.
            case ChestType.Consumable:
                ConsumableDropTable.ValidateTable();
                break;
            case ChestType.Weapon:
                WeaponDropTable.ValidateTable();
                break;
        }
    }

    /// <summary> Spawning consumable objects around the chest </summary>
    /// <param name="numItemsToDrop"></param>
    private void DropConsumableNearChest(int numItemsToDrop)
    {
        for (int i = 0; i < numItemsToDrop; i++)
        {
            GenericLootDropItemConsumable selectedItem = ConsumableDropTable.PickLootDropItem();
            GameObject collectibleObject = Instantiate(GameConfigData.Instance.Consumable);
            collectibleObject.GetComponent<ConsumableObject>().Item = selectedItem.item;
            collectibleObject.transform.position = new Vector2(transform.position.x, transform.position.y) +
                                                   Random.insideUnitCircle / 2.8f;
        }

        _lootDropped = true;
    }

    /// <summary> Spawning weapon around the chest </summary>
    private void DropWeaponNearChest()
    {
        GenericLootDropItemGameObject selectedItem = WeaponDropTable.PickLootDropItem();
        GameObject weaponObject = Instantiate(selectedItem.item);
        weaponObject.transform.position =
            new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle / 2.8f;
        _lootDropped = true;
    }
}
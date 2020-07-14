using UnityEngine;

[System.Serializable]
public class Chest : MonoBehaviour 
{
    public enum ChestType { Consumable, Weapon };
    public HealthController HealthController;
    
    [Header("Chest Properties")]
    public ChestType Type;
    public int NumItemsToDrop;          // How many items treasure will spawn
	
    [Header("Loot Drop Tables")]
	public GenericLootDropTableConsumable ConsumableDropTable;
    public GenericLootDropTableGameObject WeaponDropTable;
    private bool _lootDropped = false;

    private void Start() {
        if (Type == ChestType.Consumable)
            ConsumableDropTable.ValidateTable();
        else if (Type == ChestType.Weapon)
            WeaponDropTable.ValidateTable();
    }

    private void Update() {
        if (HealthController.IsDead && !_lootDropped) {
            if (Type == ChestType.Consumable)
                DropConsumableNearChest(NumItemsToDrop);
            else if (Type == ChestType.Weapon)
                DropWeaponNearChest();
        }     
    }
    
    private void OnValidate() {
        // Validate table and notify the programmer / designer if something went wrong.
        if (Type == ChestType.Consumable)
            ConsumableDropTable.ValidateTable();
        else if (Type == ChestType.Weapon)
            WeaponDropTable.ValidateTable();
        
    }
 
    /// <summary> Spawning consumable objects around the chest </summary>
    /// <param name="numItemsToDrop"></param>
    private void DropConsumableNearChest(int numItemsToDrop) {
        for (int i = 0; i < numItemsToDrop; i++) {
            GenericLootDropItemConsumable selectedItem = ConsumableDropTable.PickLootDropItem();
            GameObject collectibleObject = Instantiate(GameConfigData.Instance.Consumable);
            collectibleObject.GetComponent<ConsumableObject>().Item = selectedItem.item;
            collectibleObject.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle / 2.8f;
        }
        _lootDropped = true;
    }

    /// <summary> Spawning weapon around the chest </summary>
    private void DropWeaponNearChest() {
        GenericLootDropItemGameObject selectedItem = WeaponDropTable.PickLootDropItem();
        GameObject weaponObject = Instantiate(selectedItem.item);
        weaponObject.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle / 2.8f;
        _lootDropped = true;
    }
}
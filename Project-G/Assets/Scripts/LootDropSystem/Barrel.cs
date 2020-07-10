using UnityEngine;

[System.Serializable]
public class Barrel : MonoBehaviour 
{
	// Loot drop table that contains items that can spawn
	public GenericLootDropTableGameObject lootDropTable;
	
    // How many items treasure will spawn
	public int NumItemsToDrop;
    public DamageHelper DamageHelper;
    private bool _lootDropped = false;

    private void Start() {
        lootDropTable.ValidateTable();
    }

    private void Update() {
        if (DamageHelper.IsDead && !_lootDropped)
            DropLootNearChest(NumItemsToDrop);      // Spawn objects in a straight line
    }
    
    private void OnValidate() {
        // Validate table and notify the programmer / designer if something went wrong.
        lootDropTable.ValidateTable();
        
    }
 
    /// <summary>
    /// Spawning objects in horizontal line
    /// </summary>
    /// <param name="numItemsToDrop"></param>
    void DropLootNearChest(int numItemsToDrop) {
        for (int i = 0; i < numItemsToDrop; i++) {
            GenericLootDropItemGameObject selectedItem = lootDropTable.PickLootDropItem();
            GameObject collectibleObject = Instantiate(GameConfigData.Instance.Collectible);
            collectibleObject.GetComponent<CollectibleObject>().Item = selectedItem.item;
            collectibleObject.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle / 2.8f;
        }
        _lootDropped = true;
    }
}
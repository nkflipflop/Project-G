/// <summary>
/// When we're inheriting we have to insert Consumable as a type to GenericLootDropItem
/// </summary>
[System.Serializable]
public class GenericLootDropItemConsumable : GenericLootDropItem<Consumable>
{
}
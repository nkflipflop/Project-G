using UnityEngine;

/// <summary>
/// When inheriting first we have to insert GenericLootDropItemGameObject instead of T and a GameObject instead of U
/// </summary>
[System.Serializable]
public class GenericLootDropTableGameObject : GenericLootDropTable<GenericLootDropItemGameObject, GameObject>
{
}
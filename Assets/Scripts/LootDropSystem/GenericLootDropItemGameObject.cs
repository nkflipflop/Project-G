﻿using UnityEngine;

/// <summary>
/// When we're inheriting we have to insert GameObject as a type to GenericLootDropItem
/// </summary>
[System.Serializable]
public class GenericLootDropItemGameObject : GenericLootDropItem<GameObject>
{
}
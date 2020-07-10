using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item", order = 1)]
public class Item : ScriptableObject {
    public GameConfigData.CollectibleType Type;
    public int RestoreHealthValue;
    public Sprite Sprite;

}

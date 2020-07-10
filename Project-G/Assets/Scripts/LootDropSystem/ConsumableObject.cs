using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableObject : MonoBehaviour
{
    public Consumable Item;
    [SerializeField] private  SpriteRenderer _spriteRenderer = null;

    void Start() {
        _spriteRenderer.sprite = Item.Sprite;
    }
}

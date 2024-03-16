using System;
using Cysharp.Threading.Tasks;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;

public class FireEffect : MonoBehaviour, IPoolable
{
    [field: SerializeField] public ObjectType Type { get; set; }

    public async UniTaskVoid Play()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        this.ResetObject();
    }
    
    public void OnSpawn() { }

    public void OnReset() { }
}
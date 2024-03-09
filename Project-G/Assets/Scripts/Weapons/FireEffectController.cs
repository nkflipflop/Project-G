using Cysharp.Threading.Tasks;
using UnityEngine;

public class FireEffectController : MonoBehaviour
{
    private void Start()
    {
        TurnOffLight();
    }

    private async UniTaskVoid TurnOffLight()
    {
        await UniTask.Delay(50);
        Destroy(gameObject);
    }
}
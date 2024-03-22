using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Utilities
{
    public static partial class Extensions
    {
        public static void SetPositionX(this Transform transform, float x)
        {
            Vector3 position = transform.position;
            position.x = x;
            transform.position = position;
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            Vector3 position = transform.position;
            position.y = y;
            transform.position = position;
        }

        public static void SetPositionZ(this Transform transform, float z)
        {
            Vector3 position = transform.position;
            position.z = z;
            transform.position = position;
        }
		
        public static async UniTask Punch(this Transform transform)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            await transform.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
        }
    }
}
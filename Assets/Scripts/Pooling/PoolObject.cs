using System;
using Pooling.Interfaces;
using UnityEngine;

namespace Pooling
{
    public class PoolObject : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public ObjectType Type { get; set; }
        [field: SerializeField] public int InitialSize { get; set; }
        
        private Action<PoolObject> returnToPool;

        protected virtual void OnDisable()
        {
            ReturnToPool();
        }

        public virtual void Initialize(Action<IPoolable> returnAction)
        {
            returnToPool = returnAction;
        }

        public virtual void ReturnToPool()
        {
            returnToPool?.Invoke(this);
        }
    }
}
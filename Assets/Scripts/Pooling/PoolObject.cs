using Pooling.Interfaces;
using UnityEngine;

namespace Pooling
{
    public class PoolObject : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public ObjectType Type { get; set; }
        
        public virtual void OnSpawn() { }

        public virtual void OnReset() { }

        public void OnParticleSystemStopped()
        {
            if (gameObject.activeInHierarchy)
            {
                this.ResetObject();
            }
        }
    }
}
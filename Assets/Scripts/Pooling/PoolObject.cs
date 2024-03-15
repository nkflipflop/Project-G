using Pooling.Interfaces;
using UnityEngine;

namespace Pooling
{
    public class PoolObject : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public ObjectType Type { get; set; }
        public GameObject GameObject { get; set; }
        
        public void OnParticleSystemStopped()
        {
            if (gameObject.activeInHierarchy)
            {
                this.ResetObject();
            }
        }
    }
}
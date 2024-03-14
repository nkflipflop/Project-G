using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class PoolFactory : Singleton<PoolFactory>
    {
        private Dictionary<ObjectType, ObjectPool<PoolObject>> pools;
        
        [SerializeField] private PoolObject[] prefabs;
        
        public override void Awake()
        {
            base.Awake();

            if (instance == this)
            {
                Initialize();
            }
        }
        
        private void Initialize()
        {
            if (pools == null)
            {
                pools = new Dictionary<ObjectType, ObjectPool<PoolObject>>(new ObjectTypeComparer());

                foreach (PoolObject obj in prefabs)
                {
                    CreateObjectPool(obj);
                }
            }
        }
        
        private void CreateObjectPool(PoolObject obj)
        {
            if (!pools.ContainsKey(obj.Type))
            {
                ObjectPool<PoolObject> objectPool = new ObjectPool<PoolObject>(obj, obj.InitialSize);
                pools.Add(obj.Type, objectPool);
            }
        }

        private ObjectPool<PoolObject> GetPool(ObjectType type)
        {
            return pools.GetValueOrDefault(type);
        }
        
        public PoolObject GetObject(ObjectType type)
        {
            ObjectPool<PoolObject> pool = GetPool(type);
            return pool?.Pull();
        }
    }
}
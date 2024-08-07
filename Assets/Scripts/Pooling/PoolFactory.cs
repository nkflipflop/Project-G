using System;
using System.Collections.Generic;
using Pooling.Interfaces;
using UnityEngine;
using Utilities;

namespace Pooling
{
    public class PoolFactory : Singleton<PoolFactory>
    {
        private Dictionary<ObjectType, ObjectPool> pools;
        
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private List<PoolPreset> presets;
        
#if UNITY_EDITOR
        [SerializeField, NaughtyAttributes.ReadOnly] private List<ObjectPoolOnInspector> poolsOnInspector;
#endif
        
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
                pools = new Dictionary<ObjectType, ObjectPool>(new ObjectTypeComparer());
#if UNITY_EDITOR
                poolsOnInspector = new List<ObjectPoolOnInspector>();
#endif

                foreach (GameObject obj in prefabs)
                {
                    CreateObjectPool(obj);
                }
            }
        }
        
        private void CreateObjectPool(GameObject obj)
        {
            IPoolable poolableObj = TryToGetIPoolable(obj);
            if (poolableObj != null && !pools.ContainsKey(poolableObj.Type))
            {
                PoolPreset preset = presets.Find(x =>
                {
                    IPoolable targetIPoolable = TryToGetIPoolable(x.obj);
                    return targetIPoolable != null && targetIPoolable.Type == poolableObj.Type;
                });
                ObjectPool objectPool = new ObjectPool(poolableObj, preset?.initialSize ?? 0);
                pools.Add(poolableObj.Type, objectPool);
                
#if UNITY_EDITOR
                poolsOnInspector.Add(new ObjectPoolOnInspector(poolableObj.Type, objectPool));
#endif
            }
            
            IPoolable TryToGetIPoolable(GameObject targetObj)
            {
                foreach (MonoBehaviour monoBehaviour in targetObj.GetComponents<MonoBehaviour>())
                {
                    if (monoBehaviour is IPoolable iPoolable)
                    {
                        return iPoolable;
                    }
                }
                return null;
            }
        }
        
        #region Get Methods

        private ObjectPool GetPool(ObjectType type)
        {
            return pools.GetValueOrDefault(type);
        }
        
        public IPoolable GetObject(ObjectType type, Transform parent = null, Vector3? position = null, Vector3? scale = null, Vector3? rotation = null)
        {
            ObjectPool pool = GetPool(type);
            return pool?.Pull(parent, position, scale, rotation);
        }
        
        public T GetObject<T>(ObjectType type, Transform parent = null, Vector3? position = null, Vector3? scale = null, Vector3? rotation = null) where T : IPoolable
        {
            return (T)GetObject(type, parent, position, scale, rotation);
        }
        
        #endregion

        #region Reset Methods
        
        public bool ResetObject(IPoolable obj, bool removeFromActiveObjects = true)
        {
            ObjectPool pool = GetPool(obj.Type);
            if (pool != null)
            {
                pool.Push(obj, removeFromActiveObjects);
                return true;
            }
            Log.Error(("The object you are trying to reset doesn't exist in any pool ->", obj.Type), obj.GameObject);
            return false;
        }

        public void ResetAll(params ObjectType[] excludedTypes)
        {
            foreach (KeyValuePair<ObjectType, ObjectPool> pool in pools)
            {
                if (excludedTypes?.Length > 0 && Array.IndexOf(excludedTypes, pool.Key) > -1)
                {
                    continue;
                }
                pool.Value.Reset();
            }
        }
        
        #endregion
    }

    [Serializable]
    public class PoolPreset
    {
        public GameObject obj;
        public int initialSize;
    }
}
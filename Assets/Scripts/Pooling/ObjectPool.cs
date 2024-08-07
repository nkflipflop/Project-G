using System.Collections.Generic;
using Pooling.Interfaces;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

namespace Pooling
{
    public class ObjectPool
    {
        private readonly Stack<IPoolable> objects = new ();
        private readonly HashSet<IPoolable> activeObjects = new ();
        private readonly IPoolable poolObject;

        private int Count => objects.Count;
        private Transform PoolParent { get; set; }

        public ObjectPool(IPoolable poolObject, int numToSpawn = 0)
        {
            this.poolObject = poolObject;
            
            if (numToSpawn > 0)
            {
                Spawn(numToSpawn);
            }
        }

        private void Spawn(int number)
        {
            for (int i = 0; i < number; i++)
            {
                IPoolable poolableObj = CreateObject();
                objects.Push(poolableObj);
                poolableObj.GameObject.SetActive(false);
            }
        }

        private IPoolable CreateObject()
        {
            SetPoolParent();
            IPoolable poolableObj = (IPoolable)Object.Instantiate((Object)poolObject, PoolParent);
            return poolableObj;
            
            void SetPoolParent()
            {
                if (!PoolParent)
                {
                    Transform parent = new GameObject(poolObject.Type.ToString()).transform;
                    parent.SetParent(PoolFactory.instance.transform);
                    PoolParent = parent;
                }
            }
        }

        public IPoolable Pull(Transform parent = null, Vector3? position = null, Vector3? scale = null, Vector3? rotation = null)
        {
            IPoolable poolableObj = Count > 0 ? objects.Pop() : CreateObject();
            poolableObj.GameObject.SetActive(true);
            if (parent)
            {
                poolableObj.GameObject.transform.SetParent(parent);
            }
            poolableObj.GameObject.transform.localScale = scale ?? Vector3.one;
            poolableObj.GameObject.transform.position = position ?? Vector3.zero;
            poolableObj.GameObject.transform.localRotation =
                rotation.HasValue ? Quaternion.Euler(rotation.Value) : Quaternion.identity;
            poolableObj.OnSpawn();
            
            if (activeObjects.Add(poolableObj) == false)
            {
                Log.Error(("BUG: Trying to add the same object ->", poolableObj.Type), obj: poolableObj.GameObject);
            }
            return poolableObj;
        }

        public void Push(IPoolable obj, bool removeFromActiveObjects = true)
        {
            if (removeFromActiveObjects)
            {
                activeObjects.Remove(obj);
            }

            if (objects.Contains(obj))
            {
                Log.Error(("BUG: Trying to add the same object ->", obj.Type), obj: obj.GameObject);
                return;
            }
            objects.Push(obj);
            
            obj.OnReset();
            obj.GameObject.SetActive(false);
            obj.GameObject.transform.SetParent(PoolParent);
            obj.GameObject.transform.localPosition = Vector3.zero;
            obj.GameObject.transform.eulerAngles = Vector3.zero;
        }

        public void Reset()
        {
            foreach (IPoolable item in activeObjects)
            {
                item.ResetObject(false);
            }
            activeObjects.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using Pooling.Interfaces;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

namespace Pooling
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable
    {
        private Action<T> pullObject;
        private Action<T> pushObject;
        private Stack<T> objects = new ();
        private HashSet<T> activeObjects = new ();
        private GameObject prefab;

        public int Count => objects.Count;
        public Transform PoolParent { get; set; }

        public ObjectPool(PoolObject poolObject, int numToSpawn = 0)
        {
            prefab = poolObject.gameObject;
            
            if (numToSpawn > 0)
            {
                SetParent();
                Spawn(numToSpawn, PoolParent);
            }

            void SetParent()
            {
                if (PoolParent == null)
                {
                    Transform parent = new GameObject(poolObject.Type.ToString()).transform;
                    parent.SetParent(PoolFactory.instance.transform);
                    PoolParent = parent;
                }
            }
        }

        public ObjectPool(GameObject poolObject, Action<T> pullObject, Action<T> pushObject)
        {
            prefab = poolObject;
            this.pullObject = pullObject;
            this.pushObject = pushObject;
        }

        private void Spawn(int number, Transform parent = null)
        {
            for (int i = 0; i < number; i++)
            {
                T t = Object.Instantiate(prefab, parent).GetComponent<T>();
                objects.Push(t);
                t.gameObject.SetActive(false);
            }
        }

        #region Pull Functions
        
        public T Pull()
        {
            T t = Count > 0 ? objects.Pop() : Object.Instantiate(prefab, PoolParent).GetComponent<T>();
            
            t.gameObject.SetActive(true);
            t.Initialize(ReturnToPool);
            
            pullObject?.Invoke(t);
            if (activeObjects.Add(t) == false)
            {
                Log.Error(("BUG: Try to adding same object: ", t.Type), t.gameObject);
            }
            return t;
        }

        public T Pull(Transform parent)
        {
            T t = Pull();
            t.transform.SetParent(parent);
            return t;
        }
        
        public T Pull(Vector3 position)
        {
            T t = Pull();
            t.transform.position = position;
            return t;
        }
        
        public T Pull(Vector3 position, Quaternion rotation)
        {
            T t = Pull();
            t.transform.position = position;
            t.transform.rotation = rotation;
            return t;
        }
        
        #endregion

        public void Push(T t)
        {
            objects.Push(t);
            pushObject?.Invoke(t);
            t.gameObject.SetActive(false);
        }

        private void ReturnToPool(IPoolable obj)
        {
            if (obj is T t)
            {
                Push(t);
            }
            else
            {
                Log.Error(("Invalid object type:", obj,GetType().Name));
            }
        }
    }
}
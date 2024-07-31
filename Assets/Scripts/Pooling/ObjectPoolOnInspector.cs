#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Pooling.Interfaces;
using UnityEngine;

namespace Pooling
{
    [Serializable]
    public class ObjectPoolOnInspector
    {
        [HideInInspector] public string name;
        [HideInInspector] public ObjectType type;
        public ObjectPool pool;

        public List<GameObject> inActiveObjects = new();
        public List<GameObject> activeObjects = new();

        public ObjectPoolOnInspector(ObjectType type, ObjectPool pool)
        {
            this.type = type;
            this.pool = pool;
            name = type.ToString();
        }

        public void Refresh()
        {
            if (Application.isPlaying)
            {
                Stack<IPoolable> inActiveObjectsStack = GetFieldValue<Stack<IPoolable>>(pool, "objects");
                if (inActiveObjectsStack != null && inActiveObjectsStack.Count != inActiveObjects.Count)
                {
                    inActiveObjects.Clear();
                    foreach (IPoolable passiveObj in inActiveObjectsStack)
                    {
                        inActiveObjects.Add(passiveObj.GameObject);
                    }
                }
                
                HashSet<IPoolable> activeObjectsHashSet = GetFieldValue<HashSet<IPoolable>>(pool, "activeObjects");
                if (activeObjectsHashSet != null && activeObjectsHashSet.Count != activeObjects.Count)
                {
                    activeObjects.Clear();
                    foreach (IPoolable activeObj in activeObjectsHashSet)
                    {
                        activeObjects.Add(activeObj.GameObject);
                    }
                }
            }
        }
        
        public T GetFieldValue<T>(object obj, string name)
        {
            // Set the flags so that private and public fields from instances will be found
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var field = obj.GetType().GetField(name, bindingFlags);
            return (T)field?.GetValue(obj);
        }
    }
}
#endif
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    [Serializable]
    public class ObjectPoolOnInspector
    {
        public ObjectType type;
        public ObjectPool pool;

        public List<GameObject> activeObjects;      // TODO: fill this list with active objects in the pool
        public List<GameObject> passiveObjects;      // TODO: fill this list with passive objects in the pool

        public ObjectPoolOnInspector(ObjectType type, ObjectPool pool)
        {
            this.type = type;
            this.pool = pool;
        }
    }
}
#endif
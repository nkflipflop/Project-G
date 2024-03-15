#if UNITY_EDITOR
using System;

namespace Pooling
{
    [Serializable]
    public class ObjectPoolOnInspector
    {
        public ObjectType type;
        public ObjectPool pool;

        public ObjectPoolOnInspector(ObjectType type, ObjectPool pool)
        {
            this.type = type;
            this.pool = pool;
        }
    }
}
#endif
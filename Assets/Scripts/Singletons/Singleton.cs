using UnityEngine;

public class Singleton<Instance> : SingletonBase where Instance : Singleton<Instance>
{
    public static Instance instance;
    public bool isPersistent;

    public virtual void Awake()
    {
        if (isPersistent)
        {
            if (!instance || instance.gameObject == null)
            {
                instance = this as Instance;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
        else
        {
            if (!instance || instance.gameObject == null)
            {
                instance = this as Instance;
            }
        }
    }
	
    public override void ClearInstance()
    {
        instance = null;
    }
}

public abstract class SingletonBase : MonoBehaviour
{
    public abstract void ClearInstance();
}
using UnityEngine;

public abstract class SingletonClass<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance { get => instance; }

    protected virtual void Awake()
    {
        if (instance == null) instance = this as T;
        else Destroy(gameObject);

    }
}

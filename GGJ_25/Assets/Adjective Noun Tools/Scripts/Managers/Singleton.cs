using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// The static public reference for Instance to access it's variables. 
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
            }
            return instance;
        }
    }

    protected static T instance;
    [SerializeField, Tooltip("Destroy this gameobject when you change scenes or have it persist between scenes.")]
    private bool destroyBetweenScenes = false;
    [SerializeField, Tooltip("Any new instances will override this one and delete the old one.")]
    private bool allowOverrideInstances = false;
    protected void Awake()
    {
        if (instance == this || instance == null)
        {
            SetupInstance();
        }
        else if (allowOverrideInstances)
        {
            Destroy(instance.gameObject);
            SetupInstance();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Setup the instance to be a singleton. 
    /// </summary>
    private void SetupInstance()
    {
        instance = this as T;
        if (!destroyBetweenScenes)
        {
            transform.parent = null;
            DontDestroyOnLoad(this);
        }
        OnSetup();

    }

    /// <summary>
    /// Runs once the instance is setup so that it can have an awake equivalence 
    /// </summary>
    protected virtual void OnSetup()
    {

    }
}

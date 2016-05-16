﻿using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using UnityEditor;

public partial class ObjectPool : UnityEngine.MonoBehaviour
{
    public static ObjectPool Instance;
    
    Dictionary<System.Type, BaseMetaEntry> genericBasedPools = new Dictionary<System.Type, BaseMetaEntry>();

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    void OnLevelWasLoaded(int level)
    {
        genericBasedPools.Clear();
    }

    /// <summary>
    /// Releases an object back into the pool.
    /// </summary>
    /// <typeparam name="T">Object type to release</typeparam>
    /// <param name="obj">Object to release</param>
    public void Release<T>(T obj)
    {
        System.Type t = typeof(T);

        if (PoolContainsKey(t))
        {
            ((MetaEntry<T>)genericBasedPools[t]).Pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Acquires an object from the object pool
    /// </summary>
    /// <typeparam name="T">Type of object to acquire</typeparam>
    /// <returns>Acquired object</returns>
    public T Acquire<T>() where T : new()
    {
        System.Type t = typeof(T);

        MetaEntry<T> entry;

        if (!genericBasedPools.ContainsKey(t)) //Not using PoolContainsKey, as this is first instantiation
        {
            entry = new MetaEntry<T>();
            InstantiateObject<T>(entry);
            genericBasedPools.Add(t, entry);
        }
        else
        {
            entry = (MetaEntry<T>)genericBasedPools[t];
        }
        
        //Below threshold, make more instances
        if (entry.Pool.Count <= entry.LowerThreshold)
        {
            //Instantiate async if we aren't already
            if (entry.asyncInst == null) 
            {
                //Double the number of entries
                entry.LeftToInstantiate = entry.InstanceCountTotal;
                entry.InstanceCountTotal *= 2;

                //Start async instantiation
                entry.asyncInst = StartCoroutine(AsyncInstantiation<T>(entry));
            }

            //We need an instance immediatly, otherwise we will run out
            if (entry.Pool.Count <= 1)
            {
                InstantiateObject<T>(entry);
            }
        }

        return entry.Pool.Dequeue();
    }

    public void AddTypeToPool<T>(T template) where T : new ()
    {
        System.Type t = typeof(T);

        if (genericBasedPools.ContainsKey(t)) //Not using PoolContainsKey, as this is first instantiation
        {
            return;
        }

        MetaEntry<T> entry = new MetaEntry<T>();

        if (t.IsSubclassOf(typeof(UnityEngine.Object)))
        {
            entry.Original = template as UnityEngine.Object;
            entry.Pool.Enqueue(InstantiateUnityObject<T>(entry.Original));
        }
        else
        {
            entry.Pool.Enqueue(new T());
        }
    }

    private void InstantiateObject<T>(MetaEntry<T> entry) where T : new()
    {
        if (typeof(T).IsSubclassOf(typeof(UnityEngine.Object)))
        {
            if (entry.Pool.Count == 0)
            {
                entry.Original = InstantiateUnityObject<T>() as UnityEngine.Object;
                entry.Pool.Enqueue(InstantiateUnityObject<T>(entry.Original));
            }
            else
            {
                entry.Pool.Enqueue(InstantiateUnityObject<T>(entry.Original));
            }
        }
        else
        {
            entry.Pool.Enqueue(new T());
        }

        if (entry.LeftToInstantiate > 0)
            entry.LeftToInstantiate--;
    }

    private IEnumerator AsyncInstantiation<T>(MetaEntry<T> entry) where T : new()
    {
        while (entry.LeftToInstantiate > 0)
        {
            InstantiateObject<T>(entry);
            
            yield return new UnityEngine.WaitForEndOfFrame();
        }

        entry.asyncInst = null;
    }

    /// <summary>
    /// Instatiates a unity Object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T InstantiateUnityObject<T>(object obj = null)
    {
        object toCast;
        if (obj == null)
        {
            System.Type t = typeof(T);

            UnityEngine.Object[] arr = UnityEngine.Resources.LoadAll("", t);

            //Error if we didn't find anything
            if (arr == null || arr.Length == 0)
            {
                Debug.LogError(ErrorStrings.RESOURCE_NOT_FOUND);
                return default(T);
            }

            //Error if we found too much
            if (arr.Length > 1)
            {
                Debug.LogError(ErrorStrings.OBJECT_TYPE_MUST_BE_UNIQUE);
                return default(T);
            }

            toCast = arr[0];
        }
        else
        {
            toCast = UnityEngine.GameObject.Instantiate((UnityEngine.Object)obj);
        }

        T ret;
        if (TryCast<T>(toCast, out ret))
        {
            return ret;
        }
        else
        {
            return default(T);
        }
    }

    private bool TryCast<T>(object obj, out T result)
    {
        if (obj is T)
        {
            result = (T)obj;
            return true;
        }

        result = default(T);
        return false;
    }
    
    /// <summary>
    /// Sets the lower threshold for when instantiation of new objects should begin. Defaults to 1. If things are spawned often, i.e. bullets, you want to set this higher.
    /// </summary>
    /// <typeparam name="T">Object type to set threshold for</typeparam>
    /// <param name="threshold">The new lower threshold</param>
    public void SetLowerInstantiationThreshold<T>(int threshold)
    {
        System.Type t = typeof(T);

        if (threshold < 1)
        {
            Debug.LogError(ErrorStrings.THRESHOLD_TOO_LOW);
            return;
        }

        if (PoolContainsKey(t))
        {
            MetaEntry<T> entry = (MetaEntry<T>)genericBasedPools[t];
            entry.LowerThreshold = threshold;
        }
    }

    /// <summary>
    /// Gets the lower threshold for when instatiation of new objects should begin. Defaults to 1.
    /// </summary>
    /// <typeparam name="T">Object type to get threshold for</typeparam>
    /// <returns>The lower threshold, or -1 on fail</returns>
    public int GetLowerInstatiationThreshold<T>()
    {
        System.Type t = typeof(T);

        if (PoolContainsKey(t))
        {
            MetaEntry<T> entry = (MetaEntry<T>)genericBasedPools[t];
            return entry.LowerThreshold;
        }

        return -1;
    }

    /// <summary>
    /// Gets the total instance count of objects of a given type that exists, both in and out of the pool.
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    /// <returns>Instance count</returns>
    public int GetInstanceCountTotal<T>()
    {
        System.Type t = typeof(T);

        if (PoolContainsKey(t))
        {
            MetaEntry<T> entry = (MetaEntry<T>)genericBasedPools[t];
            return entry.InstanceCountTotal;
        }

        return -1;
    }

    private bool PoolContainsKey(System.Type t)
    {
        bool ret = genericBasedPools.ContainsKey(t);

        if (!ret)
        {
            Debug.LogError(ErrorStrings.OBJECT_TYPE_NOT_FOUND);
        }

        return ret;
    }

    abstract class BaseMetaEntry {};
    class MetaEntry<T> : BaseMetaEntry
    {
        public Queue<T> Pool = new Queue<T>();
        public int InstanceCountTotal = 1;
        public int LowerThreshold = 1;
        public int LeftToInstantiate = 0;
        public UnityEngine.Coroutine asyncInst;
        private UnityEngine.Object _original;

        public UnityEngine.Object Original
        {
            get 
            { 
                return _original; 
            }
            set 
            { 
                _original = value;
                System.Reflection.PropertyInfo pInfo = _original.GetType().GetProperty("gameObject");
                if (pInfo != null)
                {
                    UnityEngine.GameObject gameObjOriginal = (UnityEngine.GameObject)pInfo.GetValue(_original, null);

                    if (gameObjOriginal != null)
                    {
                        gameObjOriginal.SetActive(false);
                    }
                }
            }
        }
    }

    private static class ErrorStrings
    {
        public const string OBJECT_POOL_ERROR = "Object Pool ERROR";
        public const string OBJECT_TYPE_NOT_FOUND = OBJECT_POOL_ERROR + ": Object type not in pool.";
        public const string OBJECT_TYPE_MUST_BE_UNIQUE = OBJECT_POOL_ERROR + ": Object type must be unique.";
        public const string RESOURCE_NOT_FOUND = OBJECT_POOL_ERROR + ": Resource not found.";
        public const string THRESHOLD_TOO_LOW = OBJECT_POOL_ERROR + ": Threshold must be >= 1";
    }
}
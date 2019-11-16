using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceLocator : MonoBehaviour
{

    class UnityWeakReference<T> : System.WeakReference where T : UnityEngine.Object
    {
        public UnityWeakReference(T target) : base(target) { }

        public override bool IsAlive
        {
            get
            {
                UnityEngine.Object obj = Target as UnityEngine.Object;
                return obj != null;
            }
        }

        public new T Target
        {
            get
            {
                return base.Target as T;
            }
            set
            {
                base.Target = value;
            }
        }

        public bool TryGetTarget(out T target)
        {
            target = base.Target as T;
            return target != null;
        }
    }

    static ServiceLocator s_instance;

    Dictionary<System.Type, UnityWeakReference<MonoBehaviour>> m_dictionary = new Dictionary<System.Type, UnityWeakReference<MonoBehaviour>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void CreateInstance()
    {
        GameObject gameObject = new GameObject("ServiceLocator");
        s_instance = gameObject.AddComponent<ServiceLocator>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    private static void SceneUnloaded(Scene scene)
    {
        List<System.Type> removeKeys = new List<System.Type>(s_instance.m_dictionary.Count);

        foreach (var pair in s_instance.m_dictionary)
        {
            if (pair.Value.Target == null)
            {
                removeKeys.Add(pair.Key);
            }
            else
            {
            }
        }

        foreach (var key in removeKeys)
        {
            s_instance.m_dictionary.Remove(key);
        }
    }

    static public bool Add(System.Type type, MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour == null)
        {
            return false;
        }

        if (s_instance.m_dictionary.ContainsKey(type))
        {
            Debug.LogFormat("MonoBehaviourCache.Add() Already contains instance id of object. type:{0}", type);
            return false;
        }

        s_instance.m_dictionary.Add(type, new UnityWeakReference<MonoBehaviour>(monoBehaviour));
        return true;
    }

    static public bool Remove<T>()
    {
        return Application.isPlaying ? s_instance.m_dictionary.Remove(typeof(T)) : false;
    }

    static public T Get<T>() where T : MonoBehaviour
    {
        if (Application.isPlaying)
        {
            UnityWeakReference<MonoBehaviour> weakReference;
            System.Type type = typeof(T);

            if (!s_instance.m_dictionary.TryGetValue(type, out weakReference))
            {
                var obj = FindObjectOfType<T>();

                if (obj != null)
                {
                    Add(type, obj);
                    return obj;
                }

                return null;
            }

            return weakReference.Target as T;
        }
        else
        {
            return FindObjectOfType<T>();
        }
    }
}

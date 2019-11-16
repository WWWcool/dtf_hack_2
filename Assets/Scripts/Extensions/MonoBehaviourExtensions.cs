using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public static class MonoBehaviourExtensions
    {
        public static T GetCachedComponent<T>(this MonoBehaviour behaviour) where T : Component
        {
            return behaviour.gameObject.GetCachedComponent<T>();
        }

        public static T GetCachedComponentInChildren<T>(this MonoBehaviour behaviour) where T : Component
        {
            return behaviour.gameObject.GetCachedComponentInChildren<T>();
        }

        public static T GetCachedComponentInParent<T>(this MonoBehaviour behaviour) where T : Component
        {
            return behaviour.gameObject.GetCachedComponentInParent<T>();
        }

        public static void InvalidateCachedComponentInParent<T>(this MonoBehaviour behaviour) where T : Component
        {
            behaviour.gameObject.InvalidateCachedComponentInParent<T>();
        }

        public static void InvalidateCachedComponentInChildren<T>(this MonoBehaviour behaviour) where T : Component
        {
            behaviour.gameObject.InvalidateCachedComponentInChildren<T>();
        }

        public static void InvalidateCachedComponentsInParent(this MonoBehaviour behaviour, bool recursive = false)
        {
            behaviour.gameObject.InvalidateCachedComponentsInParent(recursive);
        }

        public static void InvalidateCachedComponentsInChildren(this MonoBehaviour behaviour, bool recursive = false)
        {
            behaviour.gameObject.InvalidateCachedComponentsInChildren(recursive);
        }
    }
}

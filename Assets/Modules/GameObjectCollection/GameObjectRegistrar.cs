using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class GameObjectRegistrar : MonoBehaviour
    {
        [SerializeField] private GameObjectCollection m_collection;

        public GameObjectCollection collection { get { return m_collection; } set { m_collection = value; } }
        public int objectIndex { get; private set; } = -1;

        private System.Action<int> m_onObjectRegistered;
        public event System.Action<int> onObjectRegistered
        {
            add
            {
                m_onObjectRegistered += value;

                if (objectIndex >= 0)
                    value(objectIndex);
            }
            remove
            {
                m_onObjectRegistered -= value;
            }
        }

        private void OnEnable()
        {
            objectIndex = m_collection.count;
            m_collection.Add(gameObject);
            m_onObjectRegistered?.Invoke(objectIndex);
        }

        private void OnDisable()
        {
            m_collection.Remove(gameObject);
        }
    }
}

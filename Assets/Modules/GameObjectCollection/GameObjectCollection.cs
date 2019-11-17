using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    [CreateAssetMenu(fileName = "GameObjectCollection", menuName = "Game/Runtime/Game object collection")]
    public class GameObjectCollection : ScriptableObject, IEnumerable<GameObject>
    {
        [SerializeField] private List<GameObject> m_objects;

        public System.Action OnCollectionChanged;
        public System.Action OnCollectionBecameEmpty;

        public int count { get { return m_objects.Count; } }
        public bool empty { get { return count <= 0; } }

        private void Awake()
        {
            if (m_objects == null)
                m_objects = new List<GameObject>();
        }

        public void Add(GameObject obj)
        {
            m_objects.Add(obj);
            InvokeChangeCallbacks(obj, true);
        }

        public void Remove(GameObject obj)
        {
            m_objects.Remove(obj);
            InvokeChangeCallbacks(obj, false);
        }

        public int GetObjectIndex(GameObject obj)
        {
            return m_objects.IndexOf(obj);
        }

        public GameObject FindClosest(Vector2 pos)
        {
            var closestDistance = float.PositiveInfinity;
            GameObject closestObject = null;

            foreach (var obj in m_objects)
            {
                var distance = Vector2.Distance(pos, obj.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }

            return closestObject;
        }

        public int aliveObjectsCount
        {
            get
            {
                int count = 0;

                foreach (var obj in m_objects)
                {
                    if (obj == null)
                        continue;

                    ++count;
                }

                return count;
            }
        }

        public Vector2 AveragePosition()
        {
            if (aliveObjectsCount <= 0)
            {
                Debug.LogError("No alive objects in collection!");
                return Vector2.zero;
            }

            var positionsSum = Vector2.zero;

            foreach (var obj in m_objects)
            {
                if (obj == null)
                    continue;

                positionsSum += (Vector2)obj.transform.position;
            }

            return positionsSum / aliveObjectsCount;
        }

        public GameObject GetAny()
        {
            if (m_objects.Count == 0)
                return null;

            return m_objects[0];
        }

        public GameObject GetAnyAlive()
        {
            foreach (var obj in m_objects)
                if (obj)
                    return obj;

            return null;
        }

        private void InvokeChangeCallbacks(GameObject obj, bool added)
        {
            OnCollectionChanged?.Invoke();

            if (m_objects.Count == 0)
                OnCollectionBecameEmpty?.Invoke();
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return m_objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

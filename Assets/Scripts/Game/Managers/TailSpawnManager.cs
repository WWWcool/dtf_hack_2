using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class TailSpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_tail = null;
        [SerializeField] private float m_decreaseTimeReload;
        [SerializeField] private float m_timeToDecrease;
        [SerializeField] private float m_minimumTime;
        private Field m_field => ServiceLocator.Get<Field>();

        public static float s_destroyTime = 2;

        private void OnEnable()
        {
            EventBus.Instance.AddListener<GameEvents.OnTileReached>(OnTileReached);
            EventBus.Instance.AddListener<GameEvents.OnDash>(SpawnTileDash);
            StartCoroutine(DestroyTimeDecrease());
        }

        private void OnDisable()
        {
            EventBus.Instance.RemoveListener<GameEvents.OnTileReached>(OnTileReached);
            EventBus.Instance.RemoveListener<GameEvents.OnDash>(SpawnTileDash);
            StopCoroutine(DestroyTimeDecrease());
        }

        private void OnTileReached(GameEvents.OnTileReached e)
        {
            if (!e.spawnTail)
                return;

            Instantiate(m_tail, e.currPosition, Quaternion.identity);
        }

        private void SpawnTileDash(GameEvents.OnDash e)
        {
            Vector2Int snappedStart = GetSnappedLocation(e.startPosition);
            Vector2Int snappedFinish = GetSnappedLocation(e.finishPosition);
            Vector2Int direction = GetSnappedLocation(e.direction);
            Vector2 delta = snappedStart - snappedFinish;
            while (delta.x + delta.y!=0)
            {
                if(direction.x + direction.y < 0 && m_field.IsPassable(snappedStart))
                    Instantiate(m_tail, m_field.GetPositionAtLocation(snappedStart), Quaternion.identity);
                snappedStart += direction;
                delta += direction;
                if (direction.x + direction.y > 0 && m_field.IsPassable(snappedStart))
                    Instantiate(m_tail, m_field.GetPositionAtLocation(snappedStart), Quaternion.identity);
            }
        }

        private IEnumerator DestroyTimeDecrease()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_decreaseTimeReload);
                if (s_destroyTime > m_minimumTime)
                {
                    s_destroyTime -= m_timeToDecrease;
                }
            }
        }

        private Vector2Int GetSnappedLocation(Vector2 location)
        {
            var loc = location;
            var x = (int)(loc.x);
            var y = (int)(loc.y);
            if (Mathf.Approximately(1.0f, Mathf.Repeat(loc.x, 1.0f))) x++;
            if (Mathf.Approximately(1.0f, Mathf.Repeat(loc.y, 1.0f))) y++;
            return new Vector2Int(x, y);
        }
    }
}


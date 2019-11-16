using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject m_tail = null;
    [SerializeField] private float m_decreaseTimeReload;
    [SerializeField] private float m_timeToDecrease;
    [SerializeField] private float m_minimumTime;

    public static float s_destroyTime = 2;

    private void OnEnable()
    {
        EventBus.Instance.AddListener<GameEvents.OnTileReached>(OnTileReached);
        StartCoroutine(DestroyTimeDecrease());
    }

    private void OnDisable()
    {
        EventBus.Instance.RemoveListener<GameEvents.OnTileReached>(OnTileReached);
        StopCoroutine(DestroyTimeDecrease());
    }

    private void OnTileReached(GameEvents.OnTileReached e)
    {
        if (!e.spawnTail)
            return;

        Instantiate(m_tail, e.currPosition, Quaternion.identity);
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
}

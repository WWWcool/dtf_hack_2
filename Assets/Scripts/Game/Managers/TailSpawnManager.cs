using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject m_tail = null;

    private void OnEnable()
    {
        EventBus.Instance.AddListener<GameEvents.OnTileReached>(OnTileReached);
    }

    private void OnDisable()
    {
        EventBus.Instance.RemoveListener<GameEvents.OnTileReached>(OnTileReached);
    }

    private void OnTileReached(GameEvents.OnTileReached e)
    {
        if (!e.spawnTail)
            return;

        Instantiate(m_tail, e.currPosition, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject m_tail = null;

    private void OnEnable()
    {
        EventBus.Instance.AddListener<GameEvents.OnTileReached>(SpawnTail);
    }

    private void OnDisable()
    {
        EventBus.Instance.RemoveListener<GameEvents.OnTileReached>(SpawnTail);
    }

    private void SpawnTail(GameEvents.OnTileReached e)
    {
        Instantiate(m_tail, e.currPosition,Quaternion.identity);
    }
}

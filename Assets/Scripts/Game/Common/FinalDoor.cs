using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class FinalDoor : MonoBehaviour
    {
        [SerializeField] private GameObject m_door = null;

        private void OnEnable()
        {
            EventBus.Instance.AddListener<GameEvents.OnGameOver>(OnGameOver);
        }

        private void OnDisable()
        {
            EventBus.Instance.RemoveListener<GameEvents.OnGameOver>(OnGameOver);
        }

        private void OnGameOver(GameEvents.OnGameOver e)
        {
            if (!e.won)
                return;

            m_door.SetActive(true);
        }
    }
}

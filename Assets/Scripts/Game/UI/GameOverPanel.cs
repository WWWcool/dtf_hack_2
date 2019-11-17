using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_panel;

        private void OnEnable()
        {
            EventBus.Instance.AddListener<GameEvents.OnGameOver>(OnGameOver);
        }

        private void OnDisable()
        {
            EventBus.Instance.RemoveListener<GameEvents.OnGameOver>(OnGameOver);
        }

        public void OnGameOver(GameEvents.OnGameOver e)
        {
            m_panel.SetActive(!e.won);
        }
    }
}

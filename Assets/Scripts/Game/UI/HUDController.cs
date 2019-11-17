using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject m_swipeHint;
    [SerializeField] private bool m_showSwipeHint = false;

    void Start()
    {
        m_swipeHint.SetActive(m_showSwipeHint);
        if (m_showSwipeHint)
        {
            PauseSystem.Instance.NeedPause(true);
        }
    }

    public void OnHintSkip()
    {
        m_swipeHint.SetActive(false);
        PauseSystem.Instance.NeedPause(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject m_swipeHint;
    [SerializeField] private bool m_showSwipeHint = false;
    [SerializeField] private Text m_chargeCount;

    void Start()
    {
        m_swipeHint.SetActive(m_showSwipeHint);
        if (m_showSwipeHint)
        {
            PauseSystem.Instance.NeedPause(true);
        }
        EventBus.Instance.AddListener<GameEvents.OnDashRecharge>(OnDashRecharge);
    }

    public void OnHintSkip()
    {
        m_swipeHint.SetActive(false);
        PauseSystem.Instance.NeedPause(false);
    }

    public void OnDashRecharge(GameEvents.OnDashRecharge e)
    {
        m_chargeCount.text = e.newCount.ToString();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityPrototype
{
    public class HealthMonitor : MonoBehaviour
    {

        [SerializeField] private Image m_bar;

        public void OnHealthDepleted()
        {
            gameObject.SetActive(false);
        }

        public void OnHealthChanged(float delta)
        {
        }

        public void OnUIChanged(float amount)
        {
            if (amount == 1)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                m_bar.fillAmount = amount;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class SlowmoManager : MonoBehaviour
    {
        [SerializeField] private float m_inputTimeScale = 0.1f;

        public void OnInputStarted()
        {
            ServiceLocator.Get<TimeManager>().ScaleTime(m_inputTimeScale);
        }
        
        public void OnInputFinished()
        {
            ServiceLocator.Get<TimeManager>().ScaleTime(1.0f);
        }
    }
}

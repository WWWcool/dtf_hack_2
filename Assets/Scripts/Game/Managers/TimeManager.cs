using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class TimeManager : MonoBehaviour
    {
        private float m_defaultFixedDeltaTime;

        private void Start()
        {
            m_defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        public void ScaleTime(float scale)
        {
            Time.timeScale = scale;
            Time.fixedDeltaTime = m_defaultFixedDeltaTime * scale;
        }
    }
}

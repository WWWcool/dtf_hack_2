using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class SoundEventEmitter : MonoBehaviour
    {
        [SerializeField] private SoundEvents.SoundType m_type;

        public void RaiseEvent()
        {
            if (m_type == SoundEvents.SoundType.None)
                return;

            EventBus.Instance.Raise(new SoundEvents.SoundEvent { type = m_type });
        }
    }
}

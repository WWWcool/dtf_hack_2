using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class DummyHealthMonitor : MonoBehaviour
    {
        public void OnHealthDepleted()
        {
            Debug.Log($"Health of object {gameObject.name} is depleted");
        }

        public void OnHealthChanged(float delta)
        {
            Debug.Log($"Health of object {gameObject.name} has changed by {delta}");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class RestartButton : MonoBehaviour
    {
        public void Restart()
        {
            ServiceLocator.Get<RestartManager>().RestartScene();
        }
    }
}

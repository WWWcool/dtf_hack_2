using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class FinalDoorTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            ServiceLocator.Get<SimpleSceneManager>().LoadNextScene();
        }
    }
}

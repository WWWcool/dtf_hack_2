using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class TailController : MonoBehaviour
    {

        private void OnEnable()
        {
            Destroy(gameObject, TailSpawnManager.s_destroyTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class FoodController : MonoBehaviour
    {
        public enum Size
        {
            small,
            midle,
            large,
        }
        [SerializeField]
        private Size size;
        [SerializeField]
        private float timeRegenSmall;
        [SerializeField]
        private float timeRegenMidle;
        [SerializeField]
        private float timeRegenLarge;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            float timeRegen = 0;
            if (collision.gameObject.tag == "Player")
            {
                switch (size)
                {
                    case Size.small:
                        timeRegen = timeRegenSmall;
                        break;
                    case Size.midle:
                        timeRegen = timeRegenMidle;
                        break;
                    case Size.large:
                        timeRegen = timeRegenLarge;
                        break;
                }
                TailSpawnManager.s_destroyTime += timeRegen;
                Destroy(gameObject);
            }
        }
    }
}
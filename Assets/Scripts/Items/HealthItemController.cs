using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class HealthItemController : MonoBehaviour
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
        private float healthRegenSmall;
        [SerializeField]
        private float healthRegenMidle;
        [SerializeField]
        private float healthRegenLarge;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            float HealthRegen = 0;
            if (collision.gameObject.tag == "Player")
            {
                switch (size)
                {
                    case Size.small:
                        HealthRegen = healthRegenSmall;
                        break;
                    case Size.midle:
                        HealthRegen = healthRegenMidle;
                        break;
                    case Size.large:
                        HealthRegen = healthRegenLarge;
                        break;
                }
                collision.gameObject.GetCachedComponentInParent<Health>().RegenerateHealth(HealthRegen);
                Destroy(gameObject);
            }
        }
    }
}

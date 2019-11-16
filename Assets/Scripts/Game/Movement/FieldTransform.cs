using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class FieldTransform : MonoBehaviour
    {
        private Field m_field => this.GetCachedComponentInParent<Field>();

        public Vector2 location
        {
            get
            {
                return m_field.GetLocationAtPosition(transform.position);
            }
            set
            {
                transform.position = m_field.GetPositionAtLocation(value);
            }
        }

        public void Snap()
        {
            var loc = location;
            loc.x = Mathf.RoundToInt(loc.x);
            loc.y = Mathf.RoundToInt(loc.y);
            location = loc;
        }
    }
}

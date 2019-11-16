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

        public Vector2Int snappedLocation
        {
            get
            {
                return GetSnappedLocation();
            }
        }

        public void Snap()
        {
            location = snappedLocation;
        }

        public Vector2Int GetSnappedLocation()
        {
            return GetSnappedLocation(location);
        }

        public Vector2Int GetSnappedLocation(Vector2 location)
        {
            var loc = location;
            var x = Mathf.RoundToInt(loc.x);
            var y = Mathf.RoundToInt(loc.y);

            return new Vector2Int(x, y);
        }
    }
}

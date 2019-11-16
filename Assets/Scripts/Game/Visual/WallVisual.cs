using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    [ExecuteInEditMode]
    public class WallVisual : MonoBehaviour
    {
        private void Update()
        {
            var wall = this.GetCachedComponentInParent<Wall>();
            var field = this.GetCachedComponentInParent<Field>();

            transform.localScale = field.tileSize;

            transform.position = field.GetPositionAtLocation(wall.centerLocation);
            transform.localScale = field.GetSize(wall.gridSize);
        }
    }
}

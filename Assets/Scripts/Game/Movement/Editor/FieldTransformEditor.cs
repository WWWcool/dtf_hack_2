using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityPrototype
{
    [CustomEditor(typeof(FieldTransform))]
    public class FieldTransformEditor : Editor
    {
        private FieldTransform m_target;

        private void OnEnable()
        {
            m_target = (FieldTransform)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Snap"))
                m_target.Snap();
        }
    }
}

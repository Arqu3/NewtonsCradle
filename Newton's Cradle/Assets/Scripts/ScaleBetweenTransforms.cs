using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsAssignments.Utility
{
    public class ScaleBetweenTransforms : MonoBehaviour
    {
        #region Exposed fields

        [SerializeField]
        Transform m_FirstTransform;
        [SerializeField]
        Transform m_SecondTransform;

        #endregion

        #region Private fields

        #endregion

        void Awake()
        {
            if (!m_FirstTransform || !m_SecondTransform)
            {
                enabled = false;
                return;
            }
        }

        void Update()
        {
            Vector3 pos = (m_FirstTransform.position + m_SecondTransform.position) / 2.0f;
            pos.z = 10.0f;
            transform.position = pos;
            Vector3 dir = m_FirstTransform.position - m_SecondTransform.position;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.down);
            Vector3 scale = transform.localScale;
            scale.z = dir.magnitude;
            transform.localScale = scale;
        }
    }
}

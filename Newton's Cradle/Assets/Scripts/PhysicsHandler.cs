using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsAssignments.Object;

namespace PhysicsAssignments.Handler
{
    public class PhysicsHandler : MonoBehaviour
    {
        #region Exposed fields

        [Header ("Physics settings")]
        [SerializeField]
        Vector3 m_Gravity = new Vector3 (0.0f, -9.82f, 0.0f);
        [SerializeField]
        bool m_ElasticSprings = true;

        #endregion

        #region Private fields

        Ball[] m_Balls;
        Spring[] m_Springs;

        #endregion

        private void Start ()
        {
            m_Balls = FindObjectsOfType<Ball> ();
            m_Springs = FindObjectsOfType<Spring> ();
        }

        private void FixedUpdate ()
        {
            for (int i = 0 ; i < m_Balls.Length ; ++i )
            {
                if ( !m_Balls[i].UseGravity && !m_Balls[i].UseSpringForce ) continue;

                m_Balls[i].CheckGround ();
                m_Balls[i].CheckCollisions (m_Balls);
                m_Balls[i].UpdateGravity (m_Gravity);
            }

            for (int i = 0 ; i < m_Springs.Length ; ++i )
            {
                m_Springs[i].UpdatePhysics (m_ElasticSprings);
            }
        }
    }
}
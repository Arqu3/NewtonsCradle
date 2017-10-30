using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsAssignments.Object;
using UnityEngine.UI;
using System.Linq;

namespace PhysicsAssignments.Handler
{
    public class PhysicsHandler : MonoBehaviour
    {
        #region Exposed fields

        [Header ("Physics settings")]
        [SerializeField]
        Vector3 m_Gravity = new Vector3 (0.0f, -9.82f, 0.0f);

        [Header ("UI elements")]
        [SerializeField]
        Text m_HeaderText;
        [SerializeField]
        GameObject m_OutlineCircle;

        [Header ("Spawnable Prefabs")]
        [SerializeField]
        GameObject[] m_Prefabs;

        #endregion

        #region Private fields

        List<Ball> m_Balls = new List<Ball>();
        List<Spring> m_Springs = new List<Spring> ();
        bool m_Active = true;
        Ball m_SelectedBall;
        GameObject m_CurrentPrefab;

        #endregion

        private void Awake ()
        {
            m_CurrentPrefab = Instantiate (m_Prefabs[0]);
        }

        private void Start ()
        {
            m_Balls = FindObjectsOfType<Ball> ().ToList ();
            m_Springs = FindObjectsOfType<Spring> ().ToList();
            m_OutlineCircle.SetActive (false);
        }

        private void Update ()
        {
            if ( Input.GetKeyDown (KeyCode.Tab) ) PauseSimulation ();
            else if ( Input.GetKeyDown (KeyCode.Escape) ) Application.Quit ();

            ActivateCircle (false);

            if ( m_Active ) return;

            if ( Input.GetKeyDown (KeyCode.Q) ) SpawnPrefab (m_Prefabs[0]);

            if ( Input.GetMouseButton (0) )
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                mousePos.z = 0.0f;
                float selectDist = Mathf.Infinity;
                foreach ( Ball ball in m_Balls )
                {
                    if ( !ball ) break;
                    Vector3 pos = ball.transform.position;
                    pos.z = 0.0f;

                    float dist = Vector3.Distance (pos, mousePos);
                    if ( dist < selectDist )
                    {
                        m_SelectedBall = ball;
                        selectDist = dist;
                    }
                }
            }
            else if (m_SelectedBall)
            {
                ActivateCircle (true);

                if ( Input.GetKeyDown (KeyCode.Alpha1) ) m_SelectedBall.SetMass (m_SelectedBall.Mass - 1f);
                else if ( Input.GetKeyDown (KeyCode.Alpha2) ) m_SelectedBall.SetMass (m_SelectedBall.Mass + 1f);
            }
        }

        /// <summary>
        /// Updates the outline circle appearing when clicking a circle
        /// </summary>
        /// <param name="active"></param>
        void ActivateCircle(bool active)
        {
            if (m_SelectedBall) m_OutlineCircle.transform.position = m_SelectedBall.transform.position;

            if ( m_OutlineCircle.activeSelf == active ) return;
            m_OutlineCircle.SetActive (active);
        }

        void SpawnPrefab(GameObject gObj)
        {
            if ( m_CurrentPrefab ) Destroy (m_CurrentPrefab);
            m_CurrentPrefab = Instantiate (gObj);
            m_Balls.Clear ();
            m_Balls = FindObjectsOfType<Ball> ().ToList();
            m_Springs.Clear ();
            m_Springs = FindObjectsOfType<Spring> ().ToList();
        }

        void PauseSimulation()
        {
            m_Active = !m_Active;
            if ( m_Active ) m_SelectedBall = null;
            m_HeaderText.text = m_Active ? "TAB to pause" : "PAUSED";
        }

        private void FixedUpdate ()
        {
            if ( !m_Active ) return;

            for (int i = 0 ; i < m_Balls.Count ; ++i )
            {
                if ( !m_Balls[i] ) break;
                if ( !m_Balls[i].gameObject.activeSelf ) continue;
                else if ( !m_Balls[i].UseGravity && !m_Balls[i].UseSpringForce ) continue;

                m_Balls[i].UpdateGravity (m_Gravity);
                m_Balls[i].CheckGround ();
                m_Balls[i].CheckCollisions (m_Balls);
            }

            for (int i = 0 ; i < m_Springs.Count ; ++i )
            {
                if ( !m_Springs[i] ) break;
                if ( !m_Springs[i].gameObject.activeSelf ) continue;
                m_Springs[i].UpdatePhysics ();
            }
        }
    }
}
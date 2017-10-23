using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace PhysicsAssignments.Object
{
    public class Ball : MonoBehaviour
    {
        #region Private exposed fields

        [Header("Mass (KG)")]
        [Range(0.0f, 200f)]
        [SerializeField]
        float m_Mass = 10.0f;

        [Header ("Initiation variables")]
        [SerializeField]
        Vector2 m_InitialForce = Vector2.zero;

        [Header("Physics variables")]
        [SerializeField]
        bool m_UseGravity = true;
        [SerializeField]
        bool m_UseSpringForce = true;

        #endregion

        #region Private fields

        private Vector3 m_ResetPos;
        private Vector3 m_StartVelo;

        private bool m_HitGround = false;
        private bool m_Active = false;

        private float m_Ground = 0.0f;

        private Body m_Body;
        float m_Radius = 0.0f;
        List<Ball> m_CollidedBalls = new List<Ball> ();
        Text m_Text;
        Spring m_Spring;

        #endregion

        void Start()
        {
            m_ResetPos = transform.position;
            m_Radius = GetComponent<SpriteRenderer> ().bounds.size.x / 2;
            m_Ground = m_Radius + 0.5f;
            m_Body = new Body(transform.position, Vector3.zero, Vector3.zero);
            m_Text = GetComponentInChildren<Text> ();

            if ( m_Text ) m_Text.text = m_Mass.ToString ();

            if (m_UseSpringForce)
            {
                if (transform.parent) m_Spring = transform.parent.GetComponentInChildren<Spring> ();
                if ( m_Spring ) m_Spring.K = -m_Mass;
            }

            AddForce (m_InitialForce);
        }

        private void LateUpdate ()
        {
            if ( m_CollidedBalls.Count > 0 ) m_CollidedBalls.Clear ();
        }

        public void UpdateGravity(Vector3 grav)
        {
            if ( m_UseGravity ) m_Body.Velocity += grav * Time.fixedDeltaTime;

            m_Body.Acceleration *= 0.999f;
            m_Body.Velocity *= 0.999f;
        }

        public void CheckGround()
        {
            if ( transform.position.y + m_Body.Velocity.y * Time.fixedDeltaTime > m_Ground )
            {
                transform.position += m_Body.Velocity * Time.fixedDeltaTime;
                m_HitGround = false;
            }
            else if ( transform.position.y + m_Body.Velocity.y * Time.fixedDeltaTime < m_Ground && !m_HitGround )
            {
                m_Body.Velocity = new Vector3 (m_Body.Velocity.x * 0.5f, m_Body.Velocity.y * -0.5f);
                m_HitGround = true;
                transform.position += m_Body.Velocity * Time.fixedDeltaTime;
                //if (m_body.Velocity.magnitude < 0.01f)
                //{
                //    m_body.Velocity = Vector3.zero;
                //    transform.position = new Vector3(transform.position.x, m_ground);

                //    m_body.Velocity += m_body.Acceleration * Time.fixedDeltaTime;
                //}
            }
        }

        public void CheckCollisions(List<Ball> balls)
        {
            for (int i = 0 ; i < balls.Count ; ++i )
            {
                if ( !balls[i] ) break;
                if ( balls[i] == this ) continue;
                if ( ( (transform.position + Velocity * Time.fixedDeltaTime ) - (balls[i].transform.position + balls[i].Velocity * Time.fixedDeltaTime ) ).magnitude - balls[i].Radius < m_Radius )
                {
                    if ( Collided (balls[i]) ) return;

                    AddToCollisions (balls[i]);
                    balls[i].AddToCollisions (this);

                    Vector3 new1 = ( Velocity * ( Mass - balls[i].Mass ) + ( 2f * balls[i].Mass * balls[i].Velocity ) ) / ( Mass + balls[i].Mass );
                    Vector3 new2 = ( balls[i].Velocity * ( balls[i].Mass - Mass ) + ( 2f * Mass * Velocity ) ) / ( Mass + balls[i].Mass );

                    if ( Vector3.Angle (new1, new2) < 90f ) new1 *= -1f;

                    AddForce (new1);
                    balls[i].AddForce (new2);

                    transform.position += new1 * Time.fixedDeltaTime;
                    m_Body.Position = transform.position;

                    balls[i].transform.position += new2 * Time.fixedDeltaTime;
                    balls[i].m_Body.Position = balls[i].transform.position;
                }
            }
        }

        public bool Collided(Ball ball)
        {
            return m_CollidedBalls.Contains (ball);
        }

        public void AddToCollisions(Ball ball)
        {
            m_CollidedBalls.Add (ball);
        }

        public void AddForce(Vector3 force)
        {
            m_Body.Velocity += force * 30f / m_Mass * Time.deltaTime;
        }

        public void Activate()
        {
            if (m_Active) return;

            m_Active = true;
            m_ResetPos = transform.position;
        }

        public void Reset()
        {
            transform.position = m_ResetPos;
            m_Body = new Body(transform.position, m_StartVelo, Vector3.zero);
            m_Active = false;
        }

        public float Mass
        {
            get { return m_Mass; }
        }

        public void SetMass(float mass)
        {
            m_Mass = Mathf.Clamp (mass, 1f, 100f);
            if ( m_Text ) m_Text.text = m_Mass.ToString ();
            if ( m_Spring ) m_Spring.K = -m_Mass;
        }

        public Vector3 Velocity
        {
            get { return m_Body.Velocity; }
        }

        public void ToggleGravity()
        {
            m_UseGravity = !m_UseGravity;
        }

        public void SetStartVelo(Vector3 v)
        {
            m_Body.Velocity = v;
            m_StartVelo = v;
        }

        public void AddSpringForce(Vector3 f)
        {
            if ( !m_UseSpringForce ) return;

            AddForce (f);
        }

        public bool UseGravity
        {
            get { return m_UseGravity; }
        }

        public bool UseSpringForce
        {
            get { return m_UseSpringForce; }
        }

        public float Radius
        {
            get { return m_Radius; }
        }
    }
}

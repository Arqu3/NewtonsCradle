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

            AddVelocity (m_InitialForce);
        }

        private void LateUpdate ()
        {
            if ( m_CollidedBalls.Count > 0 ) m_CollidedBalls.Clear ();
        }

        /// <summary>
        /// Applies gravity to the ball's velocity
        /// </summary>
        /// <param name="grav"></param>
        public void UpdateGravity(Vector3 grav)
        {
            if ( m_UseGravity ) m_Body.Velocity += grav * Time.fixedDeltaTime;

            m_Body.Velocity *= 0.985f;
        }

        /// <summary>
        /// Checks if a ball has touched the ground-plane
        /// </summary>
        public void CheckGround()
        {
            if ( transform.position.y + m_Body.Velocity.y * Time.fixedDeltaTime > m_Ground )
            {
                //transform.position += m_Body.Velocity * Time.fixedDeltaTime;
                m_HitGround = false;
            }
            else if ( transform.position.y + m_Body.Velocity.y * Time.fixedDeltaTime < m_Ground && !m_HitGround )
            {
                m_Body.Velocity = new Vector3 (m_Body.Velocity.x * 0.5f, m_Body.Velocity.y * -0.5f);
                m_HitGround = true;
            }
            transform.position += m_Body.Velocity * Time.fixedDeltaTime;
            m_Body.Position = transform.position;
        }

        /// <summary>
        /// Checks if this balls has collided with the specified balls this frame
        /// </summary>
        /// <param name="balls"></param>
        public void CheckCollisions(List<Ball> balls)
        {
            for (int i = 0 ; i < balls.Count ; ++i )
            {
                if ( !balls[i] ) break;
                if ( balls[i] == this ) continue;
                //Skip if balls have already collided this frame
                if ( Collided (balls[i]) ) continue;

                //Range check
                if ( ( (transform.position + Velocity * Time.fixedDeltaTime ) - (balls[i].transform.position + balls[i].Velocity * Time.fixedDeltaTime ) ).magnitude < m_Radius + balls[i].Radius )
                {
                    AddToCollisions (balls[i]);
                    balls[i].AddToCollisions (this);

                    //Calculate new forces
                    Vector3 force1 = ( Velocity * ( Mass - balls[i].Mass ) + ( 2f * balls[i].Mass * balls[i].Velocity ) ) / ( Mass + balls[i].Mass );
                    Vector3 force2 = ( balls[i].Velocity * ( balls[i].Mass - Mass ) + ( 2f * Mass * Velocity ) ) / ( Mass + balls[i].Mass );

                    //Check if any of the new forces needs to be inverted to help avoid getting stuck in each other
                    Vector3 pos = (transform.position + balls[i].transform.position) / 2.0f;
                    bool angle1 = Vector3.Angle (force1, pos - transform.position) <= 90.0f;
                    bool angle2 = Vector3.Angle (force2, pos - balls[i].transform.position) <= 90.0f;
                    if ( angle1 && !angle2 ) force1 *= -1.0f;
                    else if ( angle2 && !angle1 ) force2 *= -1.0f;

                    //Add forces
                    AddVelocity (force1);
                    balls[i].AddVelocity (force2);

                    //Offset balls to new position to help avoid colliding multiple times
                    transform.position += force1 * Time.fixedDeltaTime;
                    m_Body.Position = transform.position;

                    balls[i].transform.position += force2 * Time.fixedDeltaTime;
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

        public void AddVelocity(Vector3 vel)
        {
            m_Body.Velocity += vel * 30f / m_Mass * Time.deltaTime;
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

        public void AddSpringVelocity(Vector3 f)
        {
            if ( !m_UseSpringForce ) return;

            AddVelocity (f);
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

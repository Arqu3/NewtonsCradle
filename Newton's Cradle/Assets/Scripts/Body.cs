using UnityEngine;

namespace PhysicsAssignments
{
    public class Body
    {
        #region Private fields

        Vector3 m_Position;
        Vector3 m_Velocity;
        Vector3 m_Acceleration;

        private float m_Mass;

        #endregion

        /// <summary>
        /// Creates a new body with values = 0
        /// </summary>
        public Body()
        {
            m_Position = Vector3.zero;
            m_Velocity = Vector3.zero;
            m_Acceleration = Vector3.zero;
        }

        /// <summary>
        /// Creates a new body with specified values
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        public Body(Vector3 position, Vector3 velocity, Vector3 acceleration)
        {
            m_Position = position;
            m_Velocity = velocity;
            m_Acceleration = acceleration;
        }

        public Vector3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return m_Velocity;
            }
            set
            {
                m_Velocity = value;
            }
        }

        public Vector3 Acceleration
        {
            get
            {
                return m_Acceleration;
            }
            set
            {
                m_Acceleration = value;
            }
        }

        public float Mass
        {
            get { return m_Mass; }
            set { m_Mass = value; }
        }
    }
}

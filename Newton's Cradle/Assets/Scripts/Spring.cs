using PhysicsAssignments.Object;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private Ball[] m_Balls = new Ball[2];

    [SerializeField]
    float m_K = -1f;

    private float m_Distance = 1f;

    void Start()
    {
        m_Distance = Vector2.Distance(m_Balls [0].transform.position, m_Balls [1].transform.position);
    }

    public void UpdatePhysics(bool elastic)
    {
        if (elastic)
        {
            var x = Vector2.Distance (m_Balls[0].transform.position, m_Balls[1].transform.position);

            var p1 = ( m_Balls[0].transform.position - m_Balls[1].transform.position ).normalized;
            var p2 = ( m_Balls[1].transform.position - m_Balls[0].transform.position ).normalized;
            var force1 = -m_K * ( x - m_Distance ) * ( p2 / x );// - 0.01f * (m_balls[0].getVelo() - m_balls[1].getVelo());
            var force2 = -m_K * ( x - m_Distance ) * ( p1 / x );// - 0.01f * (m_balls[1].getVelo() - m_balls[0].getVelo());
            m_Balls[0].AddSpringForce (force1);
            m_Balls[1].AddSpringForce (force2);
        }
        else
        {
            m_Balls[1].transform.position = m_Balls[1].transform.position + Vector3.ClampMagnitude (m_Balls[1].transform.position - m_Balls[0].transform.position, m_Distance);
        }
    }

    public float K
    {
        get { return m_K; }
        set { m_K = value; }
    }
}

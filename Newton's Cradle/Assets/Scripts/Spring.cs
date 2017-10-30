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

    public void UpdatePhysics()
    {
        var x = Vector2.Distance (m_Balls[0].transform.position, m_Balls[1].transform.position);

        var p1 = ( m_Balls[0].transform.position - m_Balls[1].transform.position ).normalized;
        var p2 = ( m_Balls[1].transform.position - m_Balls[0].transform.position ).normalized;

        var x1 = ( x - m_Distance ) * ( p2 / x );
        var x2 = ( x - m_Distance ) * ( p1 / x );

        var force1 = -m_K * x1;
        var force2 = -m_K * x2;
        m_Balls[0].AddSpringForce (force1);
        m_Balls[1].AddSpringForce (force2);
    }

    public float K
    {
        get { return m_K; }
        set { m_K = value; }
    }
}

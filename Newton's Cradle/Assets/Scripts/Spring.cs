﻿using PhysicsAssignments.Object;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private Ball[] m_Balls = new Ball[2];

    [SerializeField]
    float m_K = -1f;

    private float m_RestingDistance = 1f;

    void Start()
    {
        m_RestingDistance = Vector2.Distance(m_Balls [0].transform.position, m_Balls [1].transform.position);
    }

    public void UpdatePhysics()
    {
        var x = Vector2.Distance (m_Balls[0].transform.position, m_Balls[1].transform.position);

        var p1 = ( m_Balls[0].transform.position - m_Balls[1].transform.position ).normalized;
        var p2 = ( m_Balls[1].transform.position - m_Balls[0].transform.position ).normalized;

        var x1 = ( x - m_RestingDistance ) * p2;
        var x2 = ( x - m_RestingDistance ) * p1;

        var force1 = -m_K * x1;
        var force2 = -m_K * x2;
        m_Balls[0].AddSpringVelocity (force1);
        m_Balls[1].AddSpringVelocity (force2);
    }

    public float K
    {
        get { return m_K; }
        set { m_K = value; }
    }
}

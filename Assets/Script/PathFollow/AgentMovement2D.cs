using System.Collections.Generic;
using UnityEngine;

public class AgentMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.1f;

    private Rigidbody2D rb;
    private List<Transform> pathPoints;
    private int currentPathIndex = 0;
    private bool isMoving = false;
    private DataAgent dataAgent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dataAgent = GetComponent<DataAgent>();
    }

    private void FixedUpdate()
    {
        if (!isMoving || pathPoints == null || currentPathIndex >= pathPoints.Count)
            return;

        Vector2 targetPosition = pathPoints[currentPathIndex].position;
        Vector2 direction = (targetPosition - rb.position).normalized;

        // Movimiento suave con aceleraci�n
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, direction * maxSpeed, acceleration * Time.fixedDeltaTime);

        // Rotaci�n opcional (si necesitas que gire)
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
        }

        // Verificaci�n de llegada al punto
        if (Vector2.Distance(rb.position, targetPosition) < stoppingDistance)
        {
            currentPathIndex++;
            if (currentPathIndex >= pathPoints.Count)
            {
                Stop();
            }
        }
    }

    public void FollowPath(List<Transform> points)
    {
        if (points == null || points.Count == 0) return;

        pathPoints = points;
        currentPathIndex = 0;
        isMoving = true;
        dataAgent.IsMoving = true;
    }

    public void Stop()
    {
        isMoving = false;
        rb.linearVelocity = Vector2.zero;
        dataAgent.IsMoving = false;
    }

    public bool IsDone() => !isMoving;
}


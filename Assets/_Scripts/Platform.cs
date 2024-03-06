using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float dir = 1;//either 1 or -1
    [SerializeField] private float acceleration;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 target1;
    [SerializeField] private Vector2 target2;
    private Vector2 currentTarget;
    private Vector2 startPos;
    enum PlatformState { Move, Wait};
    PlatformState state = PlatformState.Move;
    private void Start()
    {
        startPos = transform.position;
        target1 += startPos;
        target2 += startPos;

        if (dir < 0f)
        {
            currentTarget = target2;
        }
        else if (dir > 0f)
        {
            currentTarget = target1;
        }
    }

    void FixedUpdate()
    {
        if (state == PlatformState.Move)
        {
            MoveToNextTarget();
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, acceleration);
        }
    }

    void MoveToNextTarget()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, speed * (currentTarget - (Vector2)transform.position).normalized, acceleration);

        if (IsPointInArea(currentTarget, transform.position, 0.25f))
        {
            state = PlatformState.Wait;
        }
    }

    bool IsPointInArea(Vector2 position, Vector2 center, float radius)
    {
        return Vector2.Distance(position, center) < radius;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawLine((Vector3)target1 + transform.position, (Vector3)target2 + transform.position);
        Gizmos.DrawSphere((Vector3)target1 + transform.position, 0.25f);
        Gizmos.DrawSphere((Vector3)target2 + transform.position, 0.25f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && state == PlatformState.Wait)
        {
            if (currentTarget == target1)
            {
                currentTarget = target2;
            }
            else
            {
                currentTarget = target1;
            }
            dir *= -1f;
            state = PlatformState.Move;
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }

}

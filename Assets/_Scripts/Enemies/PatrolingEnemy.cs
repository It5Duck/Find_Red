using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PatrolingEnemy : Enemy, IGroundChecker
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ConstantForce2D cf;
    [SerializeField] private Vector2 target1;
    [SerializeField] private Vector2 target2;
    [SerializeField] private GravityDirection gravityDirection = GravityDirection.Down;
    [SerializeField] private float gravityScale;
    private Vector2 currentTarget;
    private Vector2 groundDir;
    private Vector2 startPos;
    private float angle;
    private float placeholder;
    private float groundDist;

    private void Start()
    {
        health = setHealth;
        startPos = transform.position;
        target1 += startPos;
        target2 += startPos;

        if (transform.localScale.x > 0f)
        {
            currentTarget = target2;
        }
        else if (transform.localScale.x < 0f)
        {
            currentTarget = target1;
        }

        switch (gravityDirection)
        {
            case GravityDirection.Down:
                cf.force = new Vector2(0f, -9.81f) * gravityScale;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;
            case GravityDirection.Left:
                cf.force = new Vector2(-9.81f, 0f) * gravityScale;
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
                break;
            case GravityDirection.Right:
                cf.force = new Vector2(9.81f, 0f) * gravityScale;
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                break;
            case GravityDirection.Up:
                cf.force = new Vector2(0f, 9.81f) * gravityScale;
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                break;
        }
    }
    private void FixedUpdate()
    {
        MoveToNextTarget();
        RotateInGroundDir();
    }
    void MoveToNextTarget()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, groundDir * speed * transform.localScale.x, acceleration);

        if(Math.Abs(cf.force.y) > 0f)
        {
            if (IsPointInArea(currentTarget.x, transform.position.x + transform.right.x * 0.25f, 0.25f))
            {
                ChangeTarget();
            }
        }
        else
        {
            if (IsPointInArea(currentTarget.y, transform.position.y + transform.right.x * 0.25f, 0.25f))
            {
                ChangeTarget();
            }
        }
    }
    void ChangeTarget()
    {
        if(currentTarget == target1)
        {
            currentTarget = target2;
        }
        else
        {
            currentTarget = target1;
        }
        transform.localScale = new Vector2(transform.localScale.x * -1f, 1.2f);
    }
    public void ChangeAngle(float angle, Vector2 dir)
    {
        this.angle = angle;
        groundDir = dir;
    }
    void RotateInGroundDir()
    {
        placeholder = 0f;
        float target = transform.eulerAngles.z;
        target = (target > 180) ? target - 360 : target;

        target = Mathf.SmoothDamp(target, angle, ref placeholder, 0.032f);
        transform.eulerAngles = new Vector3(0f, 0f, target);
    }
    bool IsPointInArea(float point, float center, float interval)
    {
        return Mathf.Abs(center-point) < interval;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawLine((Vector3)target1 + transform.position, (Vector3)target2 + transform.position);
        Gizmos.DrawSphere((Vector3)target1 + transform.position, 0.25f);
        Gizmos.DrawSphere((Vector3)target2 + transform.position, 0.25f);
    }
}

public interface IGroundChecker
{
    public void ChangeAngle(float angle, Vector2 dir);
}

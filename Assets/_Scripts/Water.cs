using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Water : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private SpriteShapeController ss;
    [SerializeField] private Vector2 firstTopPoint;
    [SerializeField] private Vector2 secondTopPoint;
    [SerializeField] private Vector2 firstBottomPoint;
    [SerializeField] private Vector2 secondBottomPoint;
    Spline spline;
    public int quality;

    private Vector3[] points;

    [Header("Physics Settings")]
    public float springConstant = 0.002f;
    public float damping = 0.02f;
    public float spread = 0.004f;
    public float impactMultiplier = 0.0082f;

    float[] velocities;
    float[] accelerations;
    float[] leftDeltas;
    float[] rightDeltas;

    private float timer;

    Vector2[] corners;
    float splineLength;

    void Start()
    {
        Initialize();
        CreateShape();
        BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    void Initialize()
    {
        velocities = new float[quality];
        accelerations = new float[quality];
        leftDeltas = new float[quality];
        rightDeltas = new float[quality];
        points = new Vector3[quality];
        corners = new Vector2[ss.spline.GetPointCount()];
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = ss.spline.GetPosition(i);
        }
        ss.spline.Clear();
    }

    void CreateShape()
    {
        float[] distances = new float[corners.Length];
        for (int i = 1; i < corners.Length; i++)
        {
            distances[i] = Vector2.Distance(corners[i % corners.Length], corners[i - 1]);
            splineLength += distances[i];
        }

        float segmentLength = Vector2.Distance(firstTopPoint, secondTopPoint) / quality;

        for (int i = 0; i < quality; i++)
        {
            points[i] = new Vector3(firstTopPoint.x + segmentLength * i, firstTopPoint.y);
        }

        ss.spline.InsertPointAt(0, firstBottomPoint);
        for (int i = 0; i < quality; i++)
        {
            ss.spline.InsertPointAt(i + 1, points[i]);
            ss.spline.SetHeight(i, 0f);
        }
        ss.spline.InsertPointAt(ss.spline.GetPointCount(), secondTopPoint);
        ss.spline.SetHeight(ss.spline.GetPointCount() - 1, 0f);
        ss.spline.InsertPointAt(ss.spline.GetPointCount(), secondBottomPoint);
        ss.spline.SetHeight(ss.spline.GetPointCount() - 1, 0f);
    }

    void Update()
    {
        if (timer <= 0) return;
        timer -= Time.deltaTime;

        for (int i = 0; i < quality; i++)
        {
            float force = springConstant * (points[i].y - firstTopPoint.y) + velocities[i] * damping;
            accelerations[i] = -force;
            points[i].y += velocities[i];
            velocities[i] += accelerations[i];
        }

        for (int i = 0; i < quality; i++)
        {
            if (i > 0)
            {
                leftDeltas[i] = spread * (points[i].y - points[i - 1].y);
                velocities[i - 1] += leftDeltas[i];
            }
            if (i < quality - 1)
            {
                rightDeltas[i] = spread * (points[i].y - points[i + 1].y);
                velocities[i + 1] += rightDeltas[i];
            }
        }
        for (int i = 0; i < quality; i++)
        {
            ss.spline.SetPosition(i + 1, points[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        Impact(collision, rb.velocity.y * impactMultiplier);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        Impact(collision, rb.velocity.y / 2f * impactMultiplier);
    }

    //Calculate the splash
    void Impact(Collider2D col, float force)
    {
        timer = 3f;
        float radius = col.bounds.max.x - col.bounds.min.x;
        Vector2 center = new Vector2(col.bounds.center.x, transform.position.y + firstTopPoint.y);

        for (int i = 0; i < quality; i++)
        {
            if (IsPointInArea(transform.position + points[i], center, radius))
            {
                velocities[i] = force;
            }
        }
    }

    bool IsPointInArea(Vector2 position, Vector2 center, float radius)
    {
        return Vector2.Distance(position, center) < radius;
    }
}

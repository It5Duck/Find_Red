using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    enum AttackStage { Move, Search, Attack, Think}; //Move: move to the position, Search: search for player
    [SerializeField] private float speed;
    [SerializeField] private float flightRadius;
    [SerializeField] private float size;
    [SerializeField] private float acceleration;
    [SerializeField] private float minDist;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GravityDirection downDirection = GravityDirection.Down;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private AudioSource source;
    private AttackStage attackStage = AttackStage.Move;
    private Vector2 targetPoint;
    private Vector2 origin;
    bool isTargetValid = false;

    private void Start()
    {
        origin = transform.position;

        switch (downDirection)
        {
            case GravityDirection.Down:
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;
            case GravityDirection.Left:
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
                break;
            case GravityDirection.Right:
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                break;
            case GravityDirection.Up:
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                break;
        }
    }
    private void Update()
    {
        if (attackStage == AttackStage.Move)
        {
            if (isTargetValid)
            {
                MoveToTarget();
            }
            else
            {
                FindValidPosition();
            }
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, acceleration);
            if (attackStage == AttackStage.Search)
            {
                StartCoroutine(Search());
            }
        }
    }

    void MoveToTarget()
    {
        if (IsPointInArea(targetPoint, transform.position, 0.05f))
        {
            attackStage = AttackStage.Search;
            isTargetValid = false;
        }
        Vector2 dir = ((Vector3)targetPoint - transform.position).normalized;
        rb.velocity = Vector2.Lerp(rb.velocity, dir * speed, acceleration);
    }
    void FindValidPosition()
    {
        Vector2 pos = origin + (flightRadius * UnityEngine.Random.insideUnitCircle); ;
        isTargetValid = IsPositionValid(pos);
        if (isTargetValid)
        {
            targetPoint = pos;
            attackStage = AttackStage.Move;
        }
    }

    bool IsPositionValid(Vector2 pos)
    {
        return !Physics2D.OverlapCircle(pos, size, ground) && Vector2.Distance(pos, targetPoint) > minDist;
    }
    IEnumerator Attack(Transform target)
    {
        if(attackStage == AttackStage.Attack)
        {
            attackStage = AttackStage.Think;
            yield return new WaitForSeconds(1f);
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetDirection((target.position - transform.position).normalized);
            source.Play();
            yield return new WaitForSeconds(1f);
            attackStage = AttackStage.Move;
        }
        yield return null;
    }

    IEnumerator Search()
    {
        attackStage = AttackStage.Think;
        yield return new WaitForSeconds(1f);
        RaycastHit2D gr = Physics2D.Raycast(origin, -transform.up, 25f, ground);
        if (gr.collider != null)
        {
            RaycastHit2D player = Physics2D.CircleCast(origin, flightRadius, -transform.up, gr.distance, playerLayer);
            if (player.collider != null)
            {
                attackStage = AttackStage.Attack;
                StartCoroutine(Attack(player.collider.transform));
            }
            else
            {
                attackStage = AttackStage.Move;
            }
        }
        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(origin, flightRadius);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, flightRadius);
        }
    }

    bool IsPointInArea(Vector2 point, Vector2 center, float radius)
    {
        return Vector2.Distance(point, center) < radius;
    }
}

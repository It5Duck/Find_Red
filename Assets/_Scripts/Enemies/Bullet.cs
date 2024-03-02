using System.Collections;
using UnityEngine;
[RequireComponent (typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask ignore;
    [SerializeField] private float damage;//should be negative
    [SerializeField] private float speed;
    [SerializeField] private float lifetime; //in seconds
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb;

    private IEnumerator Start()
    {
        rb = GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(lifetime);
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(direction == Vector2.zero)
        {
            return;
        }
        else
        {
            rb.velocity = direction * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == ignore.value)
        {
            return;
        }
        if(collision is IDamageable)
        {
            ((IDamageable)collision).ChangeHealth(damage);
        }
        //Destroy(gameObject);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }
}

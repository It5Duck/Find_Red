using System.Collections;
using UnityEngine;
[RequireComponent (typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask ignore;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime; //in seconds
    [SerializeField] private AudioClip impact;
    [SerializeField] private SoundSetter setter;
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
        if(((1 << collision.gameObject.layer) & ignore) != 0)
        {
            return;
        }
        else
        {
            IDamageable d;
            if (collision.gameObject.TryGetComponent<IDamageable>(out d))
            {
                d.ChangeHealth(-damage);
            }
            Instantiate(setter).SetSound(impact);
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject, 0.05f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & ignore) != 0)
        {
            return;
        }
        else
        {
            IDamageable d;
            if (collision.gameObject.TryGetComponent<IDamageable>(out d))
            {
                d.ChangeHealth(-damage);
            }
            Instantiate(setter).SetSound(impact);
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject, 0.05f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    public IEnumerator ShootTimes(int times, float rate, Transform target)
    {
        for (int i = 0; i < times; i++)
        {
            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.SetDirection((target.position - transform.position).normalized);
            yield return new WaitForSeconds(rate);
        }
        Destroy(gameObject);
    }

    public IEnumerator ShootTimes(int times, float rate, Vector2 dir)
    {
        for (int i = 0; i < times; i++)
        {
            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.SetDirection(dir);
            yield return new WaitForSeconds(rate);
        }
        Destroy(gameObject);
    }

    public IEnumerator ShootTimesAfter(int times, float rate, Vector2 dir, float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < times; i++)
        {
            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.SetDirection(dir);
            yield return new WaitForSeconds(rate);
        }
        Destroy(gameObject);
    }

    public IEnumerator ShootTimesAfter(int times, float rate, Transform target, float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < times; i++)
        {
            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.SetDirection((target.position - transform.position).normalized);
            yield return new WaitForSeconds(rate);
        }
        Destroy(gameObject);
    }
}

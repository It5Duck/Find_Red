using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IHarmful
{
    public GameObject diePrefab;
    public int damage { get; set; }
    public float health { get; set; }
    public float setHealth;
    public void ChangeHealth(float amount)
    {
        
        health += amount;
        if(health <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        Instantiate(diePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

public interface IDamageable
{
    public float health { get; set; }

    public void ChangeHealth(float amount);
}
public interface IHarmful
{
    public int damage { get; set; }
}


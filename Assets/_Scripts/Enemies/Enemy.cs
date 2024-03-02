using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IHarmful
{
    public int damage { get; set; }
    public float health { get; set; }
    public void ChangeHealth(float amount)
    {
        health += amount;
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


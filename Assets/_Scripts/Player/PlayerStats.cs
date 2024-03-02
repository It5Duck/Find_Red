using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxhealth;
    [SerializeField] private float damage;
    public int health { get; set; }
    private int currentSceneIndex;
    public static PlayerStats instance;
    private void Start()
    {
        instance = this;
        EventManager.instance.OnPlayerDamaged += ChangeHealth;
    }

    public void ChangeHealth(int amount)
    {
        int sum = amount + health;
        if(sum > maxhealth)
        {

        }
        else
        {
            health += amount;
        }
    }
}
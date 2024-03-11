using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxhealth;
    [SerializeField] private float damage;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private Checkpoint current;
    [SerializeField] private GameObject hpContainer;
    [SerializeField] private GameObject hpPrefab;
    [SerializeField] private GameObject fade;

    private List<GameObject> hps;//health points
    public int health { get; set; }
    private int currentSceneIndex;
    public static PlayerStats instance;
    private IEnumerator Start()
    {
        health = maxhealth;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].index = i;
        }
        instance = this;
        yield return new WaitForSeconds(0.1f);
        EventManager.instance.OnPlayerDamaged += ChangeHealth;
    }

    public void ChangeHealth(int amount, bool doRespawn)
    {
        int sum = amount + health;
        if(sum > maxhealth)
        {
            health = maxhealth;
        }
        else
        {
            health += amount;
        }
        if(amount < 0)
        {
            for (int i = 0; i < Mathf.Abs(amount); i++)
            {
                //StartCoroutine(RemoveHP());
            }
            if(doRespawn)
            {
                StartCoroutine(Respawn());
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                hps.Add(Instantiate(hpPrefab, hpContainer.transform));
            }
        }
        if(health <= 0)
        {
            PlayerDied();
        }
    }

    public void PlayerDied()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CheckpointEntered(int index)
    {
        if(index > current.index)
        {
            current = checkpoints[index];
        }
    }

    IEnumerator RemoveHP()
    {
        LeanTween.moveLocalY(hps[hps.Count - 1], -50f, 1f);
        yield return new WaitForSeconds(1f);
        hps.Remove(hps[hps.Count - 1]);
    }

    IEnumerator Respawn()
    {
        fade.GetComponent<Fade>().FadeIn(1f);
        gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        gameObject.SetActive(true);
        transform.position = current.transform.position;
    }
}
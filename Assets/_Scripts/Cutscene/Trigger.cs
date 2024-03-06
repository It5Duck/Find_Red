using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] AudioClip clip;
    [SerializeField] Transform bg;
    [SerializeField] GameObject fade;
    enum TriggerType {Cutscene, Falling, NextLVL}
    [SerializeField] TriggerType type = TriggerType.Cutscene;
    public int index;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(type == TriggerType.Cutscene)
            {
                EventManager.instance.CutsceneTriggered(index);
            }
            else if (type == TriggerType.Falling)
            {
                StartCoroutine(Fall());
            }
            else if(type == TriggerType.NextLVL)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    IEnumerator Fall()
    {
        SoundManager.instance.PlaySong(clip);
        GameObject.Find("Creature").SetActive(false);
        yield return new WaitForSeconds(6.2f);
        fade.SetActive(true);
        cam.m_Follow = bg;
        bg.GetComponent<Scroll>().scroll = true;
        fade.GetComponent<Image>().color = new Color(0f,0f,0f,1f);
        yield return new WaitForSeconds(0.68f);
        fade.SetActive(false);
        yield return new WaitForSeconds(16f);
        fade.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        fade.SetActive(true);
        fade.GetComponent<Fade>().FadeIn();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

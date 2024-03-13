using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }
    void Start()
    {
        instance = this;
    }

    public event Action<bool> OnGroundedStateChanged;
    public void GroundedChanged(bool isGrounded)
    {
        if(OnGroundedStateChanged != null)
            OnGroundedStateChanged(isGrounded);
    }

    public event Action OnPlayerShot;
    public void PlayerShot()
    {
        if (OnPlayerShot != null)
        {
            OnPlayerShot();
        }
    }

    public event Action<int, bool> OnPlayerDamaged;
    public void PlayerDamaged(int damage, bool doRespawn)
    {
        if(OnPlayerDamaged != null)
        {
            OnPlayerDamaged(damage, doRespawn);
        }
    }

    public event Action<int> OnCutsceneTriggered;
    public void CutsceneTriggered(int index)
    {
        if(OnCutsceneTriggered != null)
        {
            OnCutsceneTriggered(index);
        }
    }

    public event Action<Sprite, string> OnEmotionChanged;
    public void EmotionChanged(Sprite emotion, string characterName)
    {
        if(OnEmotionChanged != null)
        {
            OnEmotionChanged(emotion, characterName);
        }
    }

    public event Action OnCutsceneExited;
    public void CutsceneExited()
    {
        if (OnEmotionChanged != null)
        {
            OnCutsceneExited();
        }
    }

    public event Action<int> OnMovedToNextCutpoint;
    public void MovedToNextCutpoint(int indexTo)
    {
        if(OnMovedToNextCutpoint != null)
        {
            OnMovedToNextCutpoint(indexTo);
        }
    }

    public event Action OnPlayerDeath;
    public void PlayerDeath()
    {
        if(OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }
    }

    public void DoRespawn(GameObject gameObject, Fade fade)
    {
        StartCoroutine(Respawn(gameObject, fade));
    }
    IEnumerator Respawn(GameObject player, Fade fade)
    {
        yield return new WaitForSeconds(1.1f);
        fade.GetComponent<Fade>().FadeOut();
        player.SetActive(true);
        player.GetComponent<PlayerInput>().enabled = true;

    }
}

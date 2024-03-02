using System;
using UnityEngine;

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

    public event Action<float, Vector2> OnAngleChanged;
    public void AngleChanged(float angle, Vector2 dir)
    {
        if(OnAngleChanged != null)
            OnAngleChanged(angle, dir);
    }

    public event Action OnPlayerShot;
    public void PlayerShot()
    {
        if (OnPlayerShot != null)
        {
            OnPlayerShot();
        }
    }

    public event Action<int> OnPlayerDamaged;
    public void PlayerDamaged(int damage)
    {
        if(OnPlayerDamaged != null)
        {
            OnPlayerDamaged(damage);
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

}

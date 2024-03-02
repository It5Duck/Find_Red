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

    public event Action<float> OnAngleChanged;
    public void AngleChanged(float angle)
    {
        if(OnAngleChanged != null)
            OnAngleChanged(angle);
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
}

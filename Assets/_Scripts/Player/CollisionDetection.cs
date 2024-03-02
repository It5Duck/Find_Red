using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is IHarmful)
        {
            EventManager.instance.PlayerDamaged(((IHarmful)collision).damage);
        }
        else if (collision.gameObject.CompareTag("Cutscene"))
        {
            EventManager.instance.CutsceneTriggered(collision.GetComponent<Trigger>().index);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision is IHarmful)
        {
            EventManager.instance.PlayerDamaged(((IHarmful)collision).damage);
        }
    }
}

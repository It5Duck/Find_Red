using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private Vector2 teleportOffset;
    [SerializeField] private GroundDetector gd;
    [SerializeField] private SortingLayer layer;
    [SerializeField] private ConstantForce2D cf;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is IHarmful)
        {
            if (collision.gameObject.CompareTag("Respawn"))
            {
                EventManager.instance.PlayerDamaged(-((IHarmful)collision).damage, true);
            }
            else if (collision.gameObject.CompareTag("Harmful"))
            {
                EventManager.instance.PlayerDamaged(-((IHarmful)collision).damage, false);
            }
        }
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            EventManager.instance.PlayerDamaged(-1, true);
        }
        else if (collision.gameObject.CompareTag("Cutscene"))
        {
            EventManager.instance.CutsceneTriggered(collision.GetComponent<Trigger>().index);
        }
        else if (collision.gameObject.CompareTag("Harmful"))
        {
            EventManager.instance.PlayerDamaged(-1, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision is IHarmful)
        {
            if (collision.gameObject.CompareTag("Respawn"))
            {
                EventManager.instance.PlayerDamaged(-((IHarmful)collision).damage, true);
            }
            else if (collision.gameObject.CompareTag("Harmful"))
            {
                EventManager.instance.PlayerDamaged(-((IHarmful)collision).damage, false);
            }
        }
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            EventManager.instance.PlayerDamaged(-1, true);
        }
        else if (collision.gameObject.CompareTag("Door"))
        {
            Teleport(collision.gameObject.GetComponentInParent<Door>().link, collision.gameObject.GetComponentInParent<Door>());
        }
        else if (collision.gameObject.CompareTag("Harmful"))
        {
            EventManager.instance.PlayerDamaged(-1, false);
        }
    }

    void Teleport(Door target, Door sender)
    {
        switch (target.direction)
        {
            case GravityDirection.Down:
                cf.force = new Vector2(0f, -9.81f);
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;
            case GravityDirection.Left:
                cf.force = new Vector2(-9.81f, 0f);
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
                break;
            case GravityDirection.Right:
                cf.force = new Vector2(9.81f, 0f);
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                break;
            case GravityDirection.Up:
                cf.force = new Vector2(0f, 9.81f);
                transform.eulerAngles = new Vector3(0f, 0f, -180f);
                break;
        }
        if(sender.gameObject.layer != target.gameObject.layer)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), sender.gameObject.layer, true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), target.gameObject.layer, false);
            gd.groundMask = 1 << target.gameObject.layer;
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < srs.Length; i++)
            {
                srs[i].sortingLayerID = target.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
            }
        }
        transform.position = target.transform.position + (Vector3)teleportOffset;
    }
}

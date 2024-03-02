using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private Vector2 teleportOffset;
    [SerializeField] private ConstantForce2D cf;
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
        else if (collision.gameObject.CompareTag("Door"))
        {
            Teleport(collision.gameObject.GetComponentInParent<Door>().link);
        }
    }

    void Teleport(Door target)
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
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                break;
        }
        transform.position = target.transform.position - (target.transform.right + (Vector3)teleportOffset * target.transform.localScale.x);
    }
}

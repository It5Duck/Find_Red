using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.U2D;

public class God : MonoBehaviour, IDamageable
{
    public float health { get; set; }
    [SerializeField] Transform player;
    [SerializeField] Transform mapCenter;
    [SerializeField] Angel angelPrefab;
    [SerializeField] Laser laserPrefab;
    [SerializeField] float ringRadius;
    [SerializeField] CinemachineImpulseSource imp;
    [SerializeField] Camera cam;
    [SerializeField] SpriteShapeRenderer environment;

    [SerializeField] Transform target1;
    [SerializeField] Transform target2;

    enum AttackStage { Resting, Thinking, Angels, Colorlaser, Angels2, Angels3, Skylasers}
    AttackStage stage = AttackStage.Thinking;

    private void Update()
    {
        if (stage == AttackStage.Thinking)
        {
            SelectRandomAttack();
        }
        else if (stage == AttackStage.Angels)
        {
            for (int i = 0; i < 7; i++)
            {
                Angel a = Instantiate(angelPrefab, new Vector3(transform.position.x + (i -3) * 2.5f, transform.position.y, 0f), Quaternion.identity);
                StartCoroutine(a.ShootTimes(5, 0.5f, player));
            }
            StartCoroutine(RestFor(4f));
        }
        else if (stage == AttackStage.Colorlaser)
        {
            Laser l = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            StartCoroutine(l.GodLaser(transform.position, player.position));
            imp.GenerateImpulse();
            StartCoroutine(RestFor(3f));
        }
        else if(stage == AttackStage.Angels2)
        {
            Angel a1 = Instantiate(angelPrefab, target1.position, Quaternion.identity);
            StartCoroutine(a1.ShootTimesAfter(5, 1f, Vector2.right, 1.5f));
            Angel a2 = Instantiate(angelPrefab, target2.position, Quaternion.identity);
            StartCoroutine(a2.ShootTimesAfter(5, 1f, Vector2.left, 1.5f));
            StartCoroutine(RestFor(8f));
        }
        else if( stage == AttackStage.Angels3)
        {
            for (int i = 0; i < 20; i++)
            {
                float x = ringRadius * Mathf.Cos(20 * i * Mathf.Deg2Rad);
                float y = ringRadius * Mathf.Sin(20 * i * Mathf.Deg2Rad);
                Vector2 pos = new Vector2(x, y) * ringRadius;
                Angel a = Instantiate(angelPrefab, pos, Quaternion.identity);
                StartCoroutine(a.ShootTimesAfter(3, 2f, mapCenter, 0.1f * i));
            }
            StartCoroutine(RestFor(10f));
        }
        else if (stage == AttackStage.Skylasers)
        {
            for (int i = 0; i < 7; i++)
            {
                Laser l = Instantiate(laserPrefab, new Vector3(transform.position.x + (i - 3)*2.5f, transform.position.y, 0f), Quaternion.identity);
                StartCoroutine(l.GodLaser2(new Vector3(transform.position.x + (i - 3) * 2.5f, 30, 0f), new Vector3(transform.position.x + (i - 3) * 2.5f, -30, 0f)));
            }
            StartCoroutine(RestFor(3f));
        }
    }

    void SelectRandomAttack()
    {
        //stage = (AttackStage)Random.Range(2, 7);
        stage = AttackStage.Colorlaser;
    }

    IEnumerator RestFor(float seconds)
    {
        stage = AttackStage.Resting;
        yield return new WaitForSeconds(seconds);
        stage = AttackStage.Thinking;
    }

    public void ChangeHealth(float amount)
    {

    }
}

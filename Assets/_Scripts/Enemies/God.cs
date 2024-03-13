using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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
    [SerializeField] Transform eyeLid;
    [SerializeField] AudioClip music;
    [SerializeField] private GameObject death;
    [SerializeField] private AudioClip die;
    [SerializeField] private SoundSetter soundPrefab;

    [SerializeField] Transform target1;
    [SerializeField] Transform target2;
    [SerializeField] Slider healthBar;

    enum AttackStage { Resting, Thinking, Angels, Colorlaser, Angels2, Angels3, Skylasers }
    AttackStage stage = AttackStage.Resting;
    private void Start()
    {
        health = 24f;
    }
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
                bool sound = i<1?true:false;
                Angel a = Instantiate(angelPrefab, new Vector3(transform.position.x + (i - 3) * 2.5f, transform.position.y, 0f), Quaternion.identity);
                StartCoroutine(a.ShootTimesAfter(5, 0.5f, player, 1f, sound));
            }
            StartCoroutine(RestFor(5f));
        }
        else if (stage == AttackStage.Colorlaser)
        {
            StartCoroutine(ColorLaser());
            StartCoroutine(RestFor(3f));
        }
        else if (stage == AttackStage.Angels2)
        {
            Angel a1 = Instantiate(angelPrefab, target1.position, Quaternion.identity);
            StartCoroutine(a1.ShootTimesAfter(5, 1f, Vector2.right, 1.5f, true));
            Angel a2 = Instantiate(angelPrefab, target2.position, Quaternion.identity);
            StartCoroutine(a2.ShootTimesAfter(5, 1f, Vector2.left, 1.5f, false));
            StartCoroutine(RestFor(8f));
        }
        else if (stage == AttackStage.Angels3)
        {
            for (int i = 0; i < 20; i++)
            {
                float x = ringRadius * Mathf.Cos(20 * i * Mathf.Deg2Rad);
                float y = ringRadius * Mathf.Sin(20 * i * Mathf.Deg2Rad);
                Vector2 pos = new Vector2(x, y) * ringRadius;
                Angel a = Instantiate(angelPrefab, pos, Quaternion.identity);
                StartCoroutine(a.ShootTimesAfter(3, 2f, mapCenter, 0.1f * i, true));
            }
            StartCoroutine(RestFor(10f));
        }
        else if (stage == AttackStage.Skylasers)
        {
            StartCoroutine(SkyLasers());
            StartCoroutine(RestFor(3f));
        }
    }

    void SelectRandomAttack()
    {
        stage = (AttackStage)Random.Range(2, 7);
    }

    IEnumerator RestFor(float seconds)
    {
        stage = AttackStage.Resting;
        yield return new WaitForSeconds(seconds);
        stage = AttackStage.Thinking;
    }

    IEnumerator ColorLaser()
    {
        LeanTween.moveLocalY(eyeLid.gameObject, 0, 0.75f);
        LeanTween.color(cam.gameObject, new Color(0, 0, 0, 1), 1f).setOnUpdate((Color val) =>
        {
            Color c = cam.backgroundColor;
            c = val;
            cam.backgroundColor = c;
        });
        LeanTween.color(environment.gameObject, new Color(0, 0, 0, 1), 0.75f);
        yield return new WaitForSeconds(1f);
        Laser l = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        StartCoroutine(l.GodLaser(transform.position, player.position));
        yield return new WaitForSeconds(0.35f);
        LeanTween.moveLocalY(eyeLid.gameObject, 4, 0.1f);
        LeanTween.color(cam.gameObject, Color.white * 3f, 0.1f).setOnUpdate((Color val) =>
        {
            Color c = cam.backgroundColor;
            c = val;
            cam.backgroundColor = c;
        });
        LeanTween.color(environment.gameObject, new Color(1, 1, 1, 1), 0.1f);
        GetComponent<AudioSource>().Play();
        imp.GenerateImpulse();
        yield return new WaitForSeconds(0.12f);
        cam.backgroundColor = new Color(0.5113207f, 0.8176697f, 1f, 1);
    }

    IEnumerator SkyLasers()
    {
        for (int i = 0; i < 7; i++)
        {
            Laser l = Instantiate(laserPrefab, new Vector3(transform.position.x + (i - 3) * 2.5f, transform.position.y, 0f), Quaternion.identity);
            StartCoroutine(l.GodLaser2(new Vector3(transform.position.x + (i - 3) * 2.5f, 30, 0f), new Vector3(transform.position.x + (i - 3) * 2.5f, -30, 0f)));
        }
        yield return new WaitForSeconds(0.4f);
        GetComponent<AudioSource>().Play();
        imp.GenerateImpulse();
    }

    public void ChangeHealth(float amount)
    {
        health += amount;
        healthBar.value = health;
        if (health <= 8)
        {

            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    public void StartFight()
    {
        SoundManager.instance.PlaySong(music);
        SoundManager.instance.BossStart();
        SoundManager.instance.SetVolume(0.58F);
        StartCoroutine(RestFor(1f));
    }

    IEnumerator Die()
    {
        SoundManager.instance.BossEnd();
        stage = AttackStage.Resting;
        player.GetComponent<PlayerInput>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider2D>().enabled = false;
        Instantiate(death, transform.position, Quaternion.identity);
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].enabled = false;
        }
        SoundSetter s = Instantiate(soundPrefab);
        s.SetVolume(0.55f);
        s.SetSound(die);
        yield return new WaitForSeconds(3.5f);
        EventManager.instance.CutsceneTriggered(1);
        gameObject.SetActive(false);
    }
}

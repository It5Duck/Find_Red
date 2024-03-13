using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Devil : MonoBehaviour, IDamageable
{
    public float health {  get; set; }
    [SerializeField] Transform player;
    [SerializeField] Transform hornL;
    [SerializeField] Transform hornR;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Laser laserPrefab;
    [SerializeField] private float hornRadius;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Vector3 hornOffset;
    [SerializeField] private AudioClip laser;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip died;
    [SerializeField] private AudioClip music;
    [SerializeField] private GameObject death;
    [SerializeField] CinemachineImpulseSource imp;
    enum AttackStage { Resting, Thinking, Rotating, Shoot, Horns, Lasers}//0-5
    AttackStage stage = AttackStage.Resting;
    private float minRotationDistance;
    private bool canShoot = true;
    private float randomAngle = 999f;
    private float refVel = 0;
    private void Start()
    {
        health = 10f;
    }
    void Update()
    {
        if (stage == AttackStage.Thinking)
        {
            randomAngle = 999f;
            SelectRandomAttack();
        }
        else if(stage == AttackStage.Rotating)
        {
            if(randomAngle == 999f)
            {
                randomAngle = SelectRandomAngle();
            }
            if(canShoot)
            {
                // Start a shoot coroutine that Instantiates a projectile and starts a cooldown
                StartCoroutine(RapidFire());
            }
            if(Mathf.Abs(transform.eulerAngles.z-randomAngle) < 10f)
            {
                StartCoroutine(RestFor(1f));
            }

            transform.eulerAngles = new Vector3(0f, 0f, Mathf.SmoothDamp(transform.eulerAngles.z, randomAngle, ref refVel, 1f));
        }
        else if(stage == AttackStage.Shoot)
        {
            StartCoroutine(Shoot());
        }
        else if(stage == AttackStage.Horns)
        {
            if(Vector2.Distance(hornL.position, transform.position) < hornRadius)
            {
                hornL.position = Vector2.MoveTowards(hornL.position, transform.position, -10f * Time.deltaTime);
            }
            else
            {
                hornL.RotateAround(transform.position, new Vector3(0f, 0f, 1f), 100 * Time.deltaTime);
            }
            if (Vector2.Distance(hornR.position, transform.position) < hornRadius)
            {
                hornR.position = Vector2.MoveTowards(hornR.position, transform.position, -10f * Time.deltaTime);
            }
            else
            {
                hornR.RotateAround(transform.position, new Vector3(0f, 0f, 1f), 100 * Time.deltaTime);
            }
            if (canShoot)
            {
                StartCoroutine(HornCountDown());
                canShoot = false;
            }
        }
        else if(stage == AttackStage.Lasers)
        {
            int amount = Random.Range(5, 16);
            for (int i = 0; i < amount; i++)
            {
                float rndX = Random.Range(-1f, 1f);
                float rndY = Random.Range(-1f, 1f);
                Vector2 dir = new Vector2(rndX, rndY);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 30f, ground);
                Laser l = Instantiate(laserPrefab);
                StartCoroutine(PlaySoundAfter(0.4f, laser));
                if (hit)
                {
                    StartCoroutine(l.Activation(transform.position, hit.point));
                }
                else
                {

                    StartCoroutine(l.Activation(transform.position, dir * 30f));
                }
            }
            StartCoroutine(RestFor(2f));
        }
    }

    float SelectRandomAngle()
    {
        float a = Random.Range(0f, 360f);
        if(Mathf.Abs(a-transform.eulerAngles.z) > minRotationDistance)
        {
            return a;
        }

        return SelectRandomAngle();
    }

    void SelectRandomAttack()
    {
        stage = (AttackStage)Random.Range(2, 6);
    }

    public void ChangeHealth(float amount)
    {
        health += amount;
        healthBar.value = health;
        if (health <= 0)
        {

            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    IEnumerator RestFor(float seconds)
    {
        stage = AttackStage.Resting;
        yield return new WaitForSeconds(seconds);
        stage = AttackStage.Thinking;
    }

    IEnumerator Shoot()
    {

        StartCoroutine(RestFor(1.5f));
        transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, player.eulerAngles));
        yield return new WaitForSeconds(0.5f);
        Bullet b = Instantiate(bulletPrefab);
        b.SetDirection((player.position-transform.position).normalized);
        StartCoroutine(PlaySoundAfter(0f, shoot));
    }

    IEnumerator RapidFire()
    {
        canShoot = false;
        Bullet b = Instantiate(bulletPrefab);
        b.SetDirection(-transform.up);
        StartCoroutine(PlaySoundAfter(0f, shoot));
        yield return new WaitForSeconds(0.15f);
        canShoot = true;
    }

    IEnumerator HornCountDown()
    {
        yield return new WaitForSeconds(Random.Range(6f, 12f));
        canShoot = true;
        transform.eulerAngles = new Vector3(0, 0, 0);
        hornL.transform.position = new Vector2(transform.position.x - hornOffset.x, transform.position.y + hornOffset.y);
        hornR.transform.position = new Vector2(transform.position.x + hornOffset.x, transform.position.y + hornOffset.y);
        hornL.transform.eulerAngles = new Vector3 (0, 0, 0);
        hornR.transform.eulerAngles = new Vector3 (0, 0, 0);
        StartCoroutine(RestFor(1f));
    }

    IEnumerator PlaySoundAfter(float time, AudioClip clip)
    {
        yield return new WaitForSeconds(time);
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
    IEnumerator Die()
    {
        SoundManager.instance.BossEnd();
        stage = AttackStage.Resting;
        player.GetComponent<PlayerInput>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PlaySoundAfter(0f, died));
        Instantiate(death, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].enabled = false;
        }
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartFight()
    {
        SoundManager.instance.SetVolume(0.58F);
        SoundManager.instance.PlaySong(music);
        SoundManager.instance.BossStart();
        StartCoroutine(RestFor(1f));
    }
}

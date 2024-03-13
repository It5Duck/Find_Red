using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float fireRate;
    [SerializeField] private AudioClip shot;
    private Vector3 mousePos;
    private bool canShoot = true;
    Plane plane = new Plane(Vector3.forward, 0);
    Bullet b = null;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void GetClick(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if(canShoot)
            {
                b = Instantiate(bulletPrefab, transform.position + transform.up * spawnOffset.y, Quaternion.identity, transform);
                b.gameObject.layer = gameObject.layer;
                StartCoroutine(ShootCooldown());
            }
        }
        else if(value.canceled)
        {
            if(b != null)
            {
                b.transform.parent = null;
                b.SetDirection(-(b.transform.position - mousePos).normalized);
                b = null;
                source.clip = shot;
                source.Play();
            }
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            mousePos = ray.GetPoint(distance);

        }
        mousePos.z = 0f;
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        EventManager.instance.PlayerShot();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        yield return null;
    }
}

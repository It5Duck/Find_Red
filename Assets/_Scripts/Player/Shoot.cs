using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float fireRate;
    private Vector3 mousePos;
    private bool canShoot = true;
    Plane plane = new Plane(Vector3.forward, 0);
    Bullet b = null;

    public void GetClick(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if(canShoot)
            {
                b = Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.identity, transform);
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
            }
        }
    }

    private void FixedUpdate()
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

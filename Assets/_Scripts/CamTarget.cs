using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera mainCamera;
    [Range(2, 100)][SerializeField] private float cameraTargetDivider;
    Plane plane = new Plane(Vector3.forward, 0);
    private void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector2 cameraTargetPosition = Vector2.zero;

        if (plane.Raycast(ray, out distance))
        {
            cameraTargetPosition = (ray.GetPoint(distance) + target.position) / cameraTargetDivider;
        }

        transform.position = cameraTargetPosition;
    }
}

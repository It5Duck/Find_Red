using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    [SerializeField] EdgeCollider2D eCol;
    [SerializeField] Material mat;

    public IEnumerator Activation(Vector2 startPos, Vector2 endPos)
    {
        eCol.SetPoints(new List<Vector2> { startPos, endPos });
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        yield return new WaitForSeconds(0.4f);
        eCol.enabled = true;
        lr.widthMultiplier = 0.5f;
        lr.material = mat;
        yield return new WaitForSeconds(0.8f);
        eCol.enabled = false;
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }

    public IEnumerator GodLaser(Vector2 startPos, Vector2 endPos)
    {
        eCol.SetPoints(new List<Vector2> { startPos - new Vector2(0, startPos.y), endPos - new Vector2(0, startPos.y) });
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        yield return new WaitForSeconds(0.1f);
        eCol.enabled = true;
        lr.widthMultiplier = 0.5f;
        lr.material = mat;
        yield return new WaitForSeconds(1f);
        eCol.enabled = false;
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }

    public IEnumerator GodLaser2(Vector2 startPos, Vector2 endPos)
    {
        eCol.SetPoints(new List<Vector2> { new Vector2(0, startPos.y), new Vector2(0, endPos.y) });
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        yield return new WaitForSeconds(0.4f);
        eCol.enabled = true;
        lr.widthMultiplier = 0.5f;
        lr.material = mat;
        yield return new WaitForSeconds(1f);
        eCol.enabled = false;
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }
}

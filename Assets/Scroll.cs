using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [Range(0f, 500f), SerializeField] float speed = 1f;
    public bool scroll = false;

    private void Update()
    {
        if (!scroll)
        {
            return;
        }
        if (sr.size.y >= 14)
        {
            sr.size = new Vector2(7f, sr.size.y - 7f);
        }

        sr.size += new Vector2(0f, Time.deltaTime * speed);
    }
}

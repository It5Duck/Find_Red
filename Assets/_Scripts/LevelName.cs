using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelName : MonoBehaviour
{
    [SerializeField] TMP_Text[] texts;
    void Start()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            LeanTween.color(texts[i].gameObject, new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 0f), 4f).setEaseInQuint();
        }
    }
}

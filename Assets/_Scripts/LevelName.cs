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
            var color = texts[i].color;
            var fadeoutcolor = color;
            fadeoutcolor.a = 0f;
            LeanTween.value(gameObject, updateValueExampleCallback, color, fadeoutcolor, 1f);
        }
    }
    void updateValueExampleCallback(Color val)
    {
        texts[0].color = val;
    }
}

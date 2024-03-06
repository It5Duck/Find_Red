using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] Image img;
    void Start()
    {
        img.color = new Color(0f,0f,0f,1f);
        FadeOut();
    }

    void FadeOut()
    {
        var color = img.color;
        var fadeoutcolor = color;
        fadeoutcolor.a = 0f;
        LeanTween.value(gameObject, updateValueExampleCallback, color, fadeoutcolor, 1f);
    }

    void updateValueExampleCallback(Color val)
    {
        img.color = val;
    }

    public void FadeIn()
    {
        var color = img.color;
        var fadeoutcolor = color;
        fadeoutcolor.a = 1f;
        LeanTween.value(gameObject, updateValueExampleCallback, color, fadeoutcolor, 2f);
    }
}

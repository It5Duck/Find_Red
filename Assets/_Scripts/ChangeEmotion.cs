using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmotion : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] string myname;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        EventManager.instance.OnEmotionChanged += Change;
    }

    void Change(Sprite emotion, string speaker)
    {
        if(speaker == myname)
        {
            sr.sprite = emotion;
        }
    }
}

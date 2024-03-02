using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Cutpoint
{
    public int id; //Index of the cutscene, restarts from 0 in every scene
    //Speaker (probably needs to be another class to identify who is speaking) for now it's just a string (name)
    public bool displayText = true;
    [TextArea(2, 10)]
    public string text;
    public string speaker;

    public bool moveCamera = false;
    public Transform target;

    public bool changeEmotion = false;
    public Sprite emotion; //A sprite that's displayed on the speaker's face
}

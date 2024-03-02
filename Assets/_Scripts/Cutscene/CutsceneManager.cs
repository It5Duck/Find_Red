using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public List<Cutscene> cutscenes;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] TMP_Text speech;
    [SerializeField] TMP_Text speaker;

    bool cutsceneMode = false;
    private void Start()
    {
        EventManager.instance.OnCutsceneTriggered += StartCutscene;
    }

    public void PlayNextCutscene(int index)
    {
        cutscenes[index].StartPlaymode(this);
    }

    public void StartCutscene(int index)
    {
        cutsceneMode = true;
        PlayNextCutscene(index);
    }

    public void DisplayCutpoint(Cutpoint point)
    {
        if (point.moveCamera)
        {
            virtualCamera.m_Follow = point.target;
        }
        if (point.displayText)
        {
            speech.text = point.text;
            speaker.text = point.speaker;
        }
        if (point.changeEmotion)
        {
            EventManager.instance.EmotionChanged(point.emotion, point.speaker);
        }
    }

    public void ExitPlaymode()
    {
        cutsceneMode = false;
        EventManager.instance.CutsceneExited();
    }
}

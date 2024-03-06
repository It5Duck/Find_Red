using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneManager : MonoBehaviour
{
    public List<Cutscene> cutscenes;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] TMP_Text speech;
    [SerializeField] TMP_Text speaker;
    [SerializeField] bool playOnStart = false;
    [SerializeField] float startAfter = 0f;//seconds waited befor starting
    private int currentCutsceneIndex = 0;

    bool cutsceneMode = false;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        EventManager.instance.OnCutsceneTriggered += StartCutscene;
        if (playOnStart)
        {
            yield return new WaitForSeconds(startAfter);
            StartCutscene(0);
        }
        yield return null;
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
            speech.transform.parent.gameObject.SetActive(true);
            speech.text = point.text;
            speaker.text = point.speaker;
        }
        else
        {
            speech.text = "";
            speaker.text = "";
            speech.transform.parent.gameObject.SetActive(false);
        }
        if (point.changeEmotion)
        {
            EventManager.instance.EmotionChanged(point.emotion,point.speaker);
        }
    }

    public void ExitPlaymode()
    {
        cutsceneMode = false;
        EventManager.instance.CutsceneExited();
        speech.transform.parent.gameObject.SetActive(false);
    }

    public void NextPressed(InputAction.CallbackContext value)
    {
        if(value.performed && cutsceneMode)
        {
            cutscenes[currentCutsceneIndex].PlayNextPoint();
        }
    }
}

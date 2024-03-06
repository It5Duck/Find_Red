using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneAnim : MonoBehaviour
{
    private enum CutsceneName { House, GodsCall}
    [SerializeField] private string myName;
    [SerializeField] private int cutpointIndex;
    [SerializeField] private CutsceneName cutsceneName;
    [SerializeField] private PlayerInput input;
    [SerializeField] private Transform god;
    [SerializeField] private CinemachineVirtualCamera cam;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        if(cutsceneName == CutsceneName.House)
        {
            if(myName == "it") //"it" is the player
            {
                EventManager.instance.OnMovedToNextCutpoint += HouseIt;
            } 
        }
        else if(cutsceneName == CutsceneName.GodsCall)
        {
            if(myName == "it")
            {
                cam.m_Follow = god;
                input.enabled = false;
                EventManager.instance.OnMovedToNextCutpoint += GodIt;
            }
        }
    }

    void HouseIt(int index)
    {
        if(index == 0)
        {
            LeanTween.rotate(gameObject, new Vector3(0f, 0f, 0f), 0.75f).setEaseOutElastic();
        }
        else if( index == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void GodIt(int index)
    {
        if (index == 8)
        {
            cam.m_Follow = input.transform;
            input.enabled = true;
        }
    }
}

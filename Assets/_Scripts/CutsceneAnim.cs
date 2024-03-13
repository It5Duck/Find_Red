using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneAnim : MonoBehaviour
{
    private enum CutsceneName { House, GodsCall, Satan, FinalGod}
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
        else if(cutsceneName== CutsceneName.Satan)
        {
            if (myName == "Satan")
            {
                EventManager.instance.OnMovedToNextCutpoint += Satan;
            }
            else if (myName == "it")
            {
                input.enabled = false;
                EventManager.instance.OnMovedToNextCutpoint += SatanIt;
            }
        }
        else if(cutsceneName == CutsceneName.FinalGod)
        {
            if(myName == "it")
            {
                input.enabled = false;
                EventManager.instance.OnMovedToNextCutpoint += FinalGodIt;
            }
            else if(myName == "God")
            {
                EventManager.instance.OnMovedToNextCutpoint += FinalGod;
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
        if (index == 10)
        {
            cam.m_Follow = input.transform;
            input.enabled = true;
        }
    }

    void Satan(int index)
    {
        if(index == 5)
        {
            GetComponent<Devil>().StartFight();
        }
    }
    void SatanIt(int index)
    {
        if (index == 5)
        {
            input.enabled = true;
        }
    }

    void FinalGod(int index)
    {
        if( index == 5)
        {
            if(gameObject.activeSelf)
            {
                GetComponent<God>().StartFight();
            }
        }
    }
    void FinalGodIt(int index)
    {
        if (index == 5)
        {
            input.enabled = true;
        }
        else if(index == 6)
        {
            SceneManager.LoadScene(0);
        }
    }
}

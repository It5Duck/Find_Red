using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneAnim : MonoBehaviour
{
    private enum CutsceneName { House, GodsCall}
    [SerializeField] private string myName;
    [SerializeField] private int cutpointIndex;
    [SerializeField] private CutsceneName cutsceneName;
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

        }
    }

    void HouseIt(int index)
    {
        if(index == 0)
        {
            LeanTween.rotate(gameObject, new Vector3(0f, 0f, 0f), 0.75f).setEaseOutBounce();
        }
        else if( index == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene")]
public class Cutscene : ScriptableObject
{
    public List<Cutpoint> cutpoints;
    int index = 0;
    CutsceneManager manager;

    public void StartPlaymode(CutsceneManager m)
    {
        index = 0;
        manager = m;
        PlayNextPoint();
    }

    public void PlayNextPoint()
    {
        if (index >= cutpoints.Count)
        {
            manager.ExitPlaymode();
            return;
        }
        manager.DisplayCutpoint(cutpoints[index]);
        EventManager.instance.MovedToNextCutpoint(index);
        index++;
    }
}

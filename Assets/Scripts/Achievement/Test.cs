using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        if (FindObjectOfType<HandCardGroup>() != null)
        {
            AchievementControl.Instance.test = true;
            if (AchievementControl.Instance.ifnewgame)
            {
                AchievementControl.Instance.ifnewgame = false;
                AchievementControl.Instance.if16happen = true;
            }
            AchievementControl.Instance.disastertime = 0;
            AchievementControl.Instance.if11happen = false;
            AchievementControl.Instance.if12happen = false;
            for (int i = 0; i < AchievementControl.Instance.warwintime.Length; i++)
            {
                AchievementControl.Instance.warwintime[i] = 0;
            }
        }
        else
        {
            AchievementControl.Instance.test = false;
        }
        DisasterManager.thisDisaster=null;
    }

    void Update()
    {

    }
}

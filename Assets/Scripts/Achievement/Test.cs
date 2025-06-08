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
        }
        else
        {
            AchievementControl.Instance.test = false;
        }
    }

    void Update()
    {

    }
}

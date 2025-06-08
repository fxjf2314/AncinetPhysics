using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    CreateAchievement achievement;
    int id;
    GameObject finish;
    Sprite blacksprite;
    Sprite finishsprite;
    void Start()
    {
        finish=gameObject.transform.GetChild(2).gameObject;
        int.TryParse(gameObject.name,out id);
        achievement = Achievements.Instance.achievements[id];
        //finishsprite=achievement.finishsprite;
        //blacksprite=achievement.blacksprite;
    }

    void Update()
    {
        if(achievement != null)
        {
            if (achievement.finish)
            {
                finish.SetActive (true);
                //gameObject.GetComponent<Image>().sprite = finishsprite;
            }
            else
            {
                finish.SetActive (false);
                //gameObject.GetComponent<Image>().sprite = blacksprite;
            }
        }
    }
}

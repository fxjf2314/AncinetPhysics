using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Achievement : MonoBehaviour
{
    CreateAchievement achievement;
    int id;
    GameObject finish;
    Sprite blacksprite;
    Sprite finishsprite;
    TextMeshProUGUI icon;
    void Start()
    {
        finish=gameObject.transform.GetChild(2).gameObject;
        icon=gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        int.TryParse(gameObject.name,out id);
        achievement = Achievements.Instance.achievements[id];
        finishsprite=achievement.finishsprite;
        blacksprite=achievement.blacksprite;
    }

    void Update()
    {
        if(achievement != null)
        {
            if (achievement.finish)
            {
                finish.SetActive (true);
                icon.text=achievement.icon;
                gameObject.GetComponent<Image>().sprite = finishsprite;
            }
            else
            {
                finish.SetActive (false);
                gameObject.GetComponent<Image>().sprite = blacksprite;
            }
        }
    }
}

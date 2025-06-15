using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Slider slider;
    public Button button;
    public Sprite closevoice;

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

    public void Achi19()
    {
        if (!Achievements.Instance.achievements[19].finish)
        {
            if (slider.value == -80 && button.gameObject.GetComponent<Image>().sprite == closevoice)
            {
                Invoke("Ifalso", 0.001f);
            }
        }
    }

    void Ifalso()
    {
        CancelInvoke("Ifalso");
        if(button.gameObject.GetComponent<Image>().sprite == closevoice)
        {
            Achievements.Instance.achievements[19].finish = true;
            AchievementControl.Instance.nowachis++;
            PlayerPrefs.DeleteKey("nowachievement");
            PlayerPrefs.SetInt("nowachievement", AchievementControl.Instance.nowachis);
            PlayerPrefs.Save();
            Achievements.Instance.Saveachievement(19);
        }
    }
}

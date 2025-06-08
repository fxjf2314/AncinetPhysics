using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    static Achievements instance;
    public static Achievements Instance => instance;

    public List<CreateAchievement> achievements;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Saveachievement(int id)
    {
        PlayerPrefs.DeleteKey("achievement"+id.ToString());
        PlayerPrefs.SetInt("achievement" + id.ToString(), achievements[id].finish ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Saveallachievement()
    {
        foreach (CreateAchievement one in achievements)
        {
            if (one != null)
            {
                PlayerPrefs.DeleteKey("achievement" + one.id.ToString());
                PlayerPrefs.SetInt("achievement" + one.id.ToString(), achievements[one.id].finish ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
    }

    public void Loadallachievement()
    {
        foreach (CreateAchievement one in achievements)
        {
            if (one != null)
            {
                if (PlayerPrefs.GetInt("achievement" + one.id.ToString()) == 0)
                {
                    one.finish = false;
                }
                else if (PlayerPrefs.GetInt("achievement" + one.id.ToString()) == 1)
                {
                    one.finish = true;
                }
            }
        }
    }
}

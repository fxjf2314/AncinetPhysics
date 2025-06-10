using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Statistic : MonoBehaviour
{
    public static Statistic Instance => instance;
    private static Statistic instance;

    public TextMeshProUGUI maxscoret;
    public TextMeshProUGUI minscoret;
    public TextMeshProUGUI distime;
    public TextMeshProUGUI maxcardtime;
    public TextMeshProUGUI mincardtime;
    public Image maxcard;
    public Image mincard;

    public UnityEngine.UI.Button exitbutton;


    int maxcardid;
    int mincardid;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxcardid = 1;
        mincardid = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Updatestatistic()
    {
        if (AchievementControl.Instance.maxscore == 0)
        {
            maxscoret.text = "历史最高分数： " + "暂无游戏记录";
        }
        else
        {
            maxscoret.text = "历史最高分数： " + AchievementControl.Instance.maxscore;
        }

        if (AchievementControl.Instance.minscore == 0)
        {
            minscoret.text = "历史最低分数： " + "暂无游戏记录";
        }
        else
        {
            minscoret.text = "历史最低分数： " + AchievementControl.Instance.minscore;
        }

        distime.text = "积累发生灾害次数： "+PlayerPrefs.GetInt("disastertime",0).ToString();

        //Maxtomin(AchievementControl.Instance.usecardtime);
        for (int i=2; i < AchievementControl.Instance.usecardtime.Length; i++)
        {
            if (AchievementControl.Instance.usecardtime[i]> AchievementControl.Instance.usecardtime[maxcardid])
            {
                maxcardid = i;
            }
            if (AchievementControl.Instance.usecardtime[i] < AchievementControl.Instance.usecardtime[mincardid])
            {
                mincardid = i;
            }
        }
        maxcardtime.text = Cards.Instance.cards[maxcardid].GetDescription().title+"\n"+ AchievementControl.Instance.usecardtime[maxcardid]+"次";
        mincardtime.text = Cards.Instance.cards[mincardid].GetDescription().title + "\n" + AchievementControl.Instance.usecardtime[mincardid] + "次";
        maxcard.sprite= Cards.Instance.cards[maxcardid].GetSprite();
        mincard.sprite = Cards.Instance.cards[mincardid].GetSprite();
    }

    //int[] Maxtomin(int[] i)
    //{
    //    int[] newarray = new int[i.Length];
    //    maxcardid = 0;
    //    mincardid = 0;

    //    for(int t=0; t<i.Length; t++)
    //    {
    //        newarray[t] = i[t];
    //    }

    //    for(int t=0; t < newarray.Length-1; t++)
    //    {
    //        for(int m=t+1;m<newarray.Length; m++)
    //        {
    //            if (newarray[m] > newarray[t])
    //            {
    //                int savenum=newarray[m];
    //                newarray[m] = newarray[t];
    //                newarray[t]= savenum;
    //            }
    //        }
    //    }


    //    return newarray;
    //}
}

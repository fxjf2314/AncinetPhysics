using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementControl : MonoBehaviour
{
    public static AchievementControl Instance => instance;
    private static AchievementControl instance;

    private HandCard handcard;

    public bool if11happen;
    public bool if12happen;
    public bool if16happen;

    public bool test;
    public bool ifnewgame;
    public int newachievement;
    public int[] warwintime;

    public int minscore;
    public int maxscore;
    public int disastertime;
    public int[] usecardtime=new int[21];

    public int beforeachis;
    public int nowachis;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (int i in usecardtime)
        {
            usecardtime[i] = PlayerPrefs.GetInt("card" + i, 0);
        }
        minscore = PlayerPrefs.GetInt("minscore", 0);
        maxscore = PlayerPrefs.GetInt("maxscore", 0);
        beforeachis = PlayerPrefs.GetInt("beforeachievement", 0);
        nowachis = PlayerPrefs.GetInt("nowachievement", 0);
    }

    void Start()
    {
        Achievements.Instance.Loadallachievement();
        Achievements.Instance.Saveallachievement();
        newachievement =0;
        if11happen = false;
        if12happen = false;
        if16happen = false;
        ifnewgame = false;
        disastertime = 0;
    }


    void Update()
    {
        if (test)
        {
            if(Achievements.Instance.achievements[12].finish == false)
            {
                int fire = 0;
                foreach(AreaScript area in HandCard.MyInstance.allarea)
                {
                    if (area.gameObject.transform.GetChild(1).gameObject.activeSelf)
                    {
                        fire++;
                    }
                }
                if (fire >= 4)
                {
                    if12happen = true;
                }
            }
        }

    }

    public void GameOverTestAchievement()
    {
        if (test)
        {
            handcard=GameObject.Find("HandCard").GetComponent<HandCard>();
                if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 20000 && Achievements.Instance.achievements[1].finish == false)
                {
                    Achievements.Instance.achievements[1].finish = true;
                    newachievement++;
                }//1
                if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 30000 && Achievements.Instance.achievements[2].finish == false)
                {
                    Achievements.Instance.achievements[2].finish = true;
                    newachievement++;
                }//2
                if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 40000 && Achievements.Instance.achievements[3].finish == false)
                {
                    Achievements.Instance.achievements[3].finish = true;
                    newachievement++;
                }//3

            if ((int)((UIManager.MyInstance.populationmessage)) >= 70 && Achievements.Instance.achievements[4].finish == false)
            {
                Achievements.Instance.achievements[4].finish = true;
                newachievement++;
            }//4

            if (Achievements.Instance.achievements[5].finish == false)
            {
                if (disastertime >= 4)
                {
                    Achievements.Instance.achievements[5].finish = true;
                    newachievement++;
                }
            }//5

            if (Achievements.Instance.achievements[6].finish == false)
            {
                foreach (AreaScript area in handcard.allarea)
                {
                    if (area.areaDetail.oCoin + area.areaDetail.oFood > 3200)
                    {
                        Achievements.Instance.achievements[6].finish = true;
                        newachievement++;
                        break;
                    }
                }
            }//6

            if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 35000&&Achievements.Instance.achievements[7].finish == false)
            {
                bool nocard=true;
                foreach (AreaScript area in handcard.allarea)
                {
                    if (area.cards.Contains(Cards.Instance.cards[2]) || area.cards.Contains(Cards.Instance.cards[3]) || area.cards.Contains(Cards.Instance.cards[4]) || area.cards.Contains(Cards.Instance.cards[6]) || area.cards.Contains(Cards.Instance.cards[7]) || area.cards.Contains(Cards.Instance.cards[8])|| area.cards.Contains(Cards.Instance.cards[9]) || area.cards.Contains(Cards.Instance.cards[10]) || area.cards.Contains(Cards.Instance.cards[14]) || area.cards.Contains(Cards.Instance.cards[15]) || area.cards.Contains(Cards.Instance.cards[20]))
                    {
                        nocard = false;
                        break;
                    }
                }
                if (nocard)
                {
                    Achievements.Instance.achievements[7].finish = true;
                    newachievement++;
                }
            }//7

            if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 24000 && Achievements.Instance.achievements[8].finish == false)
            {
                if (handcard.cards.Count == 0)
                {
                    Achievements.Instance.achievements[8].finish = true;
                    newachievement++;
                }
            }//8

            if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 30000 && Achievements.Instance.achievements[9].finish == false)
            {
                if (handcard.cards.Count == 4)
                {
                    if (handcard.cards.Contains(Cards.Instance.cards[1]) && handcard.cards.Contains(Cards.Instance.cards[11]) && handcard.cards.Contains(Cards.Instance.cards[12]) && handcard.cards.Contains(Cards.Instance.cards[13]))
                    {
                        Achievements.Instance.achievements[9].finish = true;
                        newachievement++;
                    }
                }
            }//9

            if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 30000 && Achievements.Instance.achievements[10].finish == false)
            {
                if (handcard.cards.Count == 4) {
                    if(handcard.cards.Contains(Cards.Instance.cards[15])&& handcard.cards.Contains(Cards.Instance.cards[9]) && handcard.cards.Contains(Cards.Instance.cards[17]) && handcard.cards.Contains(Cards.Instance.cards[19]))
                    {
                        Achievements.Instance.achievements[10].finish = true;
                        newachievement++;
                    }
                }
            }//10

            if (if11happen)
            {
                Achievements.Instance.achievements[11].finish = true;
                newachievement++;
                if11happen = false;
            }//11

            if (if12happen)
            {
                Achievements.Instance.achievements[12].finish = true;
                newachievement++;
                if12happen = false;
            }//12

            if (Achievements.Instance.achievements[13].finish == false)
            {
                foreach(int time in warwintime)
                {
                    if (time >= 4)
                    {
                        Achievements.Instance.achievements[13].finish = true;
                        newachievement++;
                        break;
                    }
                }
            }//13

            if (Achievements.Instance.achievements[14].finish == false)
            {
                foreach (AreaScript area in handcard.allarea)
                {
                    if (area.cards.Contains(Cards.Instance.cards[6]) && area.cards.Contains(Cards.Instance.cards[8]) && area.cards.Contains(Cards.Instance.cards[20]))
                    {
                        Achievements.Instance.achievements[14].finish = true;
                        newachievement++;
                        break;
                    }
                }
            }//14

            if (Achievements.Instance.achievements[15].finish == false)
            {
                foreach (AreaScript area in handcard.allarea)
                {
                    if (area.cards.Contains(Cards.Instance.cards[19]) && area.cards.Contains(Cards.Instance.cards[17]))
                    {
                        Achievements.Instance.achievements[15].finish = true;
                        newachievement++;
                        break;
                    }
                }
            }//15

            if (if16happen && Achievements.Instance.achievements[16].finish == false)
            {
                    if (handcard.cards.Contains(Cards.Instance.cards[2]) && handcard.cards.Contains(Cards.Instance.cards[3]) && handcard.cards.Contains(Cards.Instance.cards[7]) && handcard.cards.Contains(Cards.Instance.cards[15]))
                    {
                        Achievements.Instance.achievements[16].finish = true;
                        newachievement++;
                    }
            }//16

            for (int i = 1; i < Cards.Instance.cards.Count; i++)
            {
                if (handcard.cards.Contains(Cards.Instance.cards[i]))
                {
                    usecardtime[i]=PlayerPrefs.GetInt("card"+i,0);
                    usecardtime[i]++;
                    PlayerPrefs.DeleteKey("card"+i);
                    PlayerPrefs.SetInt("card"+i, usecardtime[i]);
                    PlayerPrefs.Save();
                    if(Achievements.Instance.achievements[17].finish == false)
                    {
                        if (usecardtime[i] > 10)
                        {
                            Achievements.Instance.achievements[17].finish = true;
                            newachievement++;
                        }
                    }
                }
            }//统计卡牌使用次数+ 成就17判断

            if (Achievements.Instance.achievements[18].finish == false&& (int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage))<=15000)
            {
                foreach (AreaScript area in handcard.allarea)
                {
                    if (area.cards.Count>0)
                    {
                        Achievements.Instance.achievements[18].finish = true;
                        newachievement++;
                        break;
                    }
                }
            }//18

            //19在test

            if (disastertime > 0)
            {
                int time = PlayerPrefs.GetInt("disastertime", 0);
                time += disastertime;
                PlayerPrefs.DeleteKey("disastertime");
                PlayerPrefs.SetInt("disastertime", time);
                PlayerPrefs.Save();
            }
            #region 存储最大分数和最小分数
            if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) > maxscore)
            {
                maxscore = (int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage));
                PlayerPrefs.DeleteKey("maxscore");
                PlayerPrefs.SetInt("maxscore",maxscore);
                PlayerPrefs.Save();
                if (minscore == 0)
                {
                    minscore = (int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage));
                    PlayerPrefs.DeleteKey("minscore");
                    PlayerPrefs.SetInt("minscore", minscore);
                    PlayerPrefs.Save();
                }
            }
            else if((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) < minscore)
            {
                minscore = (int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage));
                PlayerPrefs.DeleteKey("minscore");
                PlayerPrefs.SetInt("minscore", minscore);
                PlayerPrefs.Save();
            }
            #endregion
            if (newachievement > 0)
            {
                nowachis += newachievement;
                PlayerPrefs.DeleteKey("nowachievement");
                PlayerPrefs.SetInt("nowachievement",nowachis);
                PlayerPrefs.Save();
                Achievements.Instance.Saveallachievement();
            }
        }
    }

    public void Test20()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Input.mousePosition;

            RectTransform rectTransform = Statistic.Instance.exitbutton.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out Vector2 localPoint);

            if (rectTransform.rect.Contains(localPoint))
            {
                if (Achievements.Instance.achievements[20].finish == false)
                {
                    Achievements.Instance.achievements[20].finish = true;
                    Achievements.Instance.Saveachievement(20);
                    nowachis++;
                    PlayerPrefs.DeleteKey("nowachievement");
                    PlayerPrefs.SetInt("nowachievement", nowachis);
                    PlayerPrefs.Save();
                }
#if UNITY_EDITOR
                // 如果是在Unity编辑器中，调用Unity的关闭方法
                UnityEditor.EditorApplication.isPlaying = false;
#else
			// 如果是在构建的游戏中，调用Application的退出方法
			Application.Quit();
#endif
            }
        }
    }

}

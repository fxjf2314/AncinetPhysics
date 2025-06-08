using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementControl : MonoBehaviour
{
    public static AchievementControl Instance => instance;
    private static AchievementControl instance;

    private HandCard handcard;

    public bool test;
    public int newachievement;
    public int[] usecardtime=new int[21];

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        newachievement=0;
        Achievements.Instance.Loadallachievement();
        Achievements.Instance.Saveallachievement();
    }


    void Update()
    {
        
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

            if (Achievements.Instance.achievements[9].finish == false)
            {
                foreach (AreaScript area in handcard.allarea)
                {
                    if (area.cards.Contains(Cards.Instance.cards[19])&&area.cards.Contains(Cards.Instance.cards[17])) 
                    {
                        Achievements.Instance.achievements[9].finish = true;
                        newachievement++;
                        break;
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

            if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 24000 && Achievements.Instance.achievements[11].finish == false)
            {
                if (handcard.cards.Count == 0)
                {
                        Achievements.Instance.achievements[11].finish = true;
                        newachievement++;
                }
            }//11

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

            if ((int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage)) >= 30000 && Achievements.Instance.achievements[15].finish == false)
            {
                if (handcard.cards.Count == 4)
                {
                    if (handcard.cards.Contains(Cards.Instance.cards[1]) && handcard.cards.Contains(Cards.Instance.cards[11]) && handcard.cards.Contains(Cards.Instance.cards[12]) && handcard.cards.Contains(Cards.Instance.cards[13]))
                    {
                        Achievements.Instance.achievements[15].finish = true;
                        newachievement++;
                    }
                }
            }//15

            for(int i = 1; i < Cards.Instance.cards.Count; i++)
            {
                if (handcard.cards.Contains(Cards.Instance.cards[i]))
                {
                    usecardtime[i]=PlayerPrefs.GetInt("card"+i);
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
            }//统计卡牌使用次数+17

            if (newachievement > 0)
            {
                Debug.Log("1111");
                Achievements.Instance.Saveallachievement();
            }
        }
    }
}

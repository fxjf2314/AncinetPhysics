using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gameover : MonoBehaviour
{
    public Image shader;
    public GameObject overback;
    //public GameObject finalscoremes;
   // public GameObject finalscoretip;
    public GameObject backhome;
    public Animator downappear;
    public TextMeshProUGUI population;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI food;
    public TextMeshProUGUI score;

    private float title1;
    private float title2;
    private float title3;
    private float title4;

    private bool title1start=false;
    private bool title2start=false;
    private bool title3start=false;
    private bool title4start=false;
    private bool getscore=false;

    private bool ifover=false;
    private bool ifshader=false;

    public float finalscore;//最终得分

    private float colora = 0;
    // Start is called before the first frame update
    void Start()
    {
        //finalscoremes.SetActive(false);
        //finalscoretip.SetActive(false);
        backhome.SetActive(false);
        shader.gameObject.SetActive(false);
        population.gameObject.SetActive(false);
        coin.gameObject.SetActive(false);
        food.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
         title1start = false;
         title2start = false;
         title3start = false;
         title4start = false;
        ifover = false;
        colora= 0;
        downappear.SetBool("ifdown",false);
    }

    // Update is called once per frame
    void Update()
    {

        finalscore = (int)((UIManager.MyInstance.coinmessage + UIManager.MyInstance.foodmessage));

        if (UIManager.MyInstance.totalRound == 11)
        {
            
            drageffect.Instance.allban = true;
            GameObject.Find("Main Camera").GetComponent<DragCamera>().enabled = false;
            if (!ifover)
            {
                shader.gameObject.SetActive(true);
                Invoke("GameOver", 3);
            }
        }

        if (ifshader&&colora<225)
        {
            colora += 100f*Time.deltaTime;
            shader.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, colora / 255f);
        }
        else if(ifshader&&colora>=225)
        {
            ifshader = false;
            Appearimage();
        }

        #region 数值连环增加
        //数值1增加
        if (title1start)
        {
            if (title1 < UIManager.MyInstance.populationmessage)
            {
                if ((int)(UIManager.MyInstance.populationmessage / 150) < 1)
                {
                    title1++;
                }
                else
                {
                    title1 += (int)(UIManager.MyInstance.populationmessage / 150);
                }
            }
            else if (title1 >= UIManager.MyInstance.populationmessage)
            {
                title1 = UIManager.MyInstance.populationmessage;
                Invoke("Start2", 1f);
            }
        }

        //数值2增加
        if (title2start)
        {
            if (title2 < UIManager.MyInstance.coinmessage)
            {
                if((int)(UIManager.MyInstance.coinmessage / 150) < 1)
                {
                    title2 ++;
                }
                else
                {
                    title2 += (int)(UIManager.MyInstance.coinmessage / 150);
                }
                
            }
            else if (title2 >= UIManager.MyInstance.coinmessage)
            {
                title2 = UIManager.MyInstance.coinmessage;
                Invoke("Start3", 1f);
            }
        }

        //数值3增加
        if (title3start)
        {
            if (title3 < UIManager.MyInstance.foodmessage)
            {   if((int)(UIManager.MyInstance.foodmessage / 150) < 1)
                {
                    title3 ++;
                }
                else
                {
                    title3 += (int)(UIManager.MyInstance.foodmessage / 150);
                }
                
            }
            else if (title3 >= UIManager.MyInstance.foodmessage)
            {
                title3 = UIManager.MyInstance.foodmessage;
                Invoke("Start4", 1f);
            }
        }

        //数值4增加
        if (title4start)
        {
            
            if (title4 < finalscore)
            {
                if((int)(finalscore / 250) < 1)
                {
                    title4 ++;
                }
                else
                {
                    title4 += (int)(finalscore / 250);
                }
                
            }
            else if (title4 >= finalscore)
            {
                title4 = finalscore;
                if (backhome != null)
                {
                    backhome.SetActive(true);
                }
            }
        }
        #endregion

        if (getscore)
        {
            population.text = title1.ToString();
            coin.text = title2.ToString();
            food.text = title3.ToString();
            score.text = title4.ToString();
        }
    }

    private void GameOver()
    {
        CancelInvoke("GameOver");
        ifover=true;
        ifshader = true;
    }

    private void Appearimage()
    {
        downappear.SetBool("ifdown", true);
        getscore=true;
        Invoke("Start1", 1f);
        
    }

    void Start1()
    {
        population.gameObject.SetActive(true);
        title1start = true;
    }

    void Start2() 
    {
        if (coin != null)
        {
            coin.gameObject.SetActive(true);
            title2start = true;
        }
    }

    void Start3()
    {
        if (food != null)
        {
            food.gameObject.SetActive(true);
            title3start = true;
        }
    }

    void Start4() 
    {
        if (score != null)
        {
            score.gameObject.SetActive(true);
            title4start = true;
        }
    }
        
        
}

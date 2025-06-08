using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LookAchievement : MonoBehaviour
{
    public int nowpage;
    public float movepace;
    public GameObject[] dot;
    public Color simpledot;
    public Color hitdot;
    public GameObject page;
    public GameObject Leftbtn;
    public GameObject Rightbtn;
    public GameObject achievementpage;
    public GameObject btnpanel;

    void Start()
    {

    }

    void Update()
    {
        for(int i=1;i<dot.Length; i++)
        {
            dot[i].GetComponent<Image>().color = simpledot;
        }
        dot[nowpage].GetComponent<Image>().color = hitdot;
    }

    public void OpenAchievement()
    {
        btnpanel.SetActive(false);
        nowpage = 1;
        page.transform.localPosition = new Vector3(0, 0, 0);
        Leftbtn.SetActive(false);
        Rightbtn.SetActive(true);
        achievementpage.SetActive(true);
    }

    void PageMove(float pace)
    {
        StartCoroutine(PageMoveCou(pace));
    }

    IEnumerator PageMoveCou(float movepace)
    {
        btnpanel.SetActive(true);
        float savex=page.transform.localPosition.x;
        float nowx = savex;
        if (movepace > 0)
        {
            while (nowx - savex < movepace)
            {
                nowx += movepace / 100;
                page.transform.localPosition = new Vector3(nowx, 0, 0);
                yield return null;
            }
            if (nowx - savex >= movepace)
            {
                nowx = savex + movepace;
                page.transform.localPosition = new Vector3(nowx, 0, 0);
                btnpanel.SetActive(false);
                yield break;
            }
        }
        else if (movepace < 0)
        {
            while (nowx - savex > movepace)
            {
                nowx += movepace / 100;
                page.transform.localPosition = new Vector3(nowx, 0, 0);
                yield return null;
            }
            if (nowx - savex <= movepace)
            {
                nowx = savex + movepace;
                page.transform.localPosition = new Vector3(nowx, 0, 0);
                btnpanel.SetActive(false);
                yield break;
            }
        }
    }

    public void NextPage()
    {
        switch (nowpage)
        {
            case 1:
                nowpage++;
                PageMove(-movepace);
                Leftbtn.SetActive(true);
                break;
            case 2:
                nowpage++;
                PageMove(-movepace);
                break;
            case 3:
                nowpage++;
                PageMove(-movepace);
                break;
            case 4:
                nowpage++;
                PageMove(-movepace);
                Rightbtn.SetActive(false);
                break;
        }
    }

    public void PreviousPage()
    {
        switch (nowpage)
        {
            case 2:
                nowpage--;
                PageMove(movepace);
                Leftbtn.SetActive(false);
                break;
            case 3:
                nowpage--;
                PageMove(movepace);
                break;
            case 4:
                nowpage--;
                PageMove(movepace);
                break;
            case 5:
                nowpage--;
                PageMove(movepace);
                Rightbtn.SetActive(true);
                break;
        }
    }

    public void ToPage1()
    {
        switch (nowpage)
        {
            case 2:
                nowpage = 1;
                PageMove(movepace);
                Leftbtn.SetActive(false);
                break;
            case 3:
                nowpage = 1;
                PageMove(2*movepace);
                Leftbtn.SetActive(false);
                break;
            case 4:
                nowpage = 1;
                PageMove(3*movepace);
                Leftbtn.SetActive(false);
                break;
            case 5:
                nowpage = 1;
                PageMove(4 * movepace);
                Leftbtn.SetActive(false);
                Rightbtn.SetActive(true);
                break;
        }
    }

    public void ToPage2()
    {
        switch (nowpage)
        {
            case 1:
                nowpage = 2;
                PageMove(-movepace);
                Leftbtn.SetActive(true);
                break;
            case 3:
                nowpage = 2;
                PageMove(movepace);
                break;
            case 4:
                nowpage = 2;
                PageMove(2 * movepace);
                break;
            case 5:
                nowpage = 2;
                PageMove(3 * movepace);
                Rightbtn.SetActive(true);
                break;
        }
    }

    public void ToPage3()
    {
        switch (nowpage)
        {
            case 1:
                nowpage = 3;
                PageMove(2*(-movepace));
                Leftbtn.SetActive(true);
                break;
            case 2:
                nowpage = 3;
                PageMove(-movepace);
                break;
            case 4:
                nowpage = 3;
                PageMove(movepace);
                break;
            case 5:
                nowpage = 3;
                PageMove(2 * movepace);
                Rightbtn.SetActive(true);
                break;
        }
    }

    public void ToPage4()
    {
        switch (nowpage)
        {
            case 1:
                nowpage = 4;
                PageMove(3 * (-movepace));
                Leftbtn.SetActive(true);
                break;
            case 2:
                nowpage = 4;
                PageMove(2 * (-movepace));
                break;
            case 3:
                nowpage = 4;
                PageMove(-movepace);
                break;
            case 5:
                nowpage = 4;
                PageMove(movepace);
                Rightbtn.SetActive(true);
                break;
        }
    }

    public void ToPage5()
    {
        switch (nowpage)
        {
            case 1:
                nowpage = 5;
                PageMove(4 * (-movepace));
                Leftbtn.SetActive(true);
                Rightbtn.SetActive(false);
                break;
            case 2:
                nowpage = 5;
                PageMove(3 * (-movepace));
                Rightbtn.SetActive(false);
                break;
            case 3:
                nowpage = 5;
                PageMove(2*(-movepace));
                Rightbtn.SetActive(false);
                break;
            case 4:
                nowpage = 5;
                PageMove(-movepace);
                Rightbtn.SetActive(false);
                break;
        }
    }
}

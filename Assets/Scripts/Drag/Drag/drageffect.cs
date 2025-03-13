using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drageffect : MonoBehaviour
{

    public enum State//所处状态
    {
        normal,//常态
        drag,//拖拽
        choose//选择界面
    }
    public State state = new State { };

    public bool allban = false;
    public GameObject globalpanel;

    //单例化状态，控制全局
    static drageffect mInstance;
    public static drageffect Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance=FindObjectOfType<drageffect>();
                if (mInstance == null)
                {

                }
            }
            return mInstance;
        }
    }

    public GameObject[] dragtipob = new GameObject[5];//拖动变深的物体预制件
    public GameObject panel4;

    // Start is called before the first frame update
    void Start()
    {
        globalpanel.SetActive(false);
        allban = false;
        Instance.state=State.normal;//初始化状态
        panel4.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        //实现状态转换
        if (GameObject.Find("Sphere(Clone)"))
        {
            if (GameObject.Find("cancelButton(Clone)"))
            {
                Instance.state = State.choose;
            }
            else
            {
                Instance.state = State.drag;
            }
        }
        else
        {
            Instance.state = State.normal;
        }

        if(Instance.state == State.normal)
        {
            panel4.SetActive(false);
        }
        else
        {
            panel4.SetActive(true);
        }

        if(Instance.state == State.choose)
        {
            globalpanel.SetActive(true);
        }
        else
        {
            globalpanel.SetActive(false);
        }
    }

    public void Allban()
    {
        allban=true;
    }

    public void Allresetban()
    {
        if (allban)
        {
            Invoke("Allbanfalse", 7.6f);
        }
        else
        {
            allban = true;
            Invoke("Allbanfalse", 7.6f);
        }
    }

    private void Allbanfalse()
    {
        allban = false;
    }
}

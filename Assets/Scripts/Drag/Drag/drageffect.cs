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


    // Start is called before the first frame update
    void Start()
    {
        
        Instance.state=State.normal;//初始化状态


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


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public DragCamera maincamera;

    public GameObject[] northrender=new GameObject[4];
    public GameObject[] westrender = new GameObject[2];
    public GameObject[] centerrender = new GameObject[2];
    public GameObject[] westsouthrender = new GameObject[4];
    public GameObject[] southrender = new GameObject[2];

    public GameObject hammer;
    public Animator hammerani;
    public GameObject allsmoke;
    public ParticleSystem[] smoke=new ParticleSystem[4];

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
    public GameObject dragone;

    // Start is called before the first frame update
    void Start()
    {
        
        hammerani.SetBool("ifhit",false);
        hammer.SetActive(false);
        globalpanel.SetActive(false);
        allban = false;
        Instance.state=State.normal;//初始化状态
        panel4.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //判断区域是否满卡


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
            if (maincamera.enabled == true)
            {
                maincamera.enabled = false;
            }
        }
        else
        {
            
            globalpanel.SetActive(false);
            if (maincamera.enabled == false)
            {
                maincamera.enabled = true;
            }
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
            Invoke("Allbanfalse", 3f);
        }
        else
        {
            allban = true;
            Invoke("Allbanfalse", 3f);
        }
    }

    private void Allbanfalse()
    {
        allban = false;
    }
}

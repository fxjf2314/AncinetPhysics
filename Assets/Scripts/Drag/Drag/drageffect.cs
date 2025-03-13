using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drageffect : MonoBehaviour
{

    public enum State//����״̬
    {
        normal,//��̬
        drag,//��ק
        choose//ѡ�����
    }
    public State state = new State { };

    public bool allban = false;
    public GameObject globalpanel;

    //������״̬������ȫ��
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

    public GameObject[] dragtipob = new GameObject[5];//�϶����������Ԥ�Ƽ�
    public GameObject panel4;

    // Start is called before the first frame update
    void Start()
    {
        globalpanel.SetActive(false);
        allban = false;
        Instance.state=State.normal;//��ʼ��״̬
        panel4.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        //ʵ��״̬ת��
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

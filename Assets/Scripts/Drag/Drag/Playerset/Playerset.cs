using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerset : MonoBehaviour
{
    //�������ű�
    static Playerset mInstance;
    public static Playerset Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance=FindObjectOfType<Playerset>();
                if (mInstance == null) 
                {

                }
            }
            return mInstance;
        }
    }

    public int ifbanaudio = 0;//�Ƿ���
    public int audiosound = 50;//������С
    public string playername;
    public bool banaudio=false;

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}

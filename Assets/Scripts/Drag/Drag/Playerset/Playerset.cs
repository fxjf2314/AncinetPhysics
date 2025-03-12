using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerset : MonoBehaviour
{
    //单例化脚本
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

    public int ifbanaudio = 0;//是否静音
    public int audiosound = 50;//声音大小
    public string playername;
    public bool banaudio=false;

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}

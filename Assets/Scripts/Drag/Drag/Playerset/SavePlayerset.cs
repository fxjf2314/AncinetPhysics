using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Loadplayerset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //¥ÊµµÕÊº“…Ë÷√
    public void Saveplayerset()
    {
        PlayerPrefs.SetInt("Ifbanaudio",Playerset.Instance.ifbanaudio);
        PlayerPrefs.SetInt("Audiosound", Playerset.Instance.audiosound);
    }

    //∂¡µµÕÊº“…Ë÷√
    public void Loadplayerset()
    {
        Playerset.Instance.ifbanaudio = PlayerPrefs.GetInt("Ifbanaudio");
        Playerset.Instance.audiosound = PlayerPrefs.GetInt("Audiosound");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammerhit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void killhammer()
    {
        gameObject.SetActive(false);
        drageffect.Instance.hammerani.SetBool("ifhit", false);
        foreach (var smoke in drageffect.Instance.smoke)
        {
            smoke.Stop();
        }
        drageffect.Instance.dragone.SetActive(true);
        drageffect.Instance.dragone=null;
    }
}

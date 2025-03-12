using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class backhome : MonoBehaviour
{
    public GameObject all;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Back()
    {
        Transition.Instance.LoadSceneWithTransition("StartScene");
        Destroy(all);

    }


}

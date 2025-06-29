using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class backhome : MonoBehaviour
{
    public GameObject all;


    public void Back()
    {
        Transition.Instance.LoadSceneWithTransition("StartScene");
        Destroy(all);

    }


}

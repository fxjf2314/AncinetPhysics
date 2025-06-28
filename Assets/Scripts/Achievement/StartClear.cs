using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartClear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AchievementControl.Instance.test = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Opentest()
    {
        AchievementControl.Instance.test = true;
    }
}

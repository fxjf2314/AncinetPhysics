using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance { get; private set; }

    public HashSet<int> targets = new HashSet<int>();

    public void CountCompeleted()
    {
        LockManager.Instance.TotalStarsEarned = targets.Count;
    }

    private void Awake()
    {
        SaveStar();
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        
    }

    public void SaveStar()
    {
        if(LockManager.Instance)
        {
            PlayerPrefs.SetInt("Stars", LockManager.Instance.TotalStarsEarned);
            PlayerPrefs.Save();
        }
        
    }
}
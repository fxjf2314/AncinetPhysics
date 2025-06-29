using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public static LockManager Instance;

    private int totalStarsEarned; // 玩家获得的总星星数

    public int TotalStarsEarned
    {
        get{ return totalStarsEarned; }
        set { totalStarsEarned = value;}
    }

    [SerializeField]
    private TextMeshProUGUI starCount;

    private void Awake()
    {
        totalStarsEarned = PlayerPrefs.GetInt("Stars",0);
        starCount.text = totalStarsEarned.ToString();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddStars(int stars)
    {
        totalStarsEarned += stars;
        // 这里可以添加保存逻辑
    }

    public bool IsLevelUnlocked(Data levelData)
    {
        return levelData.isUnlocked || totalStarsEarned >= levelData.requiredStars;
    }
}

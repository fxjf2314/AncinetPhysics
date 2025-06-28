using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public static LockManager Instance;

    [SerializeField] private int totalStarsEarned; // 玩家获得的总星星数

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

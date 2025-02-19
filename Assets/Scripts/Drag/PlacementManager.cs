using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GameObject prefab1; // Canvas 中的预制体（1）
    public int maxPlacementCount = 5; // 最大放置数量
    private int currentPlacementCount = 0;

    public void IncrementPlacementCount()
    {
        currentPlacementCount++;
        if (currentPlacementCount >= maxPlacementCount)
        {
            prefab1.SetActive(false); // 达到限制数量后，预制体（1）消失
            Debug.Log("PlacementManager: Prefab (1) disabled.");
        }
    }
}
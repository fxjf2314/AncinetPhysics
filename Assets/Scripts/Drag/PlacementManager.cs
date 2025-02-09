using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GameObject prefab1; // Canvas �е�Ԥ���壨1��
    public int maxPlacementCount = 5; // ����������
    private int currentPlacementCount = 0;

    public void IncrementPlacementCount()
    {
        currentPlacementCount++;
        if (currentPlacementCount >= maxPlacementCount)
        {
            prefab1.SetActive(false); // �ﵽ����������Ԥ���壨1����ʧ
            Debug.Log("PlacementManager: Prefab (1) disabled.");
        }
    }
}
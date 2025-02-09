using UnityEngine;
using UnityEngine.UI;

public class RangeConfirmationButton : MonoBehaviour
{
    [Header("Reference")]
    public CanvasClickHandler placementSystem;

    private Button button;
    private bool isActiveState = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnConfirmCancel);
    }

    void Update()
    {
        // 仅在物体已放置且未锁定范围时保持激活
        /*if (placementSystem != null && isActiveState != placementSystem.IsPlacedAndNotFinalized)
        {
            isActiveState = placementSystem.IsPlacedAndNotFinalized;
            gameObject.SetActive(isActiveState);
        }*/
    }

    private void OnConfirmCancel()
    {
        // 调用系统级取消操作
        if (placementSystem != null)
        {
            placementSystem.CancelPlacement();
        }

        // 自动销毁按钮实例
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Cancel button destroyed");
    }
}
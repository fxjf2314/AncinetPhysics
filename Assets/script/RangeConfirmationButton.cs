
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RangeConfirmationButton : MonoBehaviour
{
    [Header("Reference")]
    public DragAndPlaceIn3D placementSystem;

    private Button button;
    private bool isActiveState = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCancel);
        UpdateVisibility();
    }

    void Update()
    {
        // 仅在物体已放置且未锁定范围时保持激活
        if (placementSystem != null && isActiveState != placementSystem.IsPlacedAndNotFinalized)
        {
            isActiveState = placementSystem.IsPlacedAndNotFinalized;
            gameObject.SetActive(isActiveState);
            UpdateVisibility();
        }
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
    private void OnCancel()
    {
        placementSystem?.CancelPlacement();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
        Debug.Log("Cancel button destroyed");
    }
    private void UpdateVisibility()
    {
        gameObject.SetActive(placementSystem != null && placementSystem.IsPlacedAndNotFinalized);
    }
}
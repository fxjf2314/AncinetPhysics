using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button backToHomeButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button resetStarButton;
    [SerializeField] private GameObject levelSelectionArea;

    [Header("Text Settings")]
    [SerializeField] private string gameStartMessage = "即将进入关卡：{0}";
    [SerializeField] private string returnToTitleMessage = "返回主菜单";
    [SerializeField] private string resetStarMessage = "是否要重置所有进度，并返回标题？";

    private SceneSwitch sceneSwitch;

    private void Start()
    {
        // 获取组件引用
        if (!levelSelectionArea.TryGetComponent(out sceneSwitch))
        {
            Debug.LogError("SceneSwitch component not found on levelSelectionArea!");
            return;
        }

        // 设置按钮事件
        startGameButton.onClick.AddListener(OnStartGameClicked);
        backToHomeButton.onClick.AddListener(OnBackToHomeClicked);
        resetStarButton.onClick.AddListener(OnResetClicked);
    }

    private void OnStartGameClicked()
    {
        // 1. 先获取选中的关卡数据
        if (sceneSwitch.TryGetSelectedLevel(out var selectedLevel))
        {
            // 2. 检查关卡是否已解锁
            if (LockManager.Instance.IsLevelUnlocked(selectedLevel))
            {
                sceneSwitch.PrepareLevelData(selectedLevel);
                // 3. 显示确认提示面板
                string message = string.Format(gameStartMessage, selectedLevel.name);
                TipPanelManager.Instance.OpenPanel(BtnFunction.goToGame, message);
            }
            else
            {
                // 关卡未解锁的提示
                TipPanelManager.Instance.OpenPanel(BtnFunction.defaultMode,
                    $"关卡未解锁！");
            }
        }
        else
        {
            Debug.LogWarning("没有选中任何关卡！");
        }
    }

    private void OnBackToHomeClicked()
    {
        // 显示提示面板
        if (TipPanelManager.Instance != null)
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToTitle, returnToTitleMessage);
        }
    }

    private void OnResetClicked()
    {
        // 显示提示面板
        if (TipPanelManager.Instance != null)
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.resetStar, resetStarMessage);
        }
    }

    private void OnDestroy()
    {
        // 清理事件监听
        startGameButton.onClick.RemoveListener(OnStartGameClicked);
        backToHomeButton.onClick.RemoveListener(OnBackToHomeClicked);
    }
}

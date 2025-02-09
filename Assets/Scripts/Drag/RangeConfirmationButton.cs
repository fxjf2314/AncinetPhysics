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
        // ���������ѷ�����δ������Χʱ���ּ���
        /*if (placementSystem != null && isActiveState != placementSystem.IsPlacedAndNotFinalized)
        {
            isActiveState = placementSystem.IsPlacedAndNotFinalized;
            gameObject.SetActive(isActiveState);
        }*/
    }

    private void OnConfirmCancel()
    {
        // ����ϵͳ��ȡ������
        if (placementSystem != null)
        {
            placementSystem.CancelPlacement();
        }

        // �Զ����ٰ�ťʵ��
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Cancel button destroyed");
    }
}
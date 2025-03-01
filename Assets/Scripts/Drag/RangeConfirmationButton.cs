using UnityEngine;
using UnityEngine.UI;

public class RangeConfirmationButton : MonoBehaviour
{
    [Header("Reference")]
    public CanvasClickHandler placementSystem;
    private Button button;
    public bool ifbutton;
    private bool isActiveState = false;

    private void Awake()
    {
        ifbutton = false;
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
            //placementSystem.CancelPlacement();
        }

        // �Զ����ٰ�ťʵ��

        Destroy(gameObject);
        ifbutton = true;
    }

    private void OnDestroy()
    {
        Debug.Log("Cancel button destroyed");
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
public class DragAndPlaceIn3D : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Settings")]
    public GameObject draggableObjectPrefab;
    public LayerMask placementLayer;
    public GameObject cancelButtonPrefab;

    [Header("Range Selection")]
    public GameObject rangeIndicatorPrefab;
    public float gridSize = 1f;

    // ״̬����
    private GameObject currentObject;
    private GameObject cancelButton;
    private bool isDragging = false;
    private bool isPlaced = false;
    private bool isSelectingRange = false;
    private GameObject currentRangeIndicator;
    private GameObject currentPlane; // �޸�Ϊ GameObject ����

    private Color debugColor = new Color(1, 0.5f, 0);
    // �������Թ���ť����
    public bool IsPlacedAndNotFinalized => isPlaced && !isSelectingRange;

    void Update()
    {
        DebugState();
        if (isSelectingRange)
        {
            UpdateRangeSelection();
        }
    }
    private bool GetMousePositionOnPlane(out Vector3 position, bool allowMultiPlane = false)
    {
        position = Vector3.zero;

        // ��ȡ�����������
        Camera mainCam = Camera.main;
        if (!mainCam)
        {
            Debug.LogError("�������δ�ҵ���");
            return false;
        }

        // ��������
        Vector3 mousePos = Input.mousePosition;
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        // ���ӻ����ߣ�������ͼ�пɼ���
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.magenta, 2f);
        Debug.Log($"���������ڣ�[�����λ��]{mainCam.transform.position} " +
                 $"����[{ray.direction}] " +
                 $"��Ļ���꣺[{mousePos}]");

        // ����Ͷ����
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            Debug.Log($"�������У�[����]{hit.collider.name} " +
                     $"λ�ã�[{hit.point}] " +
                     $"�㼶��[{LayerMask.LayerToName(hit.collider.gameObject.layer)}]");

            if (allowMultiPlane)
            {
                position = SnapToGrid(hit.point);
                return true;
            }

            if (hit.collider.gameObject == currentPlane)
            {
                position = SnapToGrid(hit.point);
                Debug.Log($"��Чƽ��λ�ã�[{position}]");
                return true;
            }

            Debug.LogWarning($"��ǰƽ�治ƥ�䣺Ŀ��ƽ��[{currentPlane?.name}] ʵ������[{hit.collider.name}]");
            return false;
        }

        Debug.LogWarning($"δ�����κ�ƽ�棬��ǰ�㼶����ֵ��{placementLayer.value}");
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("��⵽��갴���¼�");

        if (!isPlaced && !isDragging)
        {
            StartDragging();
        }
        else if (IsClickValid())
        {
            StartRangeSelection();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && !isPlaced)
        {
            UpdateObjectPosition();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("[PointerUp] �ɿ��¼�");

        if (isDragging && !isPlaced)
        {
            PlaceObject();
        }
        else if (isSelectingRange)
        {
            FinalizePlacement();
        }
    }

    private void StartDragging()
    {
        try
        {
            currentObject = Instantiate(draggableObjectPrefab);
            Debug.Log("ʵ����������: " + currentObject.name);

            // �Զ���ӱ�Ҫ���
            if (!currentObject.GetComponent<Collider>())
            {
                currentObject.AddComponent<BoxCollider>();
                Debug.Log("���Զ������ײ��");
            }

            isDragging = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("ʵ����ʧ��: " + e.Message);
        }
    }
    private void UpdateObjectPosition()
    {
        if (GetMousePositionOnPlane(out Vector3 pos))
        {
            Debug.Log($"��������λ�õ���[{pos}]");
            currentObject.transform.position = pos;
        }
        else
        {
            Debug.LogWarning("�޷���ȡ��Чƽ��λ�ã����飺\n" +
                            "1. ƽ����ײ���Ƿ����\n" +
                            "2. �Ƿ�����ȷ�㼶\n" +
                            "3. ��������Ƿ���ȷ����");
        }
    }


    private void PlaceObject()
    {
        isDragging = false;
        isPlaced = true;
        Debug.Log($"<color=yellow>���������ɣ�{currentObject.transform.position}</color>");

        // ��ʾƽ�淨�߷���
        StartCoroutine(ShowPlaneNormal());
        ShowCancelButton();
    }
    IEnumerator ShowPlaneNormal()
    {
        Vector3 normal = currentPlane.transform.up;
        for (int i = 0; i < 10; i++)
        {
            Debug.DrawRay(currentPlane.transform.position, normal * 2, Color.red, 1f);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void StartRangeSelection()
    {
        isSelectingRange = true;
        // ������λ�����ɳ�ʼָʾ��
        if (currentObject != null)
        {
            currentRangeIndicator = CreateIndicator(currentObject.transform.position);
        }
    }

    private void UpdateRangeSelection()
    {
        if (GetMousePositionOnPlane(out Vector3 pos, true))
        {
            pos = SnapToGrid(pos);

            if (currentRangeIndicator == null)
            {
                currentRangeIndicator = CreateIndicator(pos);
            }
            else if (Vector3.Distance(pos, currentRangeIndicator.transform.position) > gridSize * 0.5f)
            {
                Destroy(currentRangeIndicator);
                currentRangeIndicator = CreateIndicator(pos);
            }
        }
    }

    private void FinalizePlacement()
    {
        Destroy(currentRangeIndicator);
        isSelectingRange = false;
        isPlaced = false;
        DestroyImmediate(currentObject, true);
        DestroyCancelButton();
    }

    private GameObject CreateIndicator(Vector3 pos)
    {
        return Instantiate(rangeIndicatorPrefab, pos, Quaternion.identity);
    }


    private GameObject GetCurrentPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(currentObject.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            return hit.collider.gameObject; // ���� GameObject
        }
        return null;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            position.y,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }

    private bool IsClickValid()
    {
        return IsPlacedAndNotFinalized &&
               GetMousePositionOnPlane(out _) &&
               currentPlane == GetCurrentPlane(); // ֱ�ӱȽ� GameObject
    }

    private void ShowCancelButton()
    {
        if (cancelButton == null)
        {
            cancelButton = Instantiate(cancelButtonPrefab, FindObjectOfType<Canvas>().transform);
            cancelButton.GetComponent<RangeConfirmationButton>().placementSystem = this;
        }
        cancelButton.SetActive(true);
    }

    public void DestroyCancelButton()
    {
        if (cancelButton != null)
        {
            Destroy(cancelButton);
        }
    }

    public void CancelPlacement()
    {
        Destroy(currentObject);
        DestroyCancelButton();
        ResetState();
    }

    private void ResetState()
    {
        isDragging = false;
        isPlaced = false;
        isSelectingRange = false;
        currentObject = null;
    }
    private void DebugState()
    {
        string state = $"״̬����ק��[{isDragging}] �ѷ���[{isPlaced}] ѡ��Χ[{isSelectingRange}]";
        Debug.DrawLine(Vector3.zero, new Vector3(0, 1, 0), isDragging ? Color.green : Color.red);
    }
    
}
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndPlaceIn3D : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject rangeIndicatorPrefab; // ��Χָʾ��Ԥ����
    public int maxRangeIndicatorCount = 2; // ���Χָʾ������
    public LayerMask planeLayer; // Plane �� Layer

    private bool isDragging = false;
    private bool isPlaced = false;
    private Plane targetPlane;
    private int currentRangeIndicatorCount = 0;

    private void Start()
    {
        targetPlane = new Plane(Vector3.up, Vector3.zero); // ����ƽ����ˮƽ��
        Debug.Log("DragAndPlaceIn3D: Script initialized.");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("DragAndPlaceIn3D: OnPointerDown triggered.");
        if (!isPlaced)
        {
            isDragging = true;
            Debug.Log("DragAndPlaceIn3D: Started dragging.");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // ����Ļ����ת��Ϊ��������
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (targetPlane.Raycast(ray, out float distance))
            {
                transform.position = ray.GetPoint(distance);
                Debug.Log("DragAndPlaceIn3D: Dragging to position: " + transform.position);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            isPlaced = true;
            Debug.Log("DragAndPlaceIn3D: Stopped dragging and placed.");
        }
    }

    private void Update()
    {
        if (isPlaced)
        {
            HandleRangeSelection();
        }
    }

    private void HandleRangeSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, planeLayer))
        {
            if (hit.collider.CompareTag("Plane"))
            {
                Debug.Log("DragAndPlaceIn3D: Hovering over Plane.");
                if (Input.GetMouseButtonDown(0)) // �������������
                {
                    AddRangeIndicator(hit.point);
                }
            }
        }
    }

    private void AddRangeIndicator(Vector3 position)
    {
        if (rangeIndicatorPrefab != null)
        {
            Instantiate(rangeIndicatorPrefab, position, Quaternion.identity);
            currentRangeIndicatorCount++;
            Debug.Log("DragAndPlaceIn3D: Range indicator added. Current count: " + currentRangeIndicatorCount);

            if (currentRangeIndicatorCount >= maxRangeIndicatorCount)
            {
                FinalizePlacement();
            }
        }
    }

    private void FinalizePlacement()
    {
        Debug.Log("DragAndPlaceIn3D: Placement finalized.");
        Destroy(gameObject); // ����Ԥ���壨2��
    }
}
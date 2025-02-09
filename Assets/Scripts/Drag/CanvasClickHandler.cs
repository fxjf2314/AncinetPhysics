using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class CanvasClickHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject prefabToSpawn; // Ԥ���壨2��
    public float riseHeight = 10.0f; // �����߶�
    public float riseDuration = 0.3f; // ��������ʱ��
    public LayerMask planeLayer; // Plane �� Layer
    public float yOffset = 1.0f; // Y ��ƫ���������� Inspector ���޸�
    public float buttonXOffset = 250f; // ˮƽƫ��������������
    public float buttonYOffset = 0f;   // ��ֱƫ����
    public string planeTag = "Area"; // Plane �� Tag ���ƣ����� Inspector ���޸�
    public Material highlightMaterial; // ��������
    public GameObject cancelButtonPrefab; // ȡ����ťԤ����

    private GameObject spawnedPrefab; // ���ɵ�Ԥ���壨2��

    private bool isDragging = false;
    private bool isPlaced = false;
    private bool isFinalized = false; // �Ƿ�����ɷ���
    private bool isDestroyed = false;
    private bool isRising = false;

    private Plane targetPlane;
    private int currentRangeIndicatorCount = 0;
    private List<GameObject> rangeIndicators = new List<GameObject>(); // �洢�������ɵ� RangeIndicator
    private HashSet<Collider> clickedPlanes = new HashSet<Collider>(); // ��¼�ѱ������ Plane
    private Renderer currentRenderer; // ��ǰ������ Renderer
    private Material originalMaterial; // ԭʼ����
    private GameObject cancelButtonInstance; // ȡ����ťʵ��
    private GameObject cancelButton;
    private HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private Vector3 originalPosition; // ��ʼλ��
    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private Vector2 riseStartPosition;
    private Vector3 originalWorldPosition;

    // �Ƿ��ѷ�����δ���
    public bool IsPlacedAndNotFinalized => isPlaced && !isFinalized;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;
        
        originalPosition = transform.position;
        targetPlane = new Plane(Vector3.up, Vector3.zero); // ����ƽ����ˮƽ��
        Debug.Log("CanvasClickHandler: Script initialized.");
    }

    // ���Ԥ���壨1��ʱ����Ԥ���壨2��
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isRising)
        {
            riseStartPosition = rectTransform.anchoredPosition;
            StartCoroutine(RisePrefab());
        }
        if (prefabToSpawn != null && spawnedPrefab == null)
        {
            Vector3 spawnPosition = GetMouseWorldPosition();
            spawnPosition.y += yOffset; // ����� Y ��λ�õĻ���������ƫ����
            spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            isDragging = true;
            Debug.Log("CanvasClickHandler: Prefab (2) spawned at position: " + spawnPosition);
        }
    }
    //���ƶ���
    private IEnumerator RisePrefab()
    {
        isRising = true;

        // �ڵ�ǰλ�û����ϴ�ֱ������ʹ��anchoredPosition��Y�ᣩ
        Vector2 targetPosition = riseStartPosition + new Vector2(0, riseHeight);
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                riseStartPosition,
                targetPosition,
                elapsed / riseDuration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }


    private IEnumerator ResetPosition()
    {
        Vector2 currentPosition = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                currentPosition,
                riseStartPosition, // �ص����ʱ����ʼλ��
                elapsed / riseDuration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = riseStartPosition;
        isRising = false;
    }

    // ��קԤ���壨2��
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && spawnedPrefab != null)
        {
            Vector3 newPosition = GetMouseWorldPosition();
            newPosition.y = spawnedPrefab.transform.position.y; // ���� Y �����겻��
            spawnedPrefab.transform.position = newPosition;
            //Debug.Log("CanvasClickHandler: Dragging prefab (2) to position: " + newPosition);
        }
    }

    // �ɿ����ʱ����Ԥ���壨2��
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging && spawnedPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, planeLayer))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Area"))
                {
                    // ��������� Area ƽ���ϣ���ɷ���
                    isDragging = false;
                    isPlaced = true;
                    isFinalized = false;

                    // ��ʾȡ����ť
                    if (cancelButtonPrefab != null)
                    {
                        ShowCancelButton();
                    }

                    Debug.Log("CanvasClickHandler: Stopped dragging and placed prefab (2) on Area plane.");
                }
                else
                {
                    // ������� Area ƽ���ϣ�����Ԥ���壨2��
                    Destroy(spawnedPrefab);
                    spawnedPrefab = null;
                    isDragging = false;
                    ResetPlacement();
                    Debug.Log("CanvasClickHandler: Prefab (2) placed outside Area plane, destroyed.");
                }
            }
            else
            {
                // ���û�������κ����壬����Ԥ���壨2��
                Destroy(spawnedPrefab);
                spawnedPrefab = null;
                isDragging = false;
                ResetPlacement();
                Debug.Log("CanvasClickHandler: Prefab (2) placed in empty space, destroyed.");
            }
        }

    }

    public void ResetPlacement()
    {
        if (isRising)
        {
            StartCoroutine(ResetPosition());
        }

        // ���������ɵ�Ԥ���壨2��
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);
            spawnedPrefab = null;
        }

        // �������������ɵķ�Χָʾ��
        foreach (var indicator in rangeIndicators)
        {
            if (indicator != null)
            {
                Destroy(indicator);
            }
        }
        rangeIndicators.Clear();

        // ����״̬
        isDragging = false;
        isPlaced = false;
        isFinalized = false;
        currentRangeIndicatorCount = 0;
        clickedPlanes.Clear();

        // �ָ� Area �Ĳ���
        if (currentRenderer != null)
        {
            currentRenderer.material = originalMaterial;
            currentRenderer = null;
        }

        Debug.Log("CanvasClickHandler: Placement reset.");
    }

    private void Update()
    {
        if (isPlaced)
        {
            // ���߼���ͨ����קֱ�Ӵ������
            HandleDragSelection();
        }
        HandleAreaHighlight(); // ���� Area �ĸ����߼�
    }



    //��������߼�
    private void HandleDragSelection()
    {
        // �˴��߼�����DragMaterialController����
    }

    //ȡ����ť����߼�
    private Vector2 GetButtonCanvasPosition()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // �������Ļ����ת��ΪCanvas��������
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            canvas.worldCamera,
            out localPoint
        );

        // ��Ӧ��X��ƫ�ƣ�����Y��ԭʼλ��
        return new Vector2(
            localPoint.x + buttonXOffset, // ֻ�޸�X��
            localPoint.y                  // Y�ᱣ��ԭ��
        );
    }
    private void ShowCancelButton()
    {
        if (cancelButton == null)
        {
            cancelButton = Instantiate(cancelButtonPrefab, FindObjectOfType<Canvas>().transform);
            RectTransform rt = cancelButton.GetComponent<RectTransform>();

            // ����ê��Ϊ���½ǣ���֤Y���׼һ�£�
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(0, 0.5f);
            rt.pivot = new Vector2(0, 0.5f); // ���Ķ������½�

            rt.anchoredPosition = GetButtonCanvasPosition();

            cancelButton.GetComponent<Button>().onClick.AddListener(CancelPlacement);
            cancelButtonInstance = cancelButton;
        }
        cancelButton.SetActive(true);
    }
    private void HideCancelButton()
    {
        if (cancelButton != null)
        {
            cancelButton.SetActive(false);
        }
    }
    
    // ���� Area �ĸ����߼�
    private void HandleAreaHighlight()
    {

        if (isDragging || isPlaced) return;


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Area"))
            {
                var dragController = FindObjectOfType<DragMaterialController>();
                if (dragController != null && dragController.affectedAreas.Contains(hit.collider))
                    return;
                // ���� Area
                if (currentRenderer != null && currentRenderer != hit.collider.GetComponent<Renderer>())
                {
                    currentRenderer.material = originalMaterial;
                }

                currentRenderer = hit.collider.GetComponent<Renderer>();
                originalMaterial = currentRenderer.material;
                currentRenderer.material = highlightMaterial;
            }
            else
            {
                // �ָ� Area �Ĳ���
                if (currentRenderer != null)
                {
                    currentRenderer.material = originalMaterial;
                    currentRenderer = null;
                }
            }
        }
        else
        {
            // �ָ� Area �Ĳ���
            if (currentRenderer != null)
            {
                currentRenderer.material = originalMaterial;
                currentRenderer = null;
            }
        }
    }

    // ��ɷ��ò�����Ԥ���壨1��
    public void FinalizePlacement()
    {
        if (isDestroyed) return;

        // ���ٹ�����DragMaterialController
        var dragController = FindObjectOfType<DragMaterialController>();
        if (dragController != null)
        {
            dragController.ConfirmSelection();
            Destroy(dragController);        // ���ֱ�ӹ�����spawnedPrefab��
        }

        Debug.Log("CanvasClickHandler: Placement finalized.");

        // ����Ԥ���壨1��
        if (gameObject != null)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }

        // ����ȡ����ť
        if (cancelButtonInstance != null)
        {
            HideCancelButton();
            Destroy(cancelButtonInstance);
        }

        // ����״̬
        spawnedPrefab = null;
        isPlaced = false;
        isFinalized = true;
        currentRangeIndicatorCount = 0;

        // ����ѵ���� Plane ��¼
        clickedPlanes.Clear();

        // �ָ����� Area �Ĳ���
        foreach (Collider area in affectedAreas)
        {
            ResetMaterial(area);
        }
        affectedAreas.Clear();
    }

    private void ResetMaterial(Collider collider)
    {
        if (collider != null)
        {
            Renderer renderer = collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }
        }
    }


    // ��ȡ��������������е�λ��
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (targetPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    // ȡ������
    public void CancelPlacement()
    {
        ResetPlacement();
        Debug.Log("CanvasClickHandler: Placement canceled.");
    }
}
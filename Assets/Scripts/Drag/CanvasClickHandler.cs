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
    public float buttonXOffset = 250f; // ˮƽƫ��������������
    public float buttonYOffset = 0f;   // ��ֱƫ����
    public string planeTag = "Area"; // Plane �� Tag ���ƣ����� Inspector ���޸�
    public Material highlightMaterial; // ��������
    public GameObject cancelButtonPrefab; // ȡ����ťԤ����

    public Vector3 cameraTargetPosition = new Vector3(0, 10, -10); // �����Ŀ��λ��
    public Quaternion cameraTargetRotation = Quaternion.Euler(1, 0, 0); // �����Ŀ����ת�Ƕ�
    public float cameraMoveDuration = 0.5f; // ������ƶ�����ʱ��

    private GameObject spawnedPrefab; // ���ɵ�Ԥ���壨2��
    private CardRangeController rangeController; // ��Χ������

    private bool isDragging = false;
    private bool isPlaced = false;
    private bool isFinalized = false; // �Ƿ�����ɷ���
    private bool isDestroyed = false;
    private bool isRising = false;

    private Plane targetPlane;
    private Collider currentAreaCollider;
    private Material originalMaterial; // ԭʼ����
    private GameObject cancelButtonInstance; // ȡ����ťʵ��
    private GameObject cancelButton;
    private HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private Vector3 originalPosition; // ��ʼλ��
    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private Vector2 riseStartPosition;
    private Camera mainCamera;
    private Vector3 cameraOriginalPosition; // �������ʼλ��
    private Quaternion cameraOriginalRotation; // �������ʼ��ת�Ƕ�

    private void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;

        originalPosition = transform.position;
        targetPlane = new Plane(Vector3.up, Vector3.zero); // ����ƽ����ˮƽ��

        // ��¼������ĳ�ʼλ�ú���ת�Ƕ�
        cameraOriginalPosition = Camera.main.transform.position;
        cameraOriginalRotation = Camera.main.transform.rotation;

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
            spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            rangeController = spawnedPrefab.GetComponent<CardRangeController>();
            isDragging = true;
            Debug.Log("CanvasClickHandler: Prefab (2) spawned at position: " + spawnPosition);

            // �ƶ��������Ŀ��λ�ú���ת�Ƕ�
            StartCoroutine(MoveCamera(cameraTargetPosition, cameraTargetRotation));
        }
    }

    // ���ƶ���
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

                    currentAreaCollider = hit.collider;
                    Renderer renderer = currentAreaCollider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        originalMaterial = renderer.material;
                        renderer.material = highlightMaterial; // ���ø�������
                    }

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

        // ����״̬
        isDragging = false;
        isPlaced = false;
        isFinalized = false;
        Debug.Log("CanvasClickHandler: Placement reset.");

        // ���������λ�ú���ת�Ƕ�
        StartCoroutine(MoveCamera(cameraOriginalPosition, cameraOriginalRotation));
    }

    private void Update()
    {
        if (isPlaced)
        {
            // ���߼���ͨ����קֱ�Ӵ������
            HandleDragSelection();
        }
    }

    // ��������߼�
    private void HandleDragSelection()
    {
        // �˴��߼�����DragMaterialController����
    }

    // ȡ����ť����߼�
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
    // ��ɷ��ò�����Ԥ���壨1��
    public void FinalizePlacement()
    {
        if (isDestroyed) return;

        // ������Ƿ�ֹ�ظ�����
        isDestroyed = true;

        // �����첽�������
        StartCoroutine(FinalizationProcess());
    }

    private IEnumerator FinalizationProcess()
    {
        // ��һ�׶Σ��������λ
        yield return StartCoroutine(MoveCamera(cameraOriginalPosition, cameraOriginalRotation));

        // �ڶ��׶Σ�ִ���������
        ExecuteCleanup();

        // �����׶Σ��ӳ�����ȷ��Э�����
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void ExecuteCleanup()
    {
        // �ָ�����
        if (currentAreaCollider != null)
        {
            var renderer = currentAreaCollider.GetComponent<Renderer>();
            if (renderer) renderer.material = originalMaterial;
        }

        // ����UI
        if (cancelButtonInstance)
        {
            Destroy(cancelButtonInstance);
            cancelButtonInstance = null;
        }

        // ����״̬
        spawnedPrefab = null;
        isPlaced = false;
        isFinalized = true;

        // ���������¼
        foreach (var area in affectedAreas)
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

    // ƽ���ƶ������
    private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main camera is null.");
            yield break;
        }

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / cameraMoveDuration);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / cameraMoveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }
}
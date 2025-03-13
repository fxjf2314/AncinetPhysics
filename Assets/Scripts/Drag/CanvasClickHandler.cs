using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;
using TMPro;
using Unity.VisualScripting;

public class CanvasClickHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
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
    public GameObject applicationButtonPrefab; // ȷ�ϰ�ťԤ����
    public GameObject panel;//��ֹ��ק���Ƶ���
    public TextMeshProUGUI cardtext;//�������� 

    public Transform[] model=new Transform[20];//����ģ��

    [Header("Card Info Display")]
    public string cardDetailSceneName = "CardDetailScene";
    public GameObject cardInfoPanelPrefab; // ������Ϣ���Ԥ���壨ʹ�� TMP��
    public float hoverDelay = 1.0f; // �����ӳ�ʱ��
    public string cardArea = "Area"; // ��������
    public string cardEffect = "Effect"; // ����Ч������
    public AudioSource house;//��������
    public AudioSource gun;//ǹ������
    public AudioSource book;//�鼮����

    private GameObject cardInfoPanelInstance; // ������Ϣ���ʵ��
    private bool isHovering = false; // �Ƿ���������
    private Coroutine hoverCoroutine; // ����Э��

    //-----
    public Vector3 cameraTargetPosition = new Vector3(-50, 10, -10); // �����Ŀ��λ��
    public Quaternion cameraTargetRotation = Quaternion.Euler(1, 0, 0); // �����Ŀ����ת�Ƕ�
    public float cameraMoveDuration = 0.5f; // ������ƶ�����ʱ��

    private GameObject spawnedPrefab; // ���ɵ�Ԥ���壨2��
    private CardRangeController rangeController; // ��Χ������

    private bool isDragging = false;
    private bool isPlaced = false;
    private bool isFinalized = false; // �Ƿ�����ɷ���
    private bool isDestroyed = false;
    private bool isRising = false;
    private bool ischoose=false;//�Ƿ����ѡ������

    public bool ifban = false;//�Ƿ����

    public bool isdragone = false;//�Ƿ���̧������ſ���

    public bool ifapplication = false;//�Ƿ�ȷ�Ϸ���

    private Plane targetPlane;
    private Collider[] currentAreaCollider=new Collider[5];
    private Material[] originalMaterial=new Material[5]; // ԭʼ����
    private GameObject cancelButtonInstance; // ȡ����ťʵ��
    private GameObject cancelButton;
    private GameObject applicationButtonInstance; // ȷ�ϰ�ťʵ��
    private GameObject applicationButton;
    private HashSet<Collider> affectedAreas = new HashSet<Collider>();
    private Vector3 originalPosition; // ��ʼλ��
    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private Vector2 riseStartPosition;
    private Camera mainCamera;
    private Vector3 cameraOriginalPosition; // �������ʼλ��
    private Quaternion cameraOriginalRotation; // �������ʼ��ת�Ƕ�
    private RangeConfirmationButton mabutton;
    private RangeConfirmationButton abutton;
    //private GameObject circle;
    //private GameObject circle0;

    private void Start()
    {
        ischoose = false;
        if (cardInfoPanelPrefab != null && cardInfoPanelInstance == null)
        {
            // ʵ�������
            cardInfoPanelInstance = Instantiate(cardInfoPanelPrefab, FindObjectOfType<Canvas>().transform);
            cardInfoPanelInstance.SetActive(false);

            // �������ߴ�
            RectTransform panelRect = cardInfoPanelInstance.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(100, 75);
        }
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;
        /*if (GameObject.Find("Circle") != null)
        {
            circle = GameObject.Find("Circle");
        }*/

        foreach(GameObject draghigh in drageffect.Instance.dragtipob)
        {
            if (draghigh != null)
            {
                draghigh.SetActive(false);
            }
        }

        originalPosition = transform.position;
        targetPlane = new Plane(Vector3.up, Vector3.zero); // ����ƽ����ˮƽ��

        // ��¼������ĳ�ʼλ�ú���ת�Ƕ�
        cameraOriginalPosition = Camera.main.transform.position;
        cameraOriginalRotation = Camera.main.transform.rotation;

        //Debug.Log("CanvasClickHandler: Script initialized.");
    }

    // ���Ԥ���壨1��ʱ����Ԥ���壨2��
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isRising && eventData.button == PointerEventData.InputButton.Right)
        {
            riseStartPosition = rectTransform.anchoredPosition;
            StartCoroutine(RisePrefab());
        }
        if (prefabToSpawn != null && spawnedPrefab == null && eventData.button == PointerEventData.InputButton.Right)
        {
            Vector3 spawnPosition = GetMouseWorldPosition();
            spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            rangeController = spawnedPrefab.GetComponent<CardRangeController>();
            isDragging = true;
            isdragone = true;
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
        // ����Ƿ�Ϊ�Ҽ��϶�
        if (isDragging && spawnedPrefab != null && eventData.button == PointerEventData.InputButton.Right)
        {
            AreaTips.MyInstance.FadeOut();
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
                    if (eventData.button == PointerEventData.InputButton.Right)
                    {
                        isDragging = false;
                    }
                    isPlaced = true;
                    isFinalized = false;

                    //�������÷�Χ
                    HandCard.MyInstance.targetArea=hit.transform.GetComponent<AreaScript>();
                    HandCard.MyInstance.applicationArea[0] = hit.transform.GetComponent<AreaScript>();
                    AreaTips.MyInstance.FadeIn(HandCard.MyInstance.targetArea.GetComponent<Collider>());

                    #region ��ͼ����
                    switch (cardtext.text)
                    {
                        case "���������顷":

                            break;
                        case "�ض���":
                            for(int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                            {
                                HandCard.MyInstance.applicationArea[i] = HandCard.MyInstance.allarea[i];
                            }
                            break;
                        case "������":
                            break;
                        case "���":
                            ischoose = true;
                            Setarea();
                            break;
                        case "������":
                            break;
                        case "������":
                            break;
                        case "������":
                            break;
                        case "���":
                            ischoose = true;
                            Setarea();
                            break;
                        case "��ҩ":
                            for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                            {
                                HandCard.MyInstance.applicationArea[i] = HandCard.MyInstance.allarea[i];
                            }
                            break;
                        case "�س�������ѧ":
                            
                            break;
                        case "��ī�����������ǡ�":
                            ischoose = true;
                            Setarea();
                            break;
                        case "��ľ����":
                            ischoose= true;
                            Setarea();
                            break;
                        case "��ũɣ��Ҫ��":
                            ischoose = true;
                            Setarea();
                            break;
                        case "�򵥻�е��":
                            break;
                        case "˾��":
                            break;
                        case "������":
                            ischoose=true;
                            Setarea();
                            break;
                        case "����ӡˢ��":
                            break;
                        case "����":
                            break;
                        case "��ֽ��":
                            break;
                        case "��ĸ��":
                            break;
                    }
                    #endregion

                    highmat();//����

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
                    Resetmodel();
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
                Resetmodel();
                Destroy(spawnedPrefab);
                spawnedPrefab = null;
                isDragging = false;
                ResetPlacement();
                Debug.Log("CanvasClickHandler: Prefab (2) placed in empty space, destroyed.");
            }
        }
    }

    //��������
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ��ʼ��ͣ
        isHovering = true;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine); // ֹ֮ͣǰ��Э��
        }
        hoverCoroutine = StartCoroutine(ShowCardInfoAfterDelay()); // �����µ�Э��
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ������ͣ
        isHovering = false;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine); // ֹͣЭ��
        }
        if (cardInfoPanelInstance != null)
        {
            cardInfoPanelInstance.SetActive(false); // �������
        }
    }

    private IEnumerator ShowCardInfoAfterDelay()
    {
        yield return new WaitForSeconds(hoverDelay);

        if (isHovering && cardInfoPanelInstance != null)
        {
            Debug.Log("Displaying card info panel.");
            cardInfoPanelInstance.SetActive(true);

            // ���ÿ�����Ϣ����λ��
            RectTransform rt = cardInfoPanelInstance.GetComponent<RectTransform>();
            rt.anchoredPosition = GetMouseCanvasPosition();

            // ���ÿ�����Ϣ
            CardInfoPanel cardInfoPanel = cardInfoPanelInstance.GetComponent<CardInfoPanel>();
            if (cardInfoPanel != null)
            {
                cardInfoPanel.SetCardInfo(cardArea, cardEffect);
            }
        }
    }

    private Vector2 GetMouseCanvasPosition()
    {
        // �������Ļ����ת��Ϊ Canvas ����
        Vector2 mousePosition = Input.mousePosition;
        Canvas canvas = FindObjectOfType<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.worldCamera, out Vector2 canvasPosition);
        return canvasPosition + new Vector2(50, 50); // ���ƫ�������������������
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject && eventData.button == PointerEventData.InputButton.Left)
        {
            // ���뿨�ƽ��ܽ���
            ShowCardDetail();
        }
    }

    private void ShowCardDetail()
    {
        Debug.Log("CanvasClickHandler: Loading card detail scene.");

        // ���ؿ������鳡��
        if (!string.IsNullOrEmpty(cardDetailSceneName))
        {
            SceneManager.LoadScene(cardDetailSceneName);
        }
        else
        {
            Debug.LogError("CardDetailSceneName is not assigned!");
        }
    }

    //--
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

        /*if (circle0 != null)
        {
            Destroy(circle0);
            circle0 = null;
        }*/

        // ����״̬
        isDragging = false;
        isPlaced = false;
        isFinalized = false;
        Debug.Log("CanvasClickHandler: Placement reset.");

        // ���������λ�ú���ת�Ƕ�
        Invoke("resetcamera", 0.1f);

    }

    private void resetcamera()
    {

        if (!GameObject.Find("Sphere(Clone)"))
        {
            StartCoroutine(MoveCamera(cameraOriginalPosition, cameraOriginalRotation));
        }
        CancelInvoke("resetcamera");
    }

    private void returnmat()//�ָ�����
    {
        // �ָ�����
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            //if (currentAreaCollider[i] != null)
            //{
            //var renderer = currentAreaCollider[i].GetComponent<Renderer>();
            // if (renderer)
            // {
             // renderer.material = originalMaterial[i];
             //renderer.gameObject.SetActive(true);
            //}
            // }
            if (HandCard.MyInstance.applicationArea[i] != null)
            {
                HandCard.MyInstance.applicationArea[i].gameObject.SetActive(true);
            }
        }
    }

    private void highmat()//��������
    {
        //�������÷�Χ����
       // Renderer[] renderer = new Renderer[5];
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            if (HandCard.MyInstance.applicationArea[i] != null)
            {
                //   currentAreaCollider[i] = HandCard.MyInstance.applicationArea[i].gameObject.GetComponent<Collider>();
                //  renderer[i] = currentAreaCollider[i].GetComponent<Renderer>();
                //  if (renderer[i] != null)
                // {
                //     originalMaterial[i] = renderer[i].material;
                //     renderer[i].material = highlightMaterial; // ���ø�������
                //      renderer[i].gameObject.SetActive(false);
                //  }
                HandCard.MyInstance.applicationArea[i].gameObject.SetActive(false);
            }
        }
    }

    private void Ban()
    {
        ifban = true;
    }

    private void Resetban()
    {
        ifban = false;
    }

    private void Update()
    {
        #region ������������Ϊ��ʾ
        if (!GameObject.Find("North"))
        {
            drageffect.Instance.dragtipob[0].SetActive(true);
        }
        else
        {
            drageffect.Instance.dragtipob[0].SetActive(false);
        }

        if (!GameObject.Find("West"))
        {
            drageffect.Instance.dragtipob[1].SetActive(true);
        }
        else
        {
            drageffect.Instance.dragtipob[1].SetActive(false);
        }

        if (!GameObject.Find("Center"))
        {
            drageffect.Instance.dragtipob[2].SetActive(true);
        }
        else
        {
            drageffect.Instance.dragtipob[2].SetActive(false);
        }

        if (!GameObject.Find("WestSouth"))
        {
            drageffect.Instance.dragtipob[3].SetActive(true);
        }
        else
        {
            drageffect.Instance.dragtipob[3].SetActive(false);
        }

        if (!GameObject.Find("South"))
        {
            drageffect.Instance.dragtipob[4].SetActive(true);
        }
        else
        {
            drageffect.Instance.dragtipob[4].SetActive(false);
        }
        #endregion
        if (isPlaced)
        {
            // ���߼���ͨ����קֱ�Ӵ������
            HandleDragSelection();
        }

        if (ifban)
        {
            gameObject.GetComponent<Image>().color = Color.gray;//���
            panel.gameObject.SetActive(true);//����
        }
        else
        {
            panel.gameObject.SetActive(false);
            gameObject.GetComponent<Image>().color = Color.white;
        }

        //��һ�ſ��ƺ�ȫ�����
        if (drageffect.Instance.allban&&!ifban)
        {
            Ban();
        }

        if (mabutton != null)
        {
            if (mabutton.ifbutton)
            {
                returnmat();
                Destroy(cancelButton);
                Destroy(applicationButton);
                mabutton.ifbutton = false;
            }
        }

        //ѡ��һ�ſ��Ƶ�ʱ���������Ʊ���ҽ���
        if(drageffect.Instance.state!=drageffect.State.normal && !ifban)
        {
            if (!isDragging)
            {
                Ban();
            }
        }
        else if(drageffect.Instance.state == drageffect.State.normal && ifban&&!drageffect.Instance.allban)
        {
            Resetban();
        }

        //û�п���ʱ�޷�ѡ��
        if (cardtext.text == "" && !ifban)
        {
            Ban();
        }

        if (abutton != null)
        {
            if (abutton.ifbutton)
            {
                returnmat();
                Destroy(cancelButton);
                Destroy(applicationButton);
                abutton.ifbutton = false;
            }
        }

        if (drageffect.Instance.state == drageffect.State.choose&&ischoose)
        {
            #region ��W�л���������
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[1])//��
                {
                    if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[2])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[3];

                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[3])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[2];
                        highmat();
                    }
                }
                else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[2])//��
                {
                    if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[0])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[4];
                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[1])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[0];
                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[4])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[1];
                        highmat();
                    }
                }
                else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[3])//����
                {
                    if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[1])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[4];
                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[4])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[1];
                        highmat();
                    }
                }
                else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[4])//��
                {
                    if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[2])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[3];
                        highmat();
                    }
                    else if (HandCard.MyInstance.applicationArea[1] == HandCard.MyInstance.allarea[3])
                    {
                        returnmat();
                        HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[2];
                        highmat();
                    }
                }
            }
            #endregion
        }

        if (drageffect.Instance.state != drageffect.State.normal&& GameObject.Find("Sphere(Clone)")!=null&&isDragging)
        {
            #region ��ͼ����
            switch (cardtext.text)
            {
                case "���������顷":
                    model[0].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y+10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "�ض���":
                    model[1].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x+20, GameObject.Find("Sphere(Clone)").transform.position.y + 5, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "������":
                    model[2].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "���":
                    model[3].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x-20, GameObject.Find("Sphere(Clone)").transform.position.y + 5, GameObject.Find("Sphere(Clone)").transform.position.z+20);
                    break;
                case "������":
                    model[4].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 16, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "������":
                    model[5].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x-70, GameObject.Find("Sphere(Clone)").transform.position.y + 2, GameObject.Find("Sphere(Clone)").transform.position.z+10);
                    break;
                case "������":
                    model[6].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "���":
                    model[7].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "��ҩ":
                    model[8].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 30, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "�س�������ѧ":
                    model[9].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "��ī�����������ǡ�":
                    model[10].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "��ľ����":
                    model[11].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "��ũɣ��Ҫ��":
                    model[12].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "�򵥻�е��":
                    model[13].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 18, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "˾��":
                    model[14].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x-40, GameObject.Find("Sphere(Clone)").transform.position.y + 5, GameObject.Find("Sphere(Clone)").transform.position.z-40);
                    break;
                case "������":
                    model[15].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 20, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "����ӡˢ��":
                    model[16].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 6, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "����":
                    model[17].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 10, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
                case "��ֽ��":
                    model[18].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 8, GameObject.Find("Sphere(Clone)").transform.position.z+10);
                    break;
                case "��ĸ��":
                    model[19].position = new Vector3(GameObject.Find("Sphere(Clone)").transform.position.x, GameObject.Find("Sphere(Clone)").transform.position.y + 15, GameObject.Find("Sphere(Clone)").transform.position.z);
                    break;
            }
            #endregion
        }
    }

    //����Ŀ���ʼ����ʱĬ������
    private void Setarea()
    {
        if (HandCard.MyInstance.targetArea != null)
        {
            if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[0])//��
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[2];
                ischoose = false;
            }
            else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[1])//��
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[2];
            }
            else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[2])//��
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[1];
            }
            else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[3])//����
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[4];
            }
            else if (HandCard.MyInstance.targetArea == HandCard.MyInstance.allarea[4])//��
            {
                HandCard.MyInstance.applicationArea[1] = HandCard.MyInstance.allarea[3];
            }
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
            applicationButton = Instantiate(applicationButtonPrefab, FindObjectOfType<Canvas>().transform);
            RectTransform rt1 = applicationButton.GetComponent<RectTransform>();

            // ����ê��Ϊ���½ǣ���֤Y���׼һ�£�
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(0, 0.5f);
            rt.pivot = new Vector2(-11.6f, 1f); // ���Ķ������½�

            rt.anchoredPosition = GetButtonCanvasPosition();

            rt1.anchorMin = new Vector2(0, 0.5f);
            rt1.anchorMax = new Vector2(0, 0.5f);
            rt1.pivot = new Vector2(-11.8f, 0.3f); // ���Ķ������½�

            rt1.anchoredPosition = GetButtonCanvasPosition();

            cancelButton.GetComponent<Button>().onClick.AddListener(CancelPlacement);
            applicationButton.GetComponent<Button>().onClick.AddListener(ApplicationPlacement);
            mabutton = cancelButton.GetComponent<RangeConfirmationButton>();
            abutton = applicationButton.GetComponent<RangeConfirmationButton>();
            cancelButtonInstance = cancelButton;
            applicationButtonInstance = applicationButton;
        }
        cancelButton.SetActive(true);
        applicationButton.SetActive(true);
    }

    private void HideCancelButton()
    {
        if (cancelButton != null || applicationButton != null)
        {
            cancelButton.SetActive(false);
            applicationButton.SetActive(false);
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
        returnmat();

        // ����UI
        if (cancelButtonInstance)
        {
            Destroy(cancelButtonInstance);
            Destroy(applicationButtonInstance);
            cancelButtonInstance = null;
            applicationButtonInstance = null;
        }

        // ����״̬
        spawnedPrefab = null;
        isPlaced = false;
        isFinalized = true;

        // ���������¼
        foreach (var area in affectedAreas)
        {
            //ResetMaterial(area);

            returnmat();
        }
        affectedAreas.Clear();
    }

    private void ResetMaterial(Collider collider)
    {
        Renderer[] renderer = new Renderer[5];
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            if (collider != null)
            {
                renderer[i] = collider.GetComponent<Renderer>();
                if (renderer[i] != null)
                {
                    renderer[i].material = originalMaterial[i];
                }
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
        ischoose = false;
        Resetmodel();
        returnmat();
        HandCard.MyInstance.targetArea = null;
        for(int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++) 
        {
            if (HandCard.MyInstance.applicationArea[i] != null)
            {
                HandCard.MyInstance.applicationArea[i] = null;
            }
        }
        AreaTips.MyInstance.FadeOut();
        ResetPlacement();
        Debug.Log("CanvasClickHandler: Placement canceled.");
    }

    //ȷ�Ϸ���
    public void ApplicationPlacement()
    {
        //��������
        #region ��ͼ����--����
        if (isdragone)
        {
            switch (cardtext.text)
            {
                case "���������顷":
                    house.Play();
                    break;
                case "�ض���":
                    house.Play();
                    break;
                case "������":
                    house.Play();
                    break;
                case "���":
                    house.Play();     
                    break;
                case "������":
                    house.Play();
                    break;
                case "������":
                    house.Play();
                    break;
                case "������":
                    house.Play();
                    break;
                case "���":
                    house.Play();
                    break;
                case "��ҩ":
                    house.Play();
                    break;
                case "�س�������ѧ":
                    house.Play();
                    break;
                case "��ī�����������ǡ�":
                    house.Play();
                    break;
                case "��ľ����":
                    house.Play();
                    break;
                case "��ũɣ��Ҫ��":
                    house.Play();
                    break;
                case "�򵥻�е��":
                    house.Play();
                    break;
                case "˾��":
                    house.Play();
                    break;
                case "������":
                    house.Play();
                    break;
                case "����ӡˢ��":
                    house.Play();
                    break;
                case "����":
                    house.Play();
                    break;
                case "��ֽ��":
                    house.Play();
                    break;
                case "��ĸ��":
                    house.Play();
                    break;
            }
            isdragone = false;
        }
        #endregion

        ischoose = false;
        if (ifapplication == false)
        {
            Invoke("resettargetcard", 0.5f);
            ifapplication = true;
            ResetPlacement();
        }
        
    }

    //��ԭĿ������
    void resettargetcard()
    {
        HandCard.MyInstance.targetArea = null;
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            if (HandCard.MyInstance.applicationArea[i] != null)
            {
                HandCard.MyInstance.applicationArea[i] = null;
            }
        }
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

    public void Resetmodel()
    {
        #region ��ͼ����--��ԭģ��
        if (isdragone)
        {
            switch (cardtext.text)
            {
                case "���������顷":
                    model[0].position = new Vector3(124, -500, -82);
                    break;
                case "�ض���":
                    model[1].position = new Vector3(124, -500, -82);
                    break;
                case "������":
                    model[2].position = new Vector3(124, -500, -82);
                    break;
                case "���":
                    model[3].position = new Vector3(124, -500, -82);
                    break;
                case "������":
                    model[4].position = new Vector3(124, -500, -82);
                    break;
                case "������":
                    model[5].position = new Vector3(124, -500, -82);
                    break;
                case "������":
                    model[6].position = new Vector3(124, -500, -82);
                    break;
                case "���":
                    model[7].position = new Vector3(124, -500, -82);
                    break;
                case "��ҩ":
                    model[8].position = new Vector3(124, -500, -82);
                    break;
                case "�س�������ѧ":
                    model[9].position = new Vector3(124, -500, -82);
                    break;
                case "��ī�����������ǡ�":
                    model[10].position = new Vector3(124, -500, -82);
                    break;
                case "��ľ����":
                    model[11].position = new Vector3(124, -500, -82);
                    break;
                case "��ũɣ��Ҫ��":
                    model[12].position = new Vector3(124, -500, -82);
                    break;
                case "�򵥻�е��":
                    model[13].position = new Vector3(124, -500, -82);
                    break;
                case "˾��":
                    model[14].position = new Vector3(124, -500, -82);
                    break;
                case "������":
                    model[15].position = new Vector3(124, -500, -82);
                    break;
                case "����ӡˢ��":
                    model[16].position = new Vector3(124, -500, -82);
                    break;
                case "����":
                    model[17].position = new Vector3(124, -500, -82);
                    break;
                case "��ֽ��":
                    model[18].position = new Vector3(124, -500, -82);
                    break;
                case "��ĸ��":
                    model[19].position = new Vector3(124, -500, -82);
                    break;
            }
            isdragone = false;
        }
        #endregion
    }
}
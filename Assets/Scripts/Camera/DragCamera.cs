using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCamera : MonoBehaviour
{
    public float leftBoundary = -10.0f; // 左边界
    public float rightBoundary = 10.0f; // 右边界
    public float frontBoundary = -10.0f; // 前边界
    public float backBoundary = 10.0f; // 后边界
    public float minY = -5.0f; // Y 轴最小值
    public float maxY = 5.0f; // Y 轴最大值
    public float scrollSpeed = 1.0f; // 鼠标滚轮控制摄像机缩放的速度
    public float focusDistance = 5.0f; // 摄像机与目标物体的距离
    public LayerMask targetLayer; // 目标物体所在的图层（设置为 "Area"）
    public float smoothSpeed;

    private Camera mainCamera;
    private float currentYPosition; // 当前摄像机的 Y 轴位置
    private bool isMoving;

    private void Start()
    {
        mainCamera = Camera.main;
        currentYPosition = mainCamera.transform.position.y; // 保存当前 Y 轴位置
        //targetCameraPosition = mainCamera.transform.position;
    }

    private void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 射线检测
            if (Physics.Raycast(ray, out hit, 500f, targetLayer) && !EventSystem.current.IsPointerOverGameObject())
            {
                // 如果击中了目标物体，调整摄像机的位置
                Vector3 targetPosition = hit.collider.gameObject.transform.position;

                // 计算摄像机的新位置
                Vector3 cameraPosition = targetPosition - mainCamera.transform.forward * focusDistance;
                cameraPosition.y = currentYPosition; // 保持当前 Y 轴位置

                // 应用新的位置
                //mainCamera.transform.position = cameraPosition;
                //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition, smoothSpeed);
                StartCoroutine(MoveCameraToPosition(cameraPosition));
            }
        }

        // 检测鼠标中键拖拽
        if (Input.GetMouseButton(2)) // 按住鼠标中键拖拽
        {
            float deltaX = Input.GetAxis("Mouse X");
            float deltaZ = Input.GetAxis("Mouse Y"); // 使用鼠标 Y 轴作为 Z 轴的移动

            // 将鼠标移动距离转换为世界坐标中的移动距离
            Vector3 moveDistance = new Vector3(deltaX, 0, deltaZ) * mainCamera.orthographicSize * 2.0f;

            // 更新摄像机位置
            Vector3 newPosition = mainCamera.transform.position - mainCamera.transform.rotation * moveDistance;

            // 限制摄像机的移动范围
            newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);
            newPosition.z = Mathf.Clamp(newPosition.z, frontBoundary, backBoundary);

            // 确保 Y 轴位置保持不变
            newPosition.y = currentYPosition;

            // 更新摄像机位置
            mainCamera.transform.position = newPosition;
        }

        // 使用鼠标滚轮控制摄像机在 Z 轴和 Y 轴上靠近或远离
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 scrollPosition = new Vector3(0, -scroll * scrollSpeed, scroll * scrollSpeed);

        // 更新摄像机位置
        Vector3 newScrollPosition = mainCamera.transform.position + scrollPosition;

        // 限制摄像机的 Y 轴位置
        newScrollPosition.y = Mathf.Clamp(newScrollPosition.y, minY, maxY);

        // 限制摄像机的 Z 轴位置
        newScrollPosition.z = Mathf.Clamp(newScrollPosition.z, frontBoundary, backBoundary);

        // 更新摄像机位置
        mainCamera.transform.position = newScrollPosition;

        // 更新当前 Y 轴位置
        currentYPosition = mainCamera.transform.position.y;
    }

    private IEnumerator MoveCameraToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < smoothSpeed)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / smoothSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        isMoving = false;
    }
}

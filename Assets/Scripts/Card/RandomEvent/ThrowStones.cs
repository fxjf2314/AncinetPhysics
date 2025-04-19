using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStones : MonoBehaviour
{
    public Transform launchPoint;    // 发射点
    public GameObject stonePrefab;  // 石头预制体
    public float launchAngle = 45f; // 仰角
    public float launchSpeed = 1f; // 初始速度
    public bool canThrow;
    public int num;

    void Update()
    {
        if (canThrow)
        {
            canThrow = false;
            LaunchStone();

        }
    }

    void LaunchStone()
    {
        // 生成石头
        if (num == 0)
        {
            GameObject stone = Instantiate(stonePrefab, launchPoint.position, Quaternion.identity);
            num++;
            canThrow=false;
            Rigidbody rb = stone.GetComponent<Rigidbody>();

            // 计算发射方向
            //Vector3 horizontalDir = launchPoint.transform.forward;
            Vector3 horizontalDir = new Vector3(launchPoint.transform.forward.x, launchPoint.transform.forward.y, launchPoint.transform.forward.z);
            Quaternion angleRotation = Quaternion.AngleAxis(-launchAngle, transform.right);
            Vector3 launchDir = angleRotation * horizontalDir;

            // 应用初速度
            rb.velocity = launchDir.normalized * launchSpeed;
            StartCoroutine("wait");
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        num = 0;
    }

    // 可选：在Scene视图中绘制方向
    //void OnDrawGizmos()
    //{
    //    if (launchPoint == null) return;

    //    Vector3 horizontalDir = transform.forward;
    //    Quaternion angleRot = Quaternion.Euler(-launchAngle, 0, 0);
    //    Vector3 launchDir = angleRot * horizontalDir;

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(launchPoint.position, launchDir * 10);
    //}
}
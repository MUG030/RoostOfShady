using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalVector : MonoBehaviour
{
    public float rayDistance = 10.0f;
    public float rotationTime = 1.5f; // 角度を変える時間

    private Quaternion targetRotation; // 目標の回転
    private float rotationProgress = -1; // 回転の進行度（-1は回転が始まっていないことを示す）

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Vector3 hitNormal = hit.normal;

            // hit.normalの反対方向を指すように回転を計算
            targetRotation = Quaternion.FromToRotation(transform.forward, -hitNormal) * transform.rotation;

            // 回転の進行度をリセット
            rotationProgress = 0;
        }

        // 回転が始まっている場合
        if (rotationProgress >= 0)
        {
            // 回転の進行度を更新
            rotationProgress += Time.deltaTime / rotationTime;

            // 現在の回転から目標の回転へ徐々に変化
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationProgress);

            // 回転が完了した場合
            if (rotationProgress >= 1)
            {
                // 回転の進行度をリセット
                rotationProgress = -1;
            }
        }
    }
}

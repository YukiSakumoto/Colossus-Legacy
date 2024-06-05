using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // カメラが追従するキャラクター
    [SerializeField] private Vector3 offset;   // キャラクターに対するカメラの相対位置

    void LateUpdate()
    {
        // キャラクターの位置に対してオフセットを追加してカメラの位置を設定
        transform.position = target.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // カメラが追従するキャラクター
    [SerializeField] private Vector3 offset;   // キャラクターに対するカメラの相対位置
    public Vector3 nowPos { set; get; }

    void Update()
    {
        // キャラクターの位置に対してオフセットを追加してカメラの位置を設定
        nowPos = target.position + offset;
        transform.position = nowPos;
    }
}

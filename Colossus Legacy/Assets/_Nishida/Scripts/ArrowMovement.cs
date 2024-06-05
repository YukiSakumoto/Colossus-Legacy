using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        // 親オブジェクト(主人公)のTransform取得
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        // 常に主人公の上で同じ向きになるようにする処理
        transform.rotation = Quaternion.Euler(0, -parentTransform.rotation.y, 0);
    }
}

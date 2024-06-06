using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        // �e�I�u�W�F�N�g(��l��)��Transform�擾
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ɏ�l���̏�œ��������ɂȂ�悤�ɂ��鏈��
        transform.rotation = Quaternion.Euler(0, -parentTransform.rotation.y, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // �J�������Ǐ]����L�����N�^�[
    [SerializeField] private Vector3 offset;   // �L�����N�^�[�ɑ΂���J�����̑��Έʒu
    public Vector3 nowPos { set; get; }

    void Update()
    {
        // �L�����N�^�[�̈ʒu�ɑ΂��ăI�t�Z�b�g��ǉ����ăJ�����̈ʒu��ݒ�
        nowPos = target.position + offset;
        transform.position = nowPos;
    }
}

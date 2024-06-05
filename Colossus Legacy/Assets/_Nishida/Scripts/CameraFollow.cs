using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // �J�������Ǐ]����L�����N�^�[
    [SerializeField] private Vector3 offset;   // �L�����N�^�[�ɑ΂���J�����̑��Έʒu

    void LateUpdate()
    {
        // �L�����N�^�[�̈ʒu�ɑ΂��ăI�t�Z�b�g��ǉ����ăJ�����̈ʒu��ݒ�
        transform.position = target.position + offset;
    }
}

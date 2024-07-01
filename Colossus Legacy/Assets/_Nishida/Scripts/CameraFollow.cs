using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // �J�������Ǐ]����L�����N�^�[
    public Vector3 offset { set; get; }   // �L�����N�^�[�ɑ΂���J�����̑��Έʒu
    public Vector3 nowPos { set; get; }

    private Vector3 nearOffset = new Vector3(0.0f, 3.5f, -4.5f);
    private Vector3 farOffset = new Vector3(0.0f, 9.0f, -13.0f);

    [SerializeField] private float moveTime = 1.0f;
    private float time = 0.0f;


    private void Start()
    {
        offset = farOffset;
    }

    void Update()
    {
        if (GameEvent.Instance.IsScene("GameScene"))
        {
            // �J�����̈ʒu����
            if (target.position.z >= 28.0f)
            {
                if (offset != nearOffset)
                {
                    offset = ChangeOffset(offset, nearOffset, ref time);
                }
                else { time = 0.0f; }
            }
            else if (target.position.z >= -17.5f)
            {
                if (offset != farOffset)
                {
                    offset = ChangeOffset(offset, farOffset, ref time);
                }
            }
            else if (target.position.z >= -48.0f)
            {
                if (offset != nearOffset)
                {
                    offset = ChangeOffset(offset, nearOffset, ref time);
                }
                else { time = 0.0f; }
            }
            else
            {
                if (offset != farOffset)
                {
                    offset = ChangeOffset(offset, farOffset, ref time);
                }
                else { time = 0.0f; }
            }


            // �S���[����X�^�[�g
            if (target.position.z >= -30.0f)
            {
                if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.BattleBefore)
                {
                    GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.Battle);
                }
            }
        }

        // �L�����N�^�[�̈ʒu�ɑ΂��ăI�t�Z�b�g��ǉ����ăJ�����̈ʒu��ݒ�
        nowPos = target.position + offset;
        transform.position = nowPos;
    }


    private Vector3 ChangeOffset(Vector3 _vec, Vector3 _targetVec, ref float _time)
    {
        _time += Time.deltaTime;

        float ratio = _time / moveTime;
        return Vector3.Slerp(_vec, _targetVec, ratio);
    }
}
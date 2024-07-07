using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // カメラが追従するキャラクター
    public Vector3 offset { set; get; }   // キャラクターに対するカメラの相対位置
    public Vector3 nowPos { set; get; }

    private Vector3 nowRotDeg = new Vector3(10.0f, 0.0f, 0.0f);

    [SerializeField] private Vector3 nearOffset = new Vector3(0.0f, 3.5f, -4.5f);
    [SerializeField] private Vector3 farOffset = new Vector3(0.0f, 9.0f, -13.0f);
    [SerializeField] private Vector3 finOffset = new Vector3(0.0f, 5.0f, -6.0f);

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
            // カメラの位置調整
            if (target.position.z >= 28.0f)
            {
                if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.TreasureGet)
                {
                    nowRotDeg = ChangeOffset(nowRotDeg, new Vector3(20.0f, 0.0f, 0.0f), ref time, 0.2f);
                    
                    if (offset != finOffset)
                    {
                        offset = ChangeOffset(offset, finOffset, ref time, 0.2f);
                    }
                    else
                    {
                        time = 0.0f;
                        return;
                    }
                }
                else
                {
                    if (offset != nearOffset)
                    {
                        offset = ChangeOffset(offset, nearOffset, ref time);
                    }
                    else
                    { time = 0.0f; }
                }
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


            // ゴレーム戦スタート
            if (target.position.z >= -30.0f)
            {
                if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.None ||
                    GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.BattleBefore)
                {
                    GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.Battle);
                }
            }
        }

        // キャラクターの位置に対してオフセットを追加してカメラの位置を設定
        nowPos = target.position + offset;
        transform.position = nowPos;
        transform.rotation = Quaternion.Euler(nowRotDeg);
    }


    private Vector3 ChangeOffset(Vector3 _vec, Vector3 _targetVec, ref float _time, float _speed = 1.0f)
    {
        _time += Time.deltaTime * _speed;

        float ratio = _time / moveTime;
        return Vector3.Slerp(_vec, _targetVec, ratio);
    }
}
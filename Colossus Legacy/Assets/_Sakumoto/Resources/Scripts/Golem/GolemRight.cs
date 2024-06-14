using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Timeline;
using static UnityEngine.GraphicsBuffer;

public class GolemRight : Golem
{
    public int m_nowAttackId = -1;
    private int m_nextAttackId = -1;

    public GameObject m_hand;
    private GameObject m_instantiateObj;
    private float m_protrusionNowTime = 0.0f;

    private Vector3 _forward = Vector3.forward;

    // ================================
    // �^�[�Q�b�g�ւ̉�]�␳�n
    // ================================
    // �r�̌����Z�o�p
    [SerializeField] private GameObject m_myShoulder;
    [SerializeField] private GameObject m_myHand;

    private Vector3 m_initVec;

    private Quaternion m_initRot;           // �����p�x
    private Quaternion m_targetRot;         // �Ώۂւ̊p�x�ۑ��p

    [SerializeField] private float m_smoothTime = 15.0f;    // �ڕW�l�ɓ��B����܂ł̂����悻�̎���
    private float m_nowTime = 0.0f;

    [SerializeField] private float m_limitDeg = -30.0f;     // �p�x�̌��E


    void Start()
    {
        // m_weakCollider = GameObject.Find("R_Elixir_Collider_Weak").GetComponent<WeakPoint>();

        attackManager = GetComponent<AttackManager>();

        attackManager.AddAttack(0, "SwingDown", new Vector2(10.0f, 22.0f), 1.0f);
        attackManager.AddAttack(1, "SwingDown", new Vector2(10.0f, 22.0f), 5.0f);
        attackManager.AddAttack(2, "Palms", new Vector2(0.0f, 15.0f), 5.0f, true);
        attackManager.AddAttack(3, "Protrusion", new Vector2(22.0f, 55.0f), 8.0f);
        attackManager.AddAttack(4, "Protrusion", new Vector2(0.0f, 15.0f), 8.0f);

        // �����p�x�̕ۑ�
        m_initRot = this.transform.rotation;
        m_targetRot = this.transform.rotation;

        // �r�̏����x�N�g����ۑ�
        if (m_myShoulder && m_myHand)
        {
            m_initVec = m_myHand.transform.position - m_myShoulder.transform.position;
            m_initVec = Vector3.ProjectOnPlane(m_initVec, Vector3.up);
        }
    }


    void Update()
    {
        if (!m_alive) { return; }

        SmoothAngleChanger(m_targetRot, this.transform.rotation);

        if (m_instantiateObj)
        {
            m_protrusionNowTime += Time.deltaTime;
            if (m_protrusionNowTime <= 0.17f + 0.5f && m_protrusionNowTime >= 0.5f)
            {
                Vector3 pos = m_instantiateObj.transform.position;
                pos.y += 35.0f * Time.deltaTime;
                m_instantiateObj.transform.position = pos;
            }
            else if (m_protrusionNowTime >= 0.83f + 0.5f)
            {
                Vector3 pos = m_instantiateObj.transform.position;
                pos.y -= 35.0f * Time.deltaTime;
                m_instantiateObj.transform.position = pos;
            }
        }

        if (m_stop) { return; }

        m_nowAttackId = AttackSet(DistanceToTarget(), m_nextAttackId);

        if (m_nowAttackId == m_nextAttackId)
        {
            m_nextAttackId = -1;
        }

        // �f�o�b�O�p
        WeakHit();
    }


    public bool AttackWait()
    {
        if (m_nowAttackId == 2)
        {
            m_stop = true;
        }

        return m_stop;
    }


    public void AttackStart()
    {
        m_stop = false;
        m_nextAttackId = -1;
        attackManager.AttackStart();
    }


    private void ProtrusionActionOn()
    {
        m_protrusionNowTime = 0.0f;

        Vector3 targetPos = m_target.transform.position;
        targetPos.y -= 7.0f;

        Quaternion rot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        if (!m_hand) { Debug.Log("�n���h���Ȃ���"); return; }
        m_instantiateObj = Instantiate(m_hand, targetPos, rot, this.transform);
    }


    private void ProtrusionActionOff()
    {
        if (!m_instantiateObj) { Debug.Log("�n���h���Ȃ���"); return; }
        Destroy(m_instantiateObj);
    }


    // ��]�p�x�̏�����
    private void InitRotation()
    {
        m_targetRot = m_initRot;
        m_nowTime = 0.0f;
    }


    // �^�[�Q�b�g�ւ̊p�x��ۑ�
    private void CollectionTargetAngle()
    {
        // ���g�̕����x�N�g��
        Vector3 myVec = m_myHand.transform.position - m_myShoulder.transform.position;

        // �^�[�Q�b�g�ւ̕����x�N�g��
        Vector3 targetVec = m_target.transform.position - m_myShoulder.transform.position;

        // �x�N�g���𕽖ʁiY����]�j�ɓ��e
        myVec = Vector3.ProjectOnPlane(myVec, Vector3.up);
        targetVec = Vector3.ProjectOnPlane(targetVec, Vector3.up);

        // 2�̃x�N�g������p�x�Z�o
        float deg = Vector3.SignedAngle(myVec, targetVec, Vector3.up);

        // deg > 0.0f �̂Ƃ������p�x�� 2 �{�ɂȂ��Ă������ߕ␳
        if (deg > 0.0f) deg *= 0.5f;

        // �����ʒu����p�x���Z�o�i�p�x����p�j
        float initDeg = Vector3.SignedAngle(m_initVec, targetVec, Vector3.up);
        if (initDeg > 0.0f) initDeg *= 0.5f;

        // �����ʒu�������̒l�܂ŊJ���Ȃ��悤�ɂ���i30���܂Łj
        if (initDeg < m_limitDeg)
        {
            deg = m_limitDeg - (initDeg - deg);
        }

        // �p�x�����]�����擾
        m_targetRot.eulerAngles += new Vector3(0.0f, deg, 0.0f);

        m_nowTime = 0.0f;
    }


    // �Ȃ߂炩�ɒǏ]�����鏈��
    private void SmoothAngleChanger(Quaternion _targetRot, Quaternion _nowRot)
    {
        float timeRatio = 0.0f;
        if (m_nowTime != 0.0f)
        {
            timeRatio = m_nowTime / m_smoothTime;
        }
        if (timeRatio > 1.0f) { return; }

        this.transform.rotation = Quaternion.Lerp(
            _nowRot,
            _targetRot,
            timeRatio);

        m_nowTime += Time.deltaTime;
    }


    public void SetNextAttackId(int _id) { m_nextAttackId = _id; }
}

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
    [SerializeField] private int m_nextAttackId = -1;

    [SerializeField] private GameObject m_hand;
    private GameObject m_instantiateObj;
    private float m_protrusionNowTime = 0.0f;
    private GameObject m_attackAreaIns;
    bool m_handColEnable = true;

    [SerializeField] private GameObject m_attackAreaHand;
    [SerializeField] private GameObject m_attackAreaSwing;
    [SerializeField] private GameObject m_attackAreaRampage;

    [SerializeField] private Transform m_areaSwing;
    [SerializeField] private Transform m_areaRampage;


    private Vector3 _forward = Vector3.forward;

    // ================================
    // �^�[�Q�b�g�ւ̉�]�␳�n
    // ================================
    // �r�̌����Z�o�p
    [SerializeField] private GameObject m_myShoulder;
    [SerializeField] private GameObject m_myHand;

    //private Vector3 m_initVec;

    private Quaternion m_initRot;           // �����p�x
    private Quaternion m_targetRot;         // �Ώۂւ̊p�x�ۑ��p
    private Quaternion m_nowRot;            // ���݂̕����x�N�g��

    [SerializeField] private float m_smoothTime = 15.0f;    // �ڕW�l�ɓ��B����܂ł̂����悻�̎���
    private float m_nowTime = 0.0f;

    //[SerializeField] private float m_limitDeg = -30.0f;     // �p�x�̌��E


    void Start()
    {
        m_skinMesh = GetComponent<SkinMesh>();
        m_dissolve = GetComponent<Dissolve>();
        m_camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraQuake>();

        attackManager = GetComponent<AttackManager>();

        attackManager.AddAttack(0, "Palms", new Vector2(0.0f, 15.0f), 1.0f, true);
        attackManager.AddAttack(1, "SwingDown", new Vector2(0.0f, 22.0f), 3.0f);
        attackManager.AddAttack(2, "Protrusion", new Vector2(22.0f, 55.0f), 2.0f);

        // �����p�x�̕ۑ�
        m_initRot = this.transform.rotation;
        m_targetRot = this.transform.rotation;

        m_attackCnt = 0;
        m_palmsMinCnt = 3;
    }


    void Update()
    {
        if (!m_alive)
        {
            PartsDestroy();
            return;
        }

        SmoothAngleChanger(m_targetRot, m_nowRot);

        if (m_instantiateObj)
        {
            m_protrusionNowTime += Time.deltaTime;
            if (m_protrusionNowTime <= 0.17f)
            {
                Vector3 pos = m_instantiateObj.transform.position;
                pos.y += 35.0f * Time.deltaTime;
                m_instantiateObj.transform.position = pos;
            }
            else if (m_protrusionNowTime > 0.17f && m_protrusionNowTime < 0.83f)
            {
                if (m_handColEnable)
                {
                    Collider collider = m_instantiateObj.GetComponentInChildren<Collider>();
                    collider.enabled = false;
                    m_handColEnable = false;
                }
            }
            else
            {
                Vector3 pos = m_instantiateObj.transform.position;
                pos.y -= 35.0f * Time.deltaTime;
                m_instantiateObj.transform.position = pos;
            }
        }

        // ��~�� or �_���[�W���Ȃ烊�^�[��
        if (m_stop || m_damageFlg) { return; }

        // �U�����Z�b�g
        int attackId;
        if (m_attackCnt > m_palmsMinCnt || m_palmsFlg)
        {
            attackId = AttackSet(DistanceToTarget(), m_nextAttackId);
        }
        else
        {
            if (m_nowAttackId != -1)
            {
                List<int> list = attackManager.GetAttackIdList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == 0) { list.Remove(i); break; }
                }
                m_nextAttackId = list[Random.Range(0, list.Count)];
                if (attackManager.IsAttackRange(m_nextAttackId, DistanceToTarget())) { m_nextAttackId = -1; }
            }
            attackId = AttackSet(DistanceToTarget(), m_nextAttackId);
            if (m_nowAttackId != attackId) { m_attackCnt++; }
        }
        m_nowAttackId = attackId;

        // ���̍U�����Z�b�g����Ă���Ƃ��A�ҋ@��ԂɂȂ邩������
        if (!m_attackWait)
        {
            AttackWait();
        }

        // �U����ԂȂ�U�������擾
        if (m_nowAttackId != -1)
        {
            m_nowAttackName = attackManager.GetAttackName();
        }

        // ���ɃZ�b�g�����U�������݂̍U���Ȃ玟�̃Z�b�g��j��
        if (m_nowAttackId == m_nextAttackId)
        {
            m_nextAttackId = -1;
        }
    }


    public bool AttackWait()
    {
        if (m_nowAttackId == 0)
        {
            m_stop = true;
            m_attackWait = true;
        }

        return m_attackWait;
    }


    public void AttackStart()
    {
        m_stop = false;
        m_attackWait = false;
        m_nextAttackId = -1;
        attackManager.AttackStart();
    }


    private void AttackAreaHandOn()
    {
        Vector3 targetPos = m_target.transform.position;
        targetPos.y -= 4.0f;

        Quaternion rot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        m_attackAreaIns = Instantiate(m_attackAreaHand, targetPos, rot, this.transform);

        m_handColEnable = true;
    }

    private void AttackAreaSwing()
    {
        Vector3 targetPos = m_areaSwing.position;

        m_attackAreaIns = Instantiate(m_attackAreaSwing, targetPos, new Quaternion(), this.transform);
    }

    private void AttackAreaRampage()
    {
        Vector3 targetPos = m_areaRampage.position;

        m_attackAreaIns = Instantiate(m_attackAreaRampage, targetPos, new Quaternion(), this.transform);
    }

    private void DestroyAttackArea()
    {
        if (m_attackAreaIns)
        {
            Destroy(m_attackAreaIns);
        }
    }


    private void ProtrusionActionOn()
    {
        m_protrusionNowTime = 0.0f;

        Vector3 targetPos;
        if (m_attackAreaIns)
        {
            targetPos = m_attackAreaIns.transform.position;
            targetPos.y -= 3.0f;
        }
        else
        {
            targetPos = m_target.transform.position;
            targetPos.y -= 7.0f;
        }

        Quaternion rot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        if (!m_hand) { Debug.Log("�n���h���Ȃ���"); return; }
        m_instantiateObj = Instantiate(m_hand, targetPos - new Vector3(0.0f, 0.0f, 0.6f), rot, this.transform);
    }


    private void ProtrusionActionOff()
    {
        if (!m_instantiateObj) { Debug.Log("�n���h���Ȃ���"); return; }
        Destroy(m_instantiateObj);
        DestroyAttackArea();
    }


    // ��]�p�x�̏�����
    private void InitRotation()
    {
        m_nowRot = this.transform.rotation;
        m_targetRot = m_initRot;
        m_nowTime = 0.0f;
    }


    // �^�[�Q�b�g�ւ̊p�x��ۑ�
    private void CollectionTargetAngle()
    {
        m_nowRot = this.transform.rotation;

        // �U���|�C���g�܂ł̋���
        float attackPointDist, targetDist;

        // ���g�̕����x�N�g��
        Vector3 myVec = m_myHand.transform.position - m_myShoulder.transform.position;
        attackPointDist = myVec.magnitude;
        myVec = myVec.normalized;

        // �^�[�Q�b�g�ւ̕����x�N�g��
        Vector3 targetVec = m_target.transform.position - m_myShoulder.transform.position;
        targetDist = targetVec.magnitude;
        targetVec = targetVec.normalized;

        // �x�N�g���𕽖ʁiY����]�j�ɓ��e
        myVec = Vector3.ProjectOnPlane(myVec, Vector3.up);
        targetVec = Vector3.ProjectOnPlane(targetVec, Vector3.up);

        // 2�̃x�N�g������p�x�Z�o
        float deg = Vector3.SignedAngle(myVec, targetVec, Vector3.up);


        // �^�[�Q�b�g�Ƃ̋����ɉ����Ă���Ɋp�x��␳
        if (attackPointDist > targetDist)
        {
            float dist = attackPointDist - targetDist;
            deg -= dist * 3.0f;
        }


        // �p�x�����]�����擾
        m_targetRot.eulerAngles += new Vector3(0.0f, deg, 0.0f);

        m_nowTime = 0.0f;
    }


    // �Ȃ߂炩�ɒǏ]�����鏈��
    private void SmoothAngleChanger(Quaternion _targetRot, Quaternion _nowRot)
    {
        if (_targetRot == _nowRot) { return; }

        float timeRatio = 0.0f;
        if (m_nowTime != 0.0f)
        {
            timeRatio = m_nowTime / m_smoothTime;
        }
        if (timeRatio > 1.0f) { return; }

        this.transform.rotation = Quaternion.Slerp(
            _nowRot,
            _targetRot,
            timeRatio);

        m_nowTime += Time.deltaTime;
    }


    public void SetNextAttackId(int _id) { m_nextAttackId = _id; }
}

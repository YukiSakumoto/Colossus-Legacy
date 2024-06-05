using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private CharacterMovement characterMovement;

    [SerializeField] private string m_targetWeakTag = "EnemyWeak";
    [SerializeField] private string m_targetBodyTag = "Enemy";

    private bool m_hitFlg = false;
    private bool m_deflectedFlg = false;

    void Start()
    {
        // CharacterMovementを持つGameObjectを検索
        GameObject characterObject = GameObject.Find("HumanMale_Character");

        // CharacterMovementがアタッチされているGameObjectからCharacterMovementコンポーネントを取得
        characterMovement = characterObject.GetComponent<CharacterMovement>();
    }

    void LateUpdate()
    {
        m_hitFlg = false;
        m_deflectedFlg = false;
    }

    void OnTriggerEnter(Collider _other)
    {
        // 攻撃モーション中のみ判定を行うようにする
        if (characterMovement.Getm_swordMoveFlg || (characterMovement.Getm_secondSwordAttackFlg && !m_deflectedFlg))
        {
            if (_other.gameObject.CompareTag(m_targetWeakTag))
            {
                //Destroy(_other.gameObject);
                Debug.Log("Sword Hit! Enemy Ni Damage!");
                m_hitFlg = true;
            }

            if (_other.gameObject.CompareTag(m_targetBodyTag))
            {
                //Destroy(_other.gameObject);
                Debug.Log("Sword Deflected! Damage ga Hairanaiyo!");
                m_deflectedFlg = true;
            }
        }
    }

    public bool Getm_hitFlg
    {
        get { return m_hitFlg; }
    }

    public bool Getm_deflectedFlg
    {
        get { return m_deflectedFlg; }
    }
}
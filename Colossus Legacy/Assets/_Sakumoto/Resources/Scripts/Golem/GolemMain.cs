using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMain : Golem
{
    EffekseerEffectAsset m_effect;
    // EffekseerHandle m_effectHandle;
    List<EffekseerHandle> m_effekseerHandleList;

    Vector3 m_effectPos;
    Quaternion m_effectRot;
    Vector3 m_effectScale = Vector3.one;

    // エフェクトの数（透明度が高かったため、エフェクトを大量に放つことでデカレーザーにしてみる）
    public int m_effectNum = 5;

    public float m_shotTime = 10.0f;
    public float m_coolTime = 10.0f;

    private float m_laserTime = 2.0f;
    private bool m_isLaser = false;

    [SerializeField] private Collider m_laserCollider;

    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        //attackManager.AddAttack(0, "BigLaser", 100.0f, 10.0f);

        // エフェクトを取得する。
        m_effect = Resources.Load<EffekseerEffectAsset>("BigLaser");

        m_effekseerHandleList = new List<EffekseerHandle>();
    }


    void Update()
    {

    }


    public void BigLaserEffect()
    {
        // エフェクトの位置設定
        m_effectPos = transform.position;
        m_effectPos.y += 4.0f;
        m_effectPos.x -= 1.5f;

        // エフェクトの回転設定
        m_effectRot = transform.rotation;
        m_effectRot *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

        for (int i = 0; i < m_effectNum; i++)
        {
            EffekseerHandle effectHandle = EffekseerSystem.PlayEffect(m_effect, m_effectPos);
            effectHandle.SetRotation(m_effectRot);

            m_effekseerHandleList.Add(effectHandle);
        }
    }


    private bool EndEffect()
    {
        bool result = false;

        m_effectScale *= 0.95f;

        for (int i = 0; i < m_effectNum; i++)
        {
            m_effekseerHandleList[i].SetScale(m_effectScale);
        }

        m_laserCollider.enabled = false;

        if (m_effectScale.x <= 0.01f)
        {
            m_effectScale = Vector3.one;
            m_effekseerHandleList.Clear();

            result = true;
        }

        return result;
    }


    public void SpecialAttack()
    {
        m_laserTime -= Time.deltaTime;
        if (m_isLaser)
        {
            if (m_laserTime < m_shotTime - 2.2f)
            {
                m_laserCollider.enabled = true;
            }
        }


        if (m_laserTime <= 0.0f)
        {
            // クールダウン中…
            if (m_isLaser)
            {
                bool fadeOut = false;
                
                fadeOut = EndEffect();
                if (fadeOut)
                {
                    m_laserTime = m_coolTime;
                    m_isLaser = false;
                }
            }
            // ビームを放つ！
            else
            {
                BigLaserEffect();

                m_laserTime = m_shotTime;
                m_isLaser = true;
            }
        }
    }


    // 鎧破壊
    public void ArmorDestroy()
    {
        // できればディゾルブでやってみたい！
        foreach (Transform child in transform)
        {
            if (child.name == "Armors")
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}

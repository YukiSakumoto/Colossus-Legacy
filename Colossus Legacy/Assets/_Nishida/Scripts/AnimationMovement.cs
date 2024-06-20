using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationMovement : MonoBehaviour
{
    public Animator animator;

    private CharacterMovement m_characterMovement;
    private Sword m_sword;
    private const float m_deathAnimationMax = 1.66f;
    private const float m_hitStopSetTime = 0.2f;

    private float m_deathAnimation = 0f;
    private float m_hitStopTime = 0f;
    private float m_animationSpeed = 1f;

    private bool m_hitStopFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // CharacterMovementを持つGameObjectを検索
        GameObject characterObject = GameObject.Find("HumanMale_Character");
        GameObject swordObject = GameObject.Find("Sword Variant");

        // CharacterMovementがアタッチされているGameObjectからCharacterMovementコンポーネントを取得
        m_characterMovement = characterObject.GetComponent<CharacterMovement>();
        m_sword = swordObject.GetComponent<Sword>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_characterMovement.Getm_attackAnimeFlg)
        {
            if (!m_characterMovement.Getm_weaponFlg)
            {
                animator.SetTrigger("p_Sword");
            }
            else
            {
                animator.SetTrigger("p_Bow");
            }
        }

        if (m_characterMovement.Getm_subAttackAnimeFlg)
        {
            animator.SetTrigger("p_Bomb");
        }

        if (m_characterMovement.Getm_secondSwordAttackAnimeFlg)
        {
            animator.SetTrigger("p_SecondSword");
        }

        if (m_characterMovement.Getm_rollAnimeFlg)
        {
            animator.SetTrigger("p_Roll");
        }

        if (m_characterMovement.Getm_damageAnimeFlg)
        {
            animator.SetTrigger("p_Damage");
        }

        if(m_characterMovement.Getm_blownAwayAnimeFlg)
        {
            animator.SetTrigger("p_blownAway");
        }

        if (m_characterMovement.Getm_deathFlg)
        {
            if (m_deathAnimation <= m_deathAnimationMax)
            {
                m_deathAnimation += Time.deltaTime;
                // 死亡時にアニメーションの停止位置固定(倒れたところでモーションを固定する)
                animator.Play("Death", -1, (m_deathAnimation / m_deathAnimationMax));
            }
        }
        else
        {
            animator.SetBool("b_Death", false);
        }

        if (m_characterMovement.Getm_walkAnimeFlg)
        {
            animator.SetBool("b_Run", true);
        }
        else
        {
            animator.SetBool("b_Run", false);
        }

        if(m_sword.Getm_deflectedFlg)
        {
            animator.SetTrigger("p_Deflected");
        }

        if (m_sword.Getm_hitFlg)
        {
            m_hitStopTime = m_hitStopSetTime;
            m_animationSpeed = animator.speed;
            animator.speed = 0f;
            m_hitStopFlg = true;
        }
    }

    private void FixedUpdate()
    {
        if (m_hitStopFlg)
        {
            m_hitStopTime -= Time.deltaTime;
            if (m_hitStopTime < 0)
            {
                animator.speed = m_animationSpeed;
                m_hitStopFlg = false;
            }
        }
    }
}

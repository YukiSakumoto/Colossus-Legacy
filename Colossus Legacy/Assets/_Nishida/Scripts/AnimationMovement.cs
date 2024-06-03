using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovement : MonoBehaviour
{
    public Animator animator;

    private CharacterMovement characterMovement;
    private const float m_deathAnimationMax = 1.66f;
    private float m_deathAnimation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // CharacterMovementを持つGameObjectを検索
        GameObject characterObject = GameObject.Find("HumanMale_Character");

        // CharacterMovementがアタッチされているGameObjectからCharacterMovementコンポーネントを取得
        characterMovement = characterObject.GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterMovement.Getm_attackAnimeFlg)
        {
            if (!characterMovement.Getm_weaponFlg)
            {
                animator.SetTrigger("p_Sword");
            }
            else
            {
                animator.SetTrigger("p_Bow");
            }
        }

        if (characterMovement.Getm_subAttackAnimeFlg)
        {
            animator.SetTrigger("p_Bomb");
        }

        if (characterMovement.Getm_secondSwordAttackAnimeFlg)
        {
            animator.SetTrigger("p_SecondSword");
        }

        if (characterMovement.Getm_rollAnimeFlg)
        {
            animator.SetTrigger("p_Roll");
        }

        if (characterMovement.Getm_damageAnimeFlg)
        {
            animator.SetTrigger("p_Damage");
        }

        if(characterMovement.Getm_blownAwayAnimeFlg)
        {
            animator.SetTrigger("p_blownAway");
        }

        if (characterMovement.Getm_deathFlg)
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

        if (characterMovement.Getm_walkAnimeFlg)
        {
            animator.SetBool("b_Run", true);
        }
        else
        {
            animator.SetBool("b_Run", false);
        }
    }
}

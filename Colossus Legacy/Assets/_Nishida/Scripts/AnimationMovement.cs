using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovement : MonoBehaviour
{
    public Animator animator;

    private CharacterMovement m_characterMovement;
    private Sword m_sword;
    private const float m_deathAnimationMax = 1.66f;
    private float m_deathAnimation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // CharacterMovement������GameObject������
        GameObject characterObject = GameObject.Find("HumanMale_Character");
        GameObject swordObject = GameObject.Find("Sword Variant");

        // CharacterMovement���A�^�b�`����Ă���GameObject����CharacterMovement�R���|�[�l���g���擾
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
                // ���S���ɃA�j���[�V�����̒�~�ʒu�Œ�(�|�ꂽ�Ƃ���Ń��[�V�������Œ肷��)
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
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationMovement : MonoBehaviour
{
    public Animator animator;

    [SerializeField] CharacterManager m_manager;
    //private CharacterMovement m_characterMovement;
    //private Sword m_sword;
    private const float m_deathAnimationMax = 1.66f;
    private const float m_hitStopSetTime = 0.2f;

    private float m_deathAnimation = 0f;
    private float m_hitStopTime = 0f;
    private float m_animationSpeed = 1f;

    private bool m_hitStopFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!m_manager)
        {
            Debug.Log("AnimationMovement:manager is Null");
        }

        animator = GetComponent<Animator>();
        // CharacterMovement������GameObject������
        //GameObject characterObject = GameObject.Find("HumanMale_Character");
        //GameObject swordObject = GameObject.Find("Sword Variant");

        // CharacterMovement���A�^�b�`����Ă���GameObject����CharacterMovement�R���|�[�l���g���擾
        //m_characterMovement = characterObject.GetComponent<CharacterMovement>();
        //m_sword = swordObject.GetComponent<Sword>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_manager.Getm_attackAnimeFlg)
        {
            if (!m_manager.Getm_weaponFlg)
            {
                animator.SetTrigger("p_Sword");
            }
            else
            {
                animator.SetTrigger("p_Bow");
            }
        }

        if (m_manager.Getm_subAttackAnimeFlg)
        {
            animator.SetTrigger("p_Bomb");
        }

        if (m_manager.Getm_secondSwordAttackAnimeFlg)
        {
            animator.SetTrigger("p_SecondSword");
        }

        if (m_manager.Getm_rollAnimeFlg)
        {
            animator.SetTrigger("p_Roll");
        }

        if (m_manager.Getm_damageAnimeFlg)
        {
            animator.SetTrigger("p_Damage");
        }

        if(m_manager.Getm_blownAwayAnimeFlg)
        {
            animator.SetTrigger("p_blownAway");
        }

        if (m_manager.Getm_deathFlg)
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

        if (m_manager.Getm_walkAnimeFlg)
        {
            animator.SetBool("b_Run", true);
        }
        else
        {
            animator.SetBool("b_Run", false);
        }

        if(m_manager.Getm_deflectedFlg)
        {
            animator.SetTrigger("p_Deflected");
        }

        if (m_manager.Getm_hitFlg)
        {
            m_hitStopTime = m_hitStopSetTime;
            m_animationSpeed = animator.speed;
            animator.speed = 0f;
            m_hitStopFlg = true;
        }

        if(m_manager.Getm_joyAnimeFlg)
        {
            animator.SetTrigger("p_Joy");
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

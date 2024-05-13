using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovement : MonoBehaviour
{
    public Animator animator;

    private CharacterMovement flagProvider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // FlagProviderを持つGameObjectを検索
        GameObject flagProviderObject = GameObject.Find("HumanMale_Character Variant");

        // FlagProviderがアタッチされているGameObjectからFlagProviderコンポーネントを取得
        flagProvider = flagProviderObject.GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flagProvider.Getm_attackFlg)
        {
            if (!flagProvider.Getm_weaponFlg)
            {
                animator.SetTrigger("p_Sword");
            }
            else
            {
                animator.SetTrigger("p_Bow");
            }
        }

        if (flagProvider.Getm_subAttackFlg)
        {
            animator.SetTrigger("p_Bomb");
        }

        if (flagProvider.Getm_rollFlg)
        {
            animator.SetTrigger("p_Roll");
        }

        if (Input.GetKey(KeyCode.Backspace))
        {
            animator.SetTrigger("p_Damage");
        }

        if (Input.GetKey(KeyCode.Return))
        {
            animator.SetTrigger("p_Damage");
            animator.SetBool("b_Death", true);
        }

        if (flagProvider.Getm_walkFlg)
        {
            animator.SetBool("b_Run", true);
        }
        else
        {
            animator.SetBool("b_Run", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_movement;
    [SerializeField] private AnimationMovement m_animation;
    [SerializeField] private EffectMovement m_effect;
    [SerializeField] private ThrowBomb m_throwBomb;
    [SerializeField] private PlayerSoundPlay m_sound;
    [SerializeField] private Sword m_sword;
    [SerializeField] private Bow m_bow;
    [SerializeField] private GameObject m_arrowObj;
    [SerializeField] private GameObject m_bombObj;

    private Arrow m_arrow;
    private Bomb m_bomb;

    private bool m_moveWalkAnimeFlg = false;
    private bool m_moveRollAnimeFlg = false;
    private bool m_moveWeaponFlg = false;
    private bool m_moveAttackAnimeFlg = false;
    private bool m_moveSecondSwordAttackAnimeFlg = false;
    private bool m_moveDamageAnimeFlg = false;
    private bool m_moveBlownAwayAnimeFlg = false;
    private bool m_downAnimeFlg = false;
    private bool m_pushUpAnimeFlg = false;
    private bool m_moveDeathFlg = false;
    private bool m_moveSwordMoveFlg = false;
    private bool m_moveSecondSwordAttackFlg = false;
    private bool m_moveJoyFlg = false;
    private bool m_moveJoyAnimeFlg = false;
    private bool m_swordHitFlg = false;
    private bool m_swordDeflectedFlg = false;
    private bool m_bombThrowCheckFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!m_movement)
        {
            Debug.Log("Character Manager:movement is Null");
        }

        if (!m_animation)
        {
            Debug.Log("Character Manager:animation is Null");
        }

        if (!m_effect)
        {
            Debug.Log("Character Manager:effect is Null");
        }

        if (!m_throwBomb)
        {
            Debug.Log("Character Manager:throwbomb is Null");
        }

        if (!m_sound)
        {
            Debug.Log("Character Manager:sound is Null");
        }

        if (!m_sword)
        {
            Debug.Log("Character Manager:sword is Null");
        }

        if (!m_bow)
        {
            Debug.Log("Character Manager:bow is Null");
        }

        if(!m_arrowObj)
        {
            Debug.Log("Character Manager:arrow is Null");
        }
        else
        {
            m_arrow = m_arrowObj.GetComponent<Arrow>();
        }

        if (!m_bombObj)
        {
            Debug.Log("Character Manager:bomb is Null");
        }
        else
        {
            m_bomb = m_bombObj.GetComponent<Bomb>();
        }
    }

    private void Update()
    {
        CharacterMovementFlgSet();
        SwordFlgSet();
        ThrowBombFlgSet();
    }

    private void CharacterMovementFlgSet()
    {
        m_moveWalkAnimeFlg = m_movement.Getm_walkAnimeFlg;
        m_moveRollAnimeFlg = m_movement.Getm_rollAnimeFlg;
        m_moveWeaponFlg = m_movement.Getm_weaponFlg;
        m_moveAttackAnimeFlg = m_movement.Getm_attackAnimeFlg;
        m_moveSecondSwordAttackAnimeFlg = m_movement.Getm_secondSwordAttackAnimeFlg;
        m_moveDamageAnimeFlg = m_movement.Getm_damageAnimeFlg;
        m_moveBlownAwayAnimeFlg = m_movement.Getm_blownAwayAnimeFlg;
        m_downAnimeFlg = m_movement.Getm_downAnimeFlg;
        m_pushUpAnimeFlg = m_movement.Getm_pushUpAnimeFlg;
        m_moveDeathFlg = m_movement.Getm_deathFlg;
        m_moveSwordMoveFlg = m_movement.Getm_swordMoveFlg;
        m_moveSecondSwordAttackFlg = m_movement.Getm_secondSwordAttackFlg;
        m_moveJoyFlg = m_movement.Getm_joyFlg;
        m_moveJoyAnimeFlg = m_movement.Getm_joyAnimeFlg;
    }

    private void SwordFlgSet()
    {
        m_swordHitFlg = m_sword.Getm_hitFlg;
        m_swordDeflectedFlg = m_sword.Getm_deflectedFlg;
    }

    private void ThrowBombFlgSet()
    {
        m_bombThrowCheckFlg = m_throwBomb.Getm_bombThrowCheckFlg;
    }

    public void SetHit(int _damage, bool _knockBack, bool _down, bool _pushup)
    {
        m_movement.Hit(_damage, _knockBack, _down, _pushup);
    }

    public bool Getm_walkAnimeFlg
    {
        get { return m_moveWalkAnimeFlg; }
    }
    public bool Getm_rollAnimeFlg
    {
        get { return m_moveRollAnimeFlg; }
    }
    public bool Getm_weaponFlg
    {
        get { return m_moveWeaponFlg; }
    }
    public bool Getm_attackAnimeFlg
    {
        get { return m_moveAttackAnimeFlg; }
    }
    public bool Getm_secondSwordAttackAnimeFlg
    {
        get { return m_moveSecondSwordAttackAnimeFlg; }
    }
    public bool Getm_damageAnimeFlg
    {
        get { return m_moveDamageAnimeFlg; }
    }
    public bool Getm_blownAwayAnimeFlg
    {
        get { return m_moveBlownAwayAnimeFlg; }
    }
    public bool Getm_downAnimeFlg
    {
        get { return m_downAnimeFlg; }
    }
    public bool Getm_pushUpAnimeFlg
    {
        get { return m_pushUpAnimeFlg; }
    }
    public bool Getm_deathFlg
    {
        get { return m_moveDeathFlg; }
    }

    public bool Getm_swordMoveFlg
    {
        get { return m_moveSwordMoveFlg; }
    }

    public bool Getm_secondSwordAttackFlg
    {
        get { return m_moveSecondSwordAttackFlg; }
    }

    public bool Getm_joyFlg
    {
        get { return m_moveJoyFlg; }
    }

    public bool Getm_joyAnimeFlg
    {
        get { return m_moveJoyAnimeFlg; }
    }

    public bool Getm_hitFlg
    {
        get { return m_swordHitFlg; }
    }

    public bool Getm_deflectedFlg
    {
        get { return m_swordDeflectedFlg; }
    }
    public bool Getm_bombThrowCheckFlg
    {
        get { return m_bombThrowCheckFlg; }
    }
}
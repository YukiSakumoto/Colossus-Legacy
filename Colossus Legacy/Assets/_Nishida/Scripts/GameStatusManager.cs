using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatusManager : MonoBehaviour
{
    public static GameStatusManager Instance { get; private set; }

    Golem m_golem;
    CharacterManager m_characterManager;

    int m_golemDamage = 0;
    int m_characterDamage = 0;

    bool m_golemDamageFlg = false;

    bool m_characterDamageFlg = false;
    bool m_characterKnockBackFlg = false;
    bool m_characterDownFlg = false;
    bool m_characterPushUpFlg = false;


    enum PlayerDamage // ��l�����󂯂�_���[�W��
    {
        small = 30,  // ���_���[�W
        medium = 50, // ���_���[�W
        big = 80,    // ��_���[�W
        death = 100  // �����U��
    }

    enum GolemDamage // �S�[�������󂯂�_���[�W��
    {
        Sword = 50, // ���_���[�W
        Arrow = 20, // ��_���[�W
        Bomb = 50,  // ���e�_���[�W
    }

    enum Recovery // �񕜗�
    {
        small = 20,  // ����
        medium = 50, // ����
        big = 70,    // ���
        full = 100   // ���S��
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneReset();
    }

    private void SceneReset()
    {
        GameObject golemObj = GameObject.Find("Golem");
        GameObject characterObj = GameObject.Find("HumanMale_Character");

        m_golem = golemObj.GetComponent<Golem>();
        m_characterManager = characterObj.GetComponent<CharacterManager>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneReset();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if(m_golemDamageFlg)
        {
            m_golem.SetHit(m_golemDamage);
            m_golemDamageFlg = false;
            m_golemDamage = 0;
        }
        
        if(m_characterDamageFlg)
        {
            m_characterManager.SetHit(m_characterDamage, m_characterKnockBackFlg, m_characterDownFlg, m_characterPushUpFlg);
            m_characterDamageFlg = false;
            m_characterDamage = 0;
        }
    }

    public void DamageGolemSword()
    {
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Sword;
        Debug.Log("GameStatusManager: DamageGolemSword");
    }
    public void DamageGolemArrow()
    {
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Arrow;
        Debug.Log("GameStatusManager: DamageGolemArrow");
    }

    public void DamageGolemBomb()
    {
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Bomb;
        Debug.Log("GameStatusManager: DamageGolemBomb");
    }

    public void DamagePlayerPushUP() // ������˂��グ��U��
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDownFlg = false;
        m_characterPushUpFlg = true;
        m_characterDamage = (int)PlayerDamage.small;
        Debug.Log("GameStatusManager: DamagePlayerPushUP");
    }

    public void DamagePlayerDown() // �ォ�牟���ׂ��n�̍U��
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDownFlg = true;
        m_characterPushUpFlg = false;
        m_characterDamage = (int)PlayerDamage.medium;
        Debug.Log("GameStatusManager: DamagePlayerDown");
    }

    public void DamagePlayerPressHand() // �����U��
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDownFlg = false;
        m_characterPushUpFlg = false;
        m_characterDamage = (int)PlayerDamage.big;
        Debug.Log("GameStatusManager: DamagePlayerPressHand");
    }

    public void DamagePlayerBeam() // �r�[���U��
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = true;
        m_characterDownFlg = false;
        m_characterPushUpFlg = false;
        m_characterDamage = (int)PlayerDamage.death;
        Debug.Log("GameStatusManager: DamagePlayerBeam");
    }

    public void DamagePlayerBomb() // ���e�U��(����)
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = true;
        m_characterDownFlg = false;
        m_characterPushUpFlg = false;
        m_characterDamage = (int)PlayerDamage.medium;
        Debug.Log("GameStatusManager: DamagePlayerBomb");
    }
}

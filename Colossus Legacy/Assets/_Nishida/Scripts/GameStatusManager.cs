using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatusManager : MonoBehaviour
{
    public static GameStatusManager Instance { get; private set; }

    Golem m_golem;
    CharacterManager m_characterManager;
    SoundPlay m_soundPlay;

    int m_golemDamage = 0;
    int m_characterDamage = 0;

    bool m_golemDamageFlg = false;

    bool m_characterDamageFlg = false;
    bool m_characterKnockBackFlg = false;
    bool m_characterDownFlg = false;
    bool m_characterPushUpFlg = false;
    bool m_superhardFlg = false;


    enum PlayerDamage // 主人公が受けるダメージ量
    {
        small = 30,  // 小ダメージ
        medium = 50, // 中ダメージ
        big = 80,    // 大ダメージ
        death = 100  // 即死攻撃
    }

    enum GolemDamage // ゴーレムが受けるダメージ量
    {
        Sword = 20, // 剣ダメージ
        Arrow = 10, // 矢ダメージ
        Bomb = 20,  // 爆弾ダメージ
    }

    enum Recovery // 回復量
    {
        small = 20,  // 小回復
        medium = 50, // 中回復
        big = 70,    // 大回復
        full = 100   // 完全回復
    }

    // 発表用デバッグキーをONにするか
    public bool m_debugFlg = false;

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

        //SceneReset();
    }

    private void SceneReset()
    {
        // 現在のシーンを取得
        Scene currentScene = SceneManager.GetActiveScene();

        // シーンの名前を取得
        string sceneName = currentScene.name;

        if (sceneName == "GameScene" || sceneName == "Training Scene")
        {
            GameObject characterObj = GameObject.Find("HumanMale_Character");
            if (!characterObj) { return; }
            m_characterManager = characterObj.GetComponent<CharacterManager>();

            GameObject soundObj = GameObject.Find("Audio");
            if (!soundObj) { return; }
            m_soundPlay = soundObj.GetComponent<SoundPlay>();

            if (sceneName == "GameScene")
            {
                if(GameManager.Instance.m_difficulty != GameManager.Difficulty.SuperHard)
                {
                    m_superhardFlg = false;
                }
                else
                {
                    m_superhardFlg = true;
                }

                GameObject golemObj = GameObject.Find("Golem");
                if (!golemObj) { return; }
                m_golem = golemObj.GetComponent<Golem>();
            }
        }
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
            m_characterManager.SetHit(m_characterDamage, m_characterKnockBackFlg, m_characterDownFlg, m_characterPushUpFlg, m_debugFlg);
            m_characterDamageFlg = false;
            m_characterDamage = 0;
        }

        if (m_debugFlg && Input.GetKeyDown(KeyCode.Backspace))
        {
            m_characterManager.SetHit((int)PlayerDamage.death, m_characterKnockBackFlg, m_characterDownFlg, m_characterPushUpFlg);
            m_characterDamageFlg = false;
            m_characterDamage = 0;
        }
    }

    public void DamageGolemSword()
    {
        if (!m_golem) { return; }
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Sword;
        Debug.Log("GameStatusManager: DamageGolemSword");
    }
    public void DamageGolemArrow()
    {
        if (!m_golem) { return; }
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Arrow;
        Debug.Log("GameStatusManager: DamageGolemArrow");
    }

    public void DamageGolemBomb()
    {
        if (!m_golem) { return; }
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Bomb;
        Debug.Log("GameStatusManager: DamageGolemBomb");
    }

    public void DamagePlayerPushUP() // 下から突き上げる攻撃
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDownFlg = false;
        m_characterPushUpFlg = true;
        if (!m_superhardFlg)
        {
            m_characterDamage = (int)PlayerDamage.small;
        }
        else
        {
            m_characterDamage = (int)PlayerDamage.death;
        }
        Debug.Log("GameStatusManager: DamagePlayerPushUP");
        m_soundPlay.SoundDamageKnockBackStart();
    }

    public void DamagePlayerDown() // 上から押し潰す系の攻撃
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDownFlg = true;
        m_characterPushUpFlg = false;
        if (!m_superhardFlg)
        {
            m_characterDamage = (int)PlayerDamage.medium;
        }
        else
        {
            m_characterDamage = (int)PlayerDamage.death;
        }
        Debug.Log("GameStatusManager: DamagePlayerDown");
        m_soundPlay.SoundDamageCrush();
    }

    public void DamagePlayerPressHand() // 合掌攻撃
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDownFlg = false;
        m_characterPushUpFlg = false;
        m_characterDamage = (int)PlayerDamage.death;
        Debug.Log("GameStatusManager: DamagePlayerPressHand");
        m_soundPlay.SoundDamageHeavy();
    }

    public void DamagePlayerBeam() // ビーム攻撃
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = true;
        m_characterDownFlg = false;
        m_characterPushUpFlg = false;
        m_characterDamage = (int)PlayerDamage.death;
        Debug.Log("GameStatusManager: DamagePlayerBeam");
    }

    public void DamagePlayerBomb() // 爆弾攻撃(自爆)
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = true;
        m_characterDownFlg = false;
        m_characterPushUpFlg = false;
        if (!m_superhardFlg)
        {
            m_characterDamage = (int)PlayerDamage.medium;
        }
        else
        {
            m_characterDamage = (int)PlayerDamage.death;
        }
        Debug.Log("GameStatusManager: DamagePlayerBomb");
    }
}
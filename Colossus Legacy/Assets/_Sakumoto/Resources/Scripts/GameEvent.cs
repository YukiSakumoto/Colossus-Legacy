using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEvent : MonoBehaviour
{
    public static GameEvent Instance { get; private set; }

    public enum GameEventState
    {
        None,
        BattleBefore,       // 戦闘前
        Battle,             // 戦闘中
        PlayerDead,         // プレイヤーの死亡
        PlayerWin,          // ゴーレム討伐完了
        TreasureGet,        // 宝箱の獲得
        Score,              // スコア表示
        GameFin,            // ゲーム終了
    }
    public GameEventState m_nowEvent;
    private GameEventState m_changeEvent;

    private Scene m_nowScene;

    public float m_clearTime = 0.0f;
    public int m_deathCount = 0;


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

        Reset();
    }

    void Update()
    {
        // 現在のシーンを取得
        m_nowScene = SceneManager.GetActiveScene();

        if (IsScene("GameScene"))
        {
            // イベント中の更新処理
            if (m_nowEvent == GameEventState.BattleBefore || m_nowEvent == GameEventState.Battle ||
                m_nowEvent == GameEventState.PlayerDead )
            {
                m_clearTime += Time.deltaTime;
            }


            // イベント切り替えの処理
            if (m_changeEvent != GameEventState.None && m_nowEvent != m_changeEvent)
            {
                // ゴーレム戦の前なら
                if (m_changeEvent == GameEventState.BattleBefore)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Before);
                }
                // バトルイベントに切り替わったら
                else if (m_changeEvent == GameEventState.Battle)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Battle, 0.5f);
                }
                // ゴーレムを討伐したら
                else if (m_changeEvent == GameEventState.PlayerWin)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Win, 1.5f);
                }
                // プレイヤーが死亡したら
                else if (m_changeEvent == GameEventState.PlayerDead)
                {
                    m_deathCount++;
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Stop, 1.0f);
                }
                else if (m_changeEvent == GameEventState.GameFin)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Stop, 3.0f);
                    GameManagerGameScene.Instance.GameSecneFin();
                    GameManager.Instance.SetclearFlg(true); // ゲームクリア判定
                }

                // 現在のイベントを変更
                m_nowEvent = m_changeEvent;
                m_changeEvent = GameEventState.None;
            }
        }
    }


    // イベント切り替え
    public void ChangeEvent(GameEvent.GameEventState _state)
    {
        if (m_nowEvent == _state) { return; }

        m_changeEvent = _state;
    }


    // 切り替えフレーム中のイベントを取得
    public GameEventState GetChangeEvent()
    {
        return m_changeEvent;
    }


    // イベント切り替えフレーム中かを取得
    public bool IsChangingEvent(GameEventState _event = GameEventState.None)
    {
        if (m_changeEvent == GameEventState.None) { return false; }

        if (_event != GameEventState.None)
        {
            if (m_changeEvent != _event) { return false; }
        }

        return true;
    }


    // シーン管理
    public bool IsScene(string _name)
    {
        // 現在のシーンを取得
        if (m_nowScene != SceneManager.GetSceneByName(_name)) { return false; }
        return true;
    }


    // リセット
    public void Reset()
    {
        m_nowScene = SceneManager.GetActiveScene();
        m_nowEvent = GameEventState.None;
        m_changeEvent = GameEventState.None;

        if (IsScene("GameScene"))
        {
            ChangeEvent(GameEventState.BattleBefore);
        }
    }


    public void ResetScore()
    {
        m_clearTime = 0.0f;
        m_deathCount = 0;
    }
}

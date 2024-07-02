using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEvent : Singleton<GameEvent>
{
    public enum GameEventState
    {
        None,
        BattleBefore,       // 戦闘前
        Battle,             // 戦闘中
        PlayerDead,         // プレイヤーの死亡
        PlayerWin,          // ゴーレム討伐完了
        TreasureGet,        // 宝箱の獲得
    }
    public GameEventState m_nowEvent;
    private GameEventState m_changeEvent;

    private Scene m_nowScene;


    void Start()
    {
        m_nowEvent = GameEventState.BattleBefore;
        m_changeEvent = GameEventState.None;
    }

    void Update()
    {
        // 現在のシーンを取得
        m_nowScene = SceneManager.GetActiveScene();

        if (IsScene("GameScene"))
        {
            // イベント切り替えの処理
            if (m_changeEvent != GameEventState.None && m_nowEvent != m_changeEvent)
            {
                // バトルイベントに切り替わったら
                if (m_changeEvent == GameEventState.Battle)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Battle, 0.5f);
                }
                // ゴーレムを討伐したら
                else if (m_changeEvent == GameEventState.PlayerWin)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Win, 1.5f);
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
}

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
        BattleBefore,       // �퓬�O
        Battle,             // �퓬��
        PlayerDead,         // �v���C���[�̎��S
        PlayerWin,          // �S�[������������
        TreasureGet,        // �󔠂̊l��
        GameFin,
    }
    public GameEventState m_nowEvent;
    private GameEventState m_changeEvent;

    private Scene m_nowScene;


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
        // ���݂̃V�[�����擾
        m_nowScene = SceneManager.GetActiveScene();

        if (IsScene("GameScene"))
        {
            // �C�x���g�؂�ւ��̏���
            if (m_changeEvent != GameEventState.None && m_nowEvent != m_changeEvent)
            {
                // �S�[������̑O�Ȃ�
                if (m_changeEvent == GameEventState.BattleBefore)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Before);
                }
                // �o�g���C�x���g�ɐ؂�ւ������
                else if (m_changeEvent == GameEventState.Battle)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Battle, 0.5f);
                }
                // �S�[�����𓢔�������
                else if (m_changeEvent == GameEventState.PlayerWin)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Win, 1.5f);
                }
                // �v���C���[�����S������
                else if (m_changeEvent == GameEventState.PlayerDead)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Stop, 1.0f);
                }
                else if (m_changeEvent == GameEventState.GameFin)
                {
                    GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Stop, 4.0f);
                    GameManagerGameScene.Instance.GameSecneFin();
                }

                // ���݂̃C�x���g��ύX
                m_nowEvent = m_changeEvent;
                m_changeEvent = GameEventState.None;
            }
        }
    }


    // �C�x���g�؂�ւ�
    public void ChangeEvent(GameEvent.GameEventState _state)
    {
        if (m_nowEvent == _state) { return; }

        m_changeEvent = _state;
    }


    // �؂�ւ��t���[�����̃C�x���g���擾
    public GameEventState GetChangeEvent()
    {
        return m_changeEvent;
    }


    // �C�x���g�؂�ւ��t���[���������擾
    public bool IsChangingEvent(GameEventState _event = GameEventState.None)
    {
        if (m_changeEvent == GameEventState.None) { return false; }

        if (_event != GameEventState.None)
        {
            if (m_changeEvent != _event) { return false; }
        }

        return true;
    }


    // �V�[���Ǘ�
    public bool IsScene(string _name)
    {
        // ���݂̃V�[�����擾
        if (m_nowScene != SceneManager.GetSceneByName(_name)) { return false; }
        return true;
    }


    // ���Z�b�g
    public void Reset()
    {
        m_nowScene = SceneManager.GetActiveScene();
        m_nowEvent = GameEventState.None;
        m_changeEvent = GameEventState.None;

        if (IsScene("GameScene"))
        {
            GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Before);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : Singleton<GameEvent>
{
    public enum GameEventState
    {
        None,
        BattleBefore,       // �퓬�O
        Battle,             // �퓬��
        PlayerDead,         // �v���C���[�̎��S
        PlayerWin,          // �S�[������������
        TreasureGet,        // �󔠂̊l��
    }
    public GameEventState m_nowEvent;
    private GameEventState m_changeEvent;


    void Start()
    {
        m_nowEvent = GameEventState.BattleBefore;
        m_changeEvent = GameEventState.None;
    }

    void Update()
    {
        // �C�x���g�؂�ւ��̏���
        if (m_changeEvent != GameEventState.None && m_nowEvent != m_changeEvent)
        {
            // �o�g���C�x���g�ɐ؂�ւ������
            if (m_changeEvent == GameEventState.Battle)
            {
                GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Battle, 0.5f);
            }
            // �S�[�����𓢔�������
            else if (m_changeEvent == GameEventState.PlayerWin)
            {
                GameBGM.Instance.ChangeBGMState(GameBGM.BGMState.Win, 1.5f);
            }

            // ���݂̃C�x���g��ύX
            m_nowEvent = m_changeEvent;
            m_changeEvent = GameEventState.None;
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
}

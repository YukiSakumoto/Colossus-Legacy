using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class StageClear : MonoBehaviour
{
    private AudioSource m_audio;
    [SerializeField] private AudioClip m_scoreClip;
    [SerializeField] private AudioClip m_totalClip;

    [SerializeField] private CharacterMovement m_player;

    [SerializeField] private RectTransform m_clearTime;
    [SerializeField] private RectTransform m_deathCnt;
    [SerializeField] private RectTransform m_playerHp;
    [SerializeField] private RectTransform m_totalScore;
    [SerializeField] private RectTransform m_rank;
    [SerializeField] private Image m_returnTitle;


    private float m_deltaTime = 0.0f;
    [SerializeField] float m_fadeInTime = 1.0f;
    [SerializeField] float m_nextStateTime = 0.5f;
    [SerializeField] float m_scoreTime = 0.5f;
    [SerializeField] float m_rankTime = 2.0f;
    [SerializeField] float m_returnTime = 0.5f;

    [SerializeField] float m_rankRotSpeed = 1.0f;


    private enum ScoreState
    {
        None,
        ClearTime,
        DeathCnt,
        PlayerHp,
        TotalScore,
        Rank,
        Fin
    }
    [SerializeField] private ScoreState m_state;

    private void OnEnable()
    {
        ClearStart();
    }

    void Update()
    {
        if (GameEvent.Instance.m_nowEvent != GameEvent.GameEventState.Score &&
            GameEvent.Instance.m_nowEvent != GameEvent.GameEventState.GameFin) { return; }


        // 描画処理
        float ratio = 0.0f;
        if (m_state == ScoreState.ClearTime)
        {
            ratio = m_deltaTime / m_nextStateTime;
            m_clearTime.localScale = new Vector3(ratio, ratio, ratio);
        }
        else if (m_state == ScoreState.DeathCnt)
        {
            m_clearTime.localScale = Vector3.one;

            ratio = m_deltaTime / m_nextStateTime;
            m_deathCnt.localScale = new Vector3(ratio, ratio, ratio);
        }
        else if (m_state == ScoreState.PlayerHp)
        {
            m_deathCnt.localScale = Vector3.one;

            ratio = m_deltaTime / m_nextStateTime;
            m_playerHp.localScale = new Vector3(ratio, ratio, ratio);
        }
        else if (m_state == ScoreState.TotalScore)
        {
            m_playerHp.localScale = Vector3.one;

            ratio = m_deltaTime / m_scoreTime;
            m_totalScore.localScale = new Vector3(ratio, ratio, ratio);
        }
        else if (m_state == ScoreState.Rank)
        {
            m_totalScore.localScale = Vector3.one;

            ratio = m_deltaTime / m_rankTime;
            m_rank.localScale = new Vector3(ratio, ratio, ratio);
        }
        else if (m_state == ScoreState.Fin)
        {
            m_rank.localScale = Vector3.one;
            m_rank.Rotate(0.0f, m_rankRotSpeed, 0.0f);

            Color color = m_returnTitle.color;
            ratio = m_deltaTime / m_returnTime;
            if (ratio < 1.0f)
            {
                color.a = ratio;
            }
            else
            {
                color.a = Mathf.Sin(m_deltaTime + 1.5707963265352f) * 0.4f + 0.6f;
            }

            m_returnTitle.color = color;
        }


        // 入力による状態の切り替え
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            NextState();
        }

        // 時間経過
        m_deltaTime += Time.deltaTime;

        // 時間経過による状態の切り替え
        if (m_state == ScoreState.None)
        {
            if (m_deltaTime > m_fadeInTime) { NextState(); }
        }
        else if (m_state == ScoreState.ClearTime || m_state == ScoreState.DeathCnt ||
            m_state == ScoreState.PlayerHp)
        {
            if (m_deltaTime > m_nextStateTime) { NextState(); }
        }
        else if (m_state == ScoreState.TotalScore)
        {
            if (m_deltaTime > m_scoreTime) { NextState(); }
        }
        else if (m_state == ScoreState.Rank)
        {
            if (m_deltaTime > m_rankTime) { NextState(); }
        }
    }


    private void ClearStart()
    {
        m_audio = GetComponent<AudioSource>();
        m_audio.volume = GameManager.Instance.soundVolume;

        m_state = ScoreState.None;

        TextMeshProUGUI text;

        float clearTime = GameEvent.Instance.m_clearTime;
        m_clearTime.localScale = Vector3.zero;
        text =  m_clearTime.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = clearTime.ToString("#.## s");

        int deathCnt = GameEvent.Instance.m_deathCount;
        m_deathCnt.localScale = Vector3.zero;
        text = m_deathCnt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = deathCnt.ToString();

        int life = m_player.GetLife();
        m_playerHp.localScale = Vector3.zero;
        text = m_playerHp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = life.ToString();

        // スコアの計算（最大 18000 ポイント）
        // クリア時間（最大 5000 ポイント）
        if (clearTime <= 60.0f) { clearTime = 5000.0f; }
        else { clearTime = 5000.0f - (clearTime * 10.0f >= 5000.0f ? 5000.0f : clearTime * 10.0f); }

        // デス数（最大 10000 ポイント）
        deathCnt = 10000 - (deathCnt * 2000);
        if (deathCnt < 0) { deathCnt = 0; }

        // 残りHP（最大 3000 ポイント）
        life *= 30;

        float score = clearTime + (float)deathCnt + (float)life;
        string scoreRank = "F";
        if (score >= 16500.0f) { scoreRank = "SS"; }
        else if (score >= 15000.0f) { scoreRank = "S"; }
        else if (score >= 12000.0f) { scoreRank = "A"; }
        else if (score >= 9000.0f) { scoreRank = "B"; }
        else if (score >= 6000.0f) { scoreRank = "C"; }
        else if (score >= 3000.0f) { scoreRank = "D"; }

        score = 9999.0f * (score / 18000.0f);
        Debug.Log(score);
        m_totalScore.localScale = Vector3.zero;
        text = m_totalScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = ((int)score).ToString();


        m_rank.localScale = Vector3.zero;
        text = m_rank.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = scoreRank;

        Color color = m_returnTitle.color;
        color.a = 0.0f;
        m_returnTitle.color = color;

        m_deltaTime = 0.0f;
    }

    
    private void NextState()
    {
        m_state++;
        if (m_state > ScoreState.Fin)
        {
            GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.GameFin);
        }
        m_deltaTime = 0.0f;

        if (m_state == ScoreState.ClearTime || m_state == ScoreState.DeathCnt ||
            m_state == ScoreState.PlayerHp || m_state == ScoreState.TotalScore)
        {
            Invoke(nameof(SoundScore), 0.25f);
        }
        else if (m_state == ScoreState.Rank)
        {
            Invoke(nameof(SoundTotal), 0.55f);
        }
    }

    private void SoundScore()
    {
        m_audio.PlayOneShot(m_scoreClip);
    }

    private void SoundTotal()
    {
        m_audio.PlayOneShot(m_totalClip);
        Invoke(nameof(SoundScore), 0.15f);
    }
}

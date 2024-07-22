using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float MaxSoundVolume = 0.5f;
    [SerializeField] private float MaxBGMVolume = 0.3f;

    [SerializeField] Texture2D cursorTexture; // カーソル画像
    public Vector2 hotSpot = Vector2.zero; // カーソルのホットスポット

    public float soundVolume;
    public float BGMVolume;

    public bool clearFlg = false;
    public bool noticeFlg = false;

    public enum Difficulty // 難しさ
    {
        Easy = 0,
        Hard = 1,
        SuperHard = 2
    }
    public Difficulty m_difficulty = Difficulty.Easy;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Clear"))
        {
            clearFlg = true;
            noticeFlg = true;
        }
        else
        {
            clearFlg = false;
            noticeFlg = false;
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "TitleScene_3D")
        {
            Cursor.visible = true;
        }
        else if (SceneManager.GetActiveScene().name == "Training Scene")
        {
            Cursor.visible = false;
        }
        else if (SceneManager.GetActiveScene().name == "GameScene")
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SetCustomCursor();
        soundVolume = MaxSoundVolume / 3.0f;
        BGMVolume = MaxBGMVolume / 3.0f;
    }

    public float GetMaxSoundVol() { return MaxSoundVolume; }
    public float GetMaxBGMVol() { return MaxBGMVolume; }
    public void SetDifficulty(Difficulty _difficulty) {  m_difficulty = _difficulty; }

    void SetCustomCursor()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    public void SetclearFlg(bool _flg)
    {
        if (_flg)
        {
            if (!PlayerPrefs.HasKey("Clear"))
            {
                Debug.Log("クリアデータを保存しました");
                clearFlg = true;
                int clearInt = 1;
                PlayerPrefs.SetInt("Clear", clearInt);
                PlayerPrefs.Save();
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("Clear"))
            {
                Debug.Log("クリアデータを削除しました");
                clearFlg = false;
                noticeFlg = false;
                PlayerPrefs.DeleteKey("Clear");
                PlayerPrefs.Save();
            }
        }
    }

    public void SetnoticeFlg()
    {
        noticeFlg = true;
    }
}
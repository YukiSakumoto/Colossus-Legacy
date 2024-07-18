using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScene : MonoBehaviour
{

    public bool IsLoad = false;
    // ロードするシーンの名前
    public string sceneName;

    // ロードの進捗状況を表示するUIなど
    public GameObject loadingUI;

    // ロードの進捗状況を管理するための変数
    private AsyncOperation async;

    // ロードを開始するメソッド
    public void StartLoad()
    {
        StartCoroutine(Load());
    }

    // コルーチンを使用してロードを実行するメソッド
    private IEnumerator Load()
    {
        // ロード画面を表示する
        loadingUI.SetActive(true);

        // シーンを非同期でロードする
        async = SceneManager.LoadSceneAsync(sceneName);
        if (sceneName == "GameScene")
        {
            IsLoad = true;
            GameManagerGameScene.Instance.GameSceneStart();
        }

        // ロードが完了するまで待機する
        while (!async.isDone)
        {
            yield return null;
        }

        // ロード画面を非表示にする
        loadingUI.SetActive(false);
    }
    public bool IsSceneLoaded()
    {
        return IsLoad;
    }
}
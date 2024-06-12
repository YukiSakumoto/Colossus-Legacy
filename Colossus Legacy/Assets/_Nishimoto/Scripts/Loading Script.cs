using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    // ���[�h����V�[���̖��O
    public string sceneName;

    // ���[�h�̐i���󋵂�\������UI�Ȃ�
    public GameObject loadingUI;

    // ���[�h�̐i���󋵂��Ǘ����邽�߂̕ϐ�
    private AsyncOperation async;

    // ���[�h���J�n���郁�\�b�h
    public void StartLoad()
    {
        StartCoroutine(Load());
    }

    // �R���[�`�����g�p���ă��[�h�����s���郁�\�b�h
    private IEnumerator Load()
    {
        // ���[�h��ʂ�\������
        loadingUI.SetActive(true);

        // �V�[����񓯊��Ń��[�h����
        async = SceneManager.LoadSceneAsync(sceneName);

        // ���[�h����������܂őҋ@����
        while (!async.isDone)
        {
            yield return null;
        }

        // ���[�h��ʂ��\���ɂ���
        loadingUI.SetActive(false);
    }
}
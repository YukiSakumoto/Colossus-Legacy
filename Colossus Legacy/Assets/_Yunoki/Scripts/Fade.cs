using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeTime = 2.0f;

    private bool m_fadeEnabled = true;
    public IEnumerator FadeIn()
    {
        Debug.Log("FadeIn");
        fadePanel.enabled = true;
        m_fadeEnabled = true;

        yield return new WaitForSeconds(1.0f);

        float fadeInTime = 0;
        Color startColor = fadePanel.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        while (fadeInTime < fadeTime)
        {
            fadeInTime += Time.deltaTime;
            float t = Mathf.Clamp01(fadeInTime / fadeTime);
            fadePanel.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        fadePanel.color = endColor;
        fadePanel.enabled = false;
        m_fadeEnabled = false;
    }
    public IEnumerator FadeOut()
    {
        Debug.Log("FadeOut");

        yield return new WaitForSeconds(1.0f);

        fadePanel.enabled = true;
        m_fadeEnabled = true;
        float fadeOutTime = 0;
        Color startColor = fadePanel.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        while (fadeOutTime < fadeTime)
        {
            fadeOutTime += Time.deltaTime;
            float t = Mathf.Clamp01(fadeOutTime / fadeTime);
            fadePanel.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        fadePanel.color = endColor;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

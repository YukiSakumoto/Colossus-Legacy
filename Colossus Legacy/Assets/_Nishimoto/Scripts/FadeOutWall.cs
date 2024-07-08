using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutWall : MonoBehaviour
{
    // フェードアウトするまでの時間(0.5sec)
    public float fadeTime = 0.8f;
    private float time;
    private SpriteRenderer render;
    public Material gameObjectMaterial;
    public GameObject gameobject;
    private Color col;
    public bool boolean;

    // Start is called before the first frame update
    void Start()
    {
        col.a = 1.0f;
        gameObjectMaterial.color = col;
        render = GetComponent<SpriteRenderer>();
        boolean = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlayFadeOut()
    {
        time += Time.deltaTime;
        if (time < fadeTime)
        {
            float alpha = 1.0f - time / fadeTime;
            Color color = gameObjectMaterial.color;
            color.a = alpha;
            gameObjectMaterial.color = color;
        }
        else
        {
            Destroy(gameobject);
            boolean = false;
        }
    }

    public bool GetBool()
    {
        return boolean;
    }
}

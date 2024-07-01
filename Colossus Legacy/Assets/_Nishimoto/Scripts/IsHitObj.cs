using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHitObj : MonoBehaviour
{
    private Color color = Color.white;      //color��white���x�[�X�Ɏg�p���܂��B

    //�e �q�I�u�W�F�N�g���i�[�B
    private MeshRenderer[] meshRenderers;
    private MaterialPropertyBlock m_mpb;

    public MaterialPropertyBlock mpb
    {
        get { return m_mpb ?? (m_mpb = new MaterialPropertyBlock()); }
    }

    void Awake()
    {
        //�q�I�u�W�F�N�g�Ɛe�I�u�W�F�N�g��meshrenderer���擾
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
    }
    public void ClearMaterialInvoke()
    {
        color.a = 0.25f;

        mpb.SetColor(Shader.PropertyToID("_Color"), color);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].GetComponent<Renderer>().material.shader = Shader.Find("Unlit/SkeltonShader");
            meshRenderers[i].SetPropertyBlock(mpb);
        }
    }
    public void NotClearMaterialInvoke()
    {
        color.a = 1f;
        mpb.SetColor(Shader.PropertyToID("_Color"), color);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            meshRenderers[i].SetPropertyBlock(mpb);
        }
    }
}

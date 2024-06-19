using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinMesh : MonoBehaviour
{
    SkinnedMeshRenderer[] m_renderers;
    void Start()
    {
        m_renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }


    public void SetSkinMeshShadow(bool _flg)
    {
        foreach (SkinnedMeshRenderer renderer in m_renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = _flg;
        }
    }
}

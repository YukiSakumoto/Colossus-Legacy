using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinMesh : MonoBehaviour
{
    List<Renderer> m_renderers = new List<Renderer>();
    void Start()
    {
        m_renderers.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
        m_renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
    }


    public void SetSkinMeshShadow(bool _flg)
    {
        foreach (Renderer renderer in m_renderers)
        {
            if (renderer)
            {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = _flg;
            }
        }
    }
}

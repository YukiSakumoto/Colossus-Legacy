using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Dissolve : MonoBehaviour
{
    List<Material> materials = new List<Material>();

    void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }
    }


    public void SetDissolveAmount(float _ratio)
    {
        foreach (Material mat in materials)
        {
            mat.SetFloat("_DissolveAmount", _ratio);
        }
    }
}

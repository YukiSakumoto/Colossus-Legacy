using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Dissolve : MonoBehaviour
{
    Material[] mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().materials;
    }


    public void SetDissolveAmount(float _ratio)
    {
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].SetFloat("_DissolveAmount", _ratio);
        }
    }
}

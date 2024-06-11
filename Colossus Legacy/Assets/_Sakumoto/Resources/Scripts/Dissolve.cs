using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }


    public void SetDissolveAmount(float _ratio)
    {
        mat.SetFloat("_DissolveAmount", _ratio);
    }
}

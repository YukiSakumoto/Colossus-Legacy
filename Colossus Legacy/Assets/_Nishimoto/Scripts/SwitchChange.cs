using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchChange : MonoBehaviour
{
    [SerializeField] Material[] materialArray = new Material[2];
    Material cubeMaterial;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = materialArray[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSwitch()
    {
        GetComponent<MeshRenderer>().material = materialArray[1];
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArrow : MonoBehaviour
{

    public void AllDestroy()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ArrowAttack");
        foreach (GameObject obj in objects) 
        {
            Destroy(obj);
        }
    }
}

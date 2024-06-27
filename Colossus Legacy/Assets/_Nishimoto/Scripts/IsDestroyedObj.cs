using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class IsDestroyedObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    GameObject Obj;
    public void SetObj(GameObject @object)
    {
        Obj = @object;
    }

    public void ObjDestroy()
    {
        if (Obj != null)
        {
            Destroy(Obj);
        }
    }
}

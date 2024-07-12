using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject Player;
    GameObject obj;
     bool IsTeleport = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBool(bool _isteleport)
    {
        IsTeleport = _isteleport;
    }
    public void SetTagCollision(GameObject _obj)
    {
        obj = _obj;
    }
    public void TeleportNext()
    {
        if(IsTeleport) { return; }
        Player.transform.position = obj.transform.position;
        IsTeleport = true;
    }
}

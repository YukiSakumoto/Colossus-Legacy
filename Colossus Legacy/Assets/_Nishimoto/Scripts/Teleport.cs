using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    GameObject obj;
    [SerializeField] GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTagCollision(GameObject _obj)
    {
        obj = _obj;
    }
    public void TeleportNext()
    {
        Player.transform.position = obj.transform.position;
    }
}

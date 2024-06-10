using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject expParticle;

    [SerializeField] float GroundPos = 0.2f;

    private Vector3 pos;

    private void Update()
    {
        pos = transform.position;
        if (pos.y <= GroundPos) { transform.position = new Vector3(pos.x, GroundPos, pos.z); }
    }
    
    private void OnDestroy()
    {
        Instantiate(expParticle,transform.position,Quaternion.identity);
    }
}

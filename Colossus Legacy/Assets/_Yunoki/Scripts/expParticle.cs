using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expParticle : MonoBehaviour
{
    private bool hit = false;   

    void Update()
    {
        Destroy(gameObject, 1.2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hit) { return; }
        if (other.gameObject.tag == ("Enemy"))
        {
            Debug.Log("Hit Bomb");
            hit = true;
        }
    }
}

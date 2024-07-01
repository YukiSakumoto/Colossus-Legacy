using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCollider : MonoBehaviour
{
    [SerializeField] ChestOpen chest;
    private void OnTriggerEnter(Collider other)
    {
        chest.Open();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator= GetComponent<Animator>();
    }
    public void Open()
    {
        animator.SetBool("Open", true);
    }
}

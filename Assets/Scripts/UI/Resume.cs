using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public GameObject pauzePanel;
    private Animator animator;

    void Start()
    {
        animator = pauzePanel.GetComponent<Animator>();
    }

    public void Doorgaan()
    {
        animator.SetBool("IsOpen",  false);
    }
}

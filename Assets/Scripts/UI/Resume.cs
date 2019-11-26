using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resume : MonoBehaviour
{
    public GameObject canvas;
    public GameObject pauzePanel;
    private Animator animator;

    void Start()
    {
        animator = pauzePanel.GetComponent<Animator>();
    }

    public void Doorgaan()
    {
        animator.SetBool("IsOpen",  false);
        canvas.SetActive(false);
    }

}

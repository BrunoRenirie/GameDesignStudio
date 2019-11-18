using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MenuSwitcher : MonoBehaviour
{
   // public int sceneIndex;
    public bool isUsed;
    public bool switcher;
    public GameObject pauseMenuUi;
    bool isOpen;
    private AudioSource buttonSource;
    private Animator animator;


    void Start()
    {
        buttonSource =  gameObject.AddComponent<AudioSource>();

        animator = pauseMenuUi.GetComponent<Animator>();
        isOpen = animator.GetBool("CreditsStart");
    }
    public void Switch(int sceneIndex)
    {
        if (sceneIndex == -1)
        {
            Application.Quit();
        }
        SceneManager.LoadScene(sceneBuildIndex: sceneIndex);
     
    }
    public void StartCredits()
    {
        if (!isUsed)
        {
            Credits();
        }
        else
        {
            Resume();

        }
    }
    public void Credits()
    {     
        animator.SetBool("CreditsStart", !isOpen);
        isUsed = true;
    }
    public void Resume()
    {
        animator.SetBool("CreditsStart", isOpen);
        isUsed = false;
    }
}

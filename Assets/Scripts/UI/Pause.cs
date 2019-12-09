using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private Animator animator;
    public bool spawnPanel = false;

    public void Paus()
    {
        if (spawnPanel == false)
        {
            SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
            spawnPanel = true;
        }else
        {
            animator =  GameObject.Find("PauzePanel2").GetComponent<Animator>();
            animator.SetBool("IsOpen", true);
        }
    }
}

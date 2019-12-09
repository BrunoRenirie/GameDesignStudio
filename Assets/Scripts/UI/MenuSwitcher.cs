using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MenuSwitcher : MonoBehaviour
{
    public bool  _isUsed;
    public bool switcher;
    private AudioSource buttonSource;

    void Start()
    {
        buttonSource =  gameObject.AddComponent<AudioSource>();





    }
    public void Switch(int sceneIndex)
    {
        if (sceneIndex == -1)
        {
            Application.Quit();
        }
        SceneSwitcher._Instance.StartCoroutine(SceneSwitcher._Instance.LoadNextScene(sceneIndex));
     

    }
}

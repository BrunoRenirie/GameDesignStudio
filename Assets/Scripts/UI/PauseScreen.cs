using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject _OptionsScreen;
    [SerializeField] private GameObject _CreditsScreen;

    public void TogglePauseScreen()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OptionsScreen()
    {
        _OptionsScreen.SetActive(!_OptionsScreen.activeSelf);
    }

    public void CreditsScreen()
    {
        _CreditsScreen.SetActive(!_CreditsScreen.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject _MainMenuScreen;

    public void CloseMainMenu()
    {
        _MainMenuScreen.SetActive(false);
    }

}

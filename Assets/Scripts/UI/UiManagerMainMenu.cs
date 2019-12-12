using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UiManagerMainMenu : MonoBehaviour
{
    public static UiManagerMainMenu _Instance;
    private TileManager _TileManager;

    [SerializeField] private GameObject _ResetScreen, _MainMenuScreen, _ReturnButton;

    [SerializeField] private bool _Closed;
    private void Start()
    {
        _TileManager = TileManager._Instance;
    }

    private void Update()
    {
        if(_MainMenuScreen == null && _Closed)
        {
            CloseMainMenu();
        }
    }

    public void CloseMainMenu()
    {
        _MainMenuScreen = GameObject.FindGameObjectWithTag("MainMenuScreen");

        if(_MainMenuScreen != null)
        {
            _MainMenuScreen.SetActive(false);

        }
        _Closed = true;
    }

    public void EnableResetScreen()
    {
        _ResetScreen.SetActive(!_ResetScreen.activeSelf);
    }

    public void ClearPersistentDatapath()
    {
        foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
        {
            DirectoryInfo data_dir = new DirectoryInfo(directory);
            data_dir.Delete(true);
        }

        foreach (var file in Directory.GetFiles(Application.persistentDataPath))
        {
            FileInfo file_info = new FileInfo(file);
            file_info.Delete();
        }

        _TileManager.ResetTiles(); 
    }

    public void ToggleReturnButton()
    {
        _ReturnButton.SetActive(_ReturnButton.activeSelf);
    }
}

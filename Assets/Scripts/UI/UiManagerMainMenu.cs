using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UiManagerMainMenu : MonoBehaviour
{
    private TileManager _TileManager;

    [SerializeField] private GameObject _ResetScreen;

    private void Start()
    {
        _TileManager = TileManager._Instance;
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
}

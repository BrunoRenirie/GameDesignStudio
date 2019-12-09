using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager _Instance;

    public LevelData _LevelData;
    private SceneSwitcher _SceneSwitcher;
    private TileManager _TileManager;
    private PlayModeSwitcher _PlayModeSwitcher;
    private LevelEditorManager _LevelEditorManager;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        GetReferences();

        StartCoroutine(DelayLoad());

        _SceneSwitcher._SceneSaveEvent.AddListener(Save);
        _SceneSwitcher._SceneLoadEvent.AddListener(LoadDelay);
    }

    private void Save()
    {
        GetReferences();

        LevelData newData = new LevelData();

        if (_PlayModeSwitcher != null)
            newData._EntityList = _PlayModeSwitcher._EntityPosList;
        else if (_LevelData != null)
            newData._EntityList = _LevelData._EntityList;

        if(_TileManager != null)
            newData._TileList = _TileManager._Tiles;
        else if (_LevelData != null)
            newData._TileList = _LevelData._TileList;

        if (_LevelEditorManager != null)
            newData._TileDict = _LevelEditorManager.GetWorldTiles();
        else if (_LevelData != null)
            newData._TileDict = _LevelData._TileDict;

        _LevelData = newData;

        _TileManager.SaveTiles();

        _LevelData.Save();

        ES3.Save<LevelData>(_TileManager._LevelName, _LevelData);
    }

    public void LoadLevel()
    {
        GetReferences();
        if (ES3.KeyExists(_TileManager._LevelName))
        {
            LevelData Data = ES3.Load<LevelData>(_TileManager._LevelName);

            Data.Load();
            _LevelData = new LevelData
            {
                _EntityList = Data._EntityList,
                _TileList = Data._TileList,
                _TileDict = Data._TileDict
            };
        }
        else
        {
            SaveDelay();
            return;
        }

        if (_LevelData == null)
        {
            _LevelData = new LevelData();
            return;
        }

        //TileManager Data
        if (_LevelData._TileList.Count > 0)
        {
            List<ScriptableTile> newTiles = new List<ScriptableTile>();

            for (int i = 0; i < _TileManager._Tiles.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < _LevelData._TileList.Count; j++)
                {
                    if (_TileManager._Tiles[i]._Name == _LevelData._TileList[j]._Name)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    newTiles.Add(_TileManager._Tiles[i]);
            }
            _TileManager._Tiles = _LevelData._TileList;
            _TileManager._Tiles.AddRange(newTiles);
        }

        _TileManager.LoadTiles();

        //Tiles Data
        if (_LevelData._TileDict.Count > 0 && _LevelEditorManager != null)
            _LevelEditorManager.LoadLevel(_LevelData._TileDict);

        //Playmode Data
        if (_LevelData._EntityList.Count > 0 && _PlayModeSwitcher != null)
        {
            _PlayModeSwitcher._EntityPosList = _LevelData._EntityList;
            _PlayModeSwitcher.LoadEntityPos();
        }
    }

    public void LoadDelay()
    {
        StartCoroutine(DelayLoad());
    }

    public void SaveDelay()
    {
        StartCoroutine(DelaySave());
    }

    private IEnumerator DelayLoad()
    {
        yield return new WaitForEndOfFrame();

        LoadLevel();
    }

    private IEnumerator DelaySave()
    {
        yield return new WaitForEndOfFrame();

        Save();
    }

    public void GetReferences()
    {
        _TileManager = TileManager._Instance;
        _PlayModeSwitcher = PlayModeSwitcher._Instance;
        _LevelEditorManager = LevelEditorManager._Instance;
        _SceneSwitcher = SceneSwitcher._Instance;
    }
}

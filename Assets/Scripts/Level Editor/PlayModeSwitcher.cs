﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayModeSwitcher : MonoBehaviour
{
    public static PlayModeSwitcher _Instance;

    private TileManager _TileManager;
    private LevelEditorManager _LevelEditorManager;

    [SerializeField] private Camera _EditorCam, _PlayCam;
    [SerializeField] private Canvas _BackgroundCanvas;

    public List<ObjectTileData> _EntityList;
    public Dictionary<GameObject, Vector3> _EntityPosList;

    public UnityEvent _SwitchPlaymode, _SwitchEditMode;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        _TileManager = TileManager._Instance;
        _LevelEditorManager = LevelEditorManager._Instance;

        _EntityPosList = new Dictionary<GameObject, Vector3>();

        _SwitchPlaymode.AddListener(SwitchCamera);
        _SwitchEditMode.AddListener(SwitchCamera);

        _SwitchPlaymode.AddListener(SaveEntityPos);
        _SwitchEditMode.AddListener(LoadEntityPos);

        _SwitchPlaymode.AddListener(BackgroundToPlay);
        _SwitchEditMode.AddListener(BackgroundToEdit);
    }

    public void EnterPlayMode()
    {
        _SwitchPlaymode.Invoke();
    }
    public void EnterEditMode()
    {
        _SwitchEditMode.Invoke();
    }

    void SwitchCamera()
    {
        _EditorCam.gameObject.SetActive(!_EditorCam.gameObject.activeSelf);
        _PlayCam.gameObject.SetActive(!_PlayCam.gameObject.activeSelf);
    }

    void SaveEntityPos()
    {
        if (_EntityList.Count == 0)
            return;

        for (int i = 0; i < _EntityList.Count; i++)
        {
            if (!_EntityPosList.ContainsKey(_EntityList[i].gameObject))
                _EntityPosList.Add(_EntityList[i].gameObject, _EntityList[i].transform.position);
            else
                _EntityPosList[_EntityList[i].gameObject] = _EntityList[i].transform.position;
        }

        /*
        if (_EntityList.Count == 0)
            return;

        for (int i = 0; i < _Entities.Count; i++)
        {
            if (!_EntityList.ContainsKey(_Entities[i]))
                _EntityList.Add(_Entities[i], _Entities[i].transform.position);
            else
                _EntityList[_Entities[i]] = _Entities[i].transform.position;
        }
        */
    }

    public void LoadEntityPos()
    {
        List<GameObject> newEntities = new List<GameObject>();
        List<GameObject> oldEntities = new List<GameObject>();
        _EntityList = new List<ObjectTileData>();

        Debug.Log("starting load");
        foreach (KeyValuePair<GameObject, Vector3> Entity in _EntityPosList)
        {
            Entity.Key.transform.position = Entity.Value;

            switch (Entity.Key.tag)
            {
                case "Player":

                    _LevelEditorManager._Player = Instantiate(_TileManager.PlayerPrefab, Entity.Value, Quaternion.identity);

                    ObjectTileData playerData = _LevelEditorManager._Player.GetComponent<ObjectTileData>();
                    if (playerData == null)
                    {
                        playerData = _LevelEditorManager._Player.AddComponent<ObjectTileData>();

                        for (int i = 0; i < _TileManager._Tiles.Count; i++)
                        {
                            if (_TileManager._Tiles[i]._TileEnum == TilesEnum.Player)
                                playerData._Tile = _TileManager._Tiles[i];
                        }
                    }

                    if (!_EntityList.Contains(playerData))
                    {
                        _EntityList.Add(playerData);
                    }

                    newEntities.Add(_LevelEditorManager._Player);
                    oldEntities.Add(Entity.Key);

                    break;
                case "Enemy":

                    GameObject enemy = Instantiate(_TileManager.EnemyPrefab, Entity.Value, Quaternion.identity);

                    ObjectTileData enemyData = enemy.GetComponent<ObjectTileData>();
                    if (enemyData == null)
                    {
                        enemyData = enemy.AddComponent<ObjectTileData>();

                        for (int i = 0; i < _TileManager._Tiles.Count; i++)
                        {
                            if (_TileManager._Tiles[i]._TileEnum == TilesEnum.Enemy)
                                enemyData._Tile = _TileManager._Tiles[i];
                        }
                    }

                    if (!_EntityList.Contains(enemyData))
                    {
                        _EntityList.Add(enemyData);
                    }

                    newEntities.Add(enemy);
                    oldEntities.Add(Entity.Key);

                    break;
            }

        }


        for (int i = 0; i < oldEntities.Count; i++)
        {
            _EntityPosList.Remove(oldEntities[i]);
            
            Destroy(oldEntities[i]);
        }

        for (int i = 0; i < newEntities.Count; i++)
        {
            _EntityPosList.Add(newEntities[i], newEntities[i].transform.position);
        }

        SaveEntityPos();
    }

    void BackgroundToEdit()
    {
        _BackgroundCanvas.worldCamera = _EditorCam;
    }
    void BackgroundToPlay()
    {
        _BackgroundCanvas.worldCamera = _PlayCam;
    }
}

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

    public Dictionary<ScriptableTile, Vector3> _EntityList;

    public UnityEvent _SwitchPlaymode, _SwitchEditMode;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        _TileManager = TileManager._Instance;
        _LevelEditorManager = LevelEditorManager._Instance;

        _EntityList = new Dictionary<ScriptableTile, Vector3>();

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



    }

    public void LoadEntityPos()
    {
        foreach (KeyValuePair<ScriptableTile, Vector3> Entity in _EntityList)
        {
            switch (Entity.Key._TileEnum)
            {
                case TilesEnum.Player:

                    if (_LevelEditorManager._Player == null)
                    {
                        _LevelEditorManager._Player = Instantiate(_TileManager.PlayerPrefab, Entity.Value, Quaternion.identity);
                    }
                    else
                    {
                        _LevelEditorManager._Player.transform.position = Entity.Value;
                    }

                    break;
                case TilesEnum.Enemy:



                    break;
                case TilesEnum.Checkpoint:

                    break;
            }
        }
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

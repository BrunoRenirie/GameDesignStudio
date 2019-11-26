using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public enum EditorTools
{
    Move,
    Place,
    Remove
}

public class LevelEditorUi : MonoBehaviour
{
    public static LevelEditorUi _Instance;

    private TileManager _TileManager;

    [SerializeField] private List<GameObject> _PlayModeUi, _EditModeUi;
    [SerializeField] private Image _Background;

    public Tile _TileObj;
    public EditorTools _CurrentTool;
    [SerializeField] private GameObject _SelectionBackground, _MoveTool, _EditTool, _RemoveTool, _BlockScreen, _ResetScreen;

    public int _CurrentTile;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        _TileManager = TileManager._Instance;
        if (_TileManager._Tiles.Count > 0)
            SetTilemapTiles();
    }

    private void Update()
    {
        if (_TileManager._TileMapTiles.Count > 0)
            _TileObj = _TileManager._TileMapTiles[_CurrentTile];

        if (_TileManager._Tiles.Count > 0)
            SetTilemapTiles();
    }

    public void ToPlayMode()
    {
        for (int i = 0; i < _PlayModeUi.Count; i++)
        {
            _PlayModeUi[i].SetActive(true);
        }
        for (int i = 0; i < _EditModeUi.Count; i++)
        {
            _EditModeUi[i].SetActive(false);
        }
    }
    public void ToEditMode()
    {
        for (int i = 0; i < _PlayModeUi.Count; i++)
        {
            _PlayModeUi[i].SetActive(false);
        }
        for (int i = 0; i < _EditModeUi.Count; i++)
        {
            _EditModeUi[i].SetActive(true);
        }
    }

    public void SetTilemapTiles()
    {
        _TileManager._TileMapTiles.Clear();

        for (int i = 0; i < _TileManager._Tiles.Count; i++)
        {
            Tile TileObj = new Tile();
            TileObj.sprite = _TileManager._Tiles[i]._Image;
            TileObj.name = _TileManager._Tiles[i]._Name;
            TileObj.colliderType = Tile.ColliderType.Grid;


            _TileManager._TileMapTiles.Add(TileObj);
        }
    }

    public void SetBackground(Sprite image)
    {
        _Background.sprite = image;
    }

    public void OpenGallery()
    {
        _BlockScreen.SetActive(true);
    }
    public void EnableResetScreen()
    {
        _ResetScreen.SetActive(!_ResetScreen.activeSelf);
    }
    public void MoveTool()
    {
        _SelectionBackground.transform.SetParent(_MoveTool.transform);
        _SelectionBackground.transform.localPosition = new Vector3(0, 2, 0);

        _CurrentTool = EditorTools.Move;
    }
    public void PlaceTool()
    {
        _SelectionBackground.transform.SetParent(_EditTool.transform);
        _SelectionBackground.transform.localPosition = new Vector3(0, 2, 0);

        _CurrentTool = EditorTools.Place;
    }
    public void RemoveTool()
    {
        _SelectionBackground.transform.SetParent(_RemoveTool.transform);
        _SelectionBackground.transform.localPosition = new Vector3(0, 2, 0);

        _CurrentTool = EditorTools.Remove;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelEditorUi : MonoBehaviour
{
    public static LevelEditorUi _Instance;

    private TileManager _TileManager;

    [SerializeField] private List<GameObject> _PlayModeUi, _EditModeUi;
    [SerializeField] private Image _Background;

    [HideInInspector] public TMP_Dropdown _Dropdown;
    public Tile _TileObj;

    private int _DropdownSize = -1;

    public int _CurrentTile;
    [SerializeField] private GameObject _BlockScreen;

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
}

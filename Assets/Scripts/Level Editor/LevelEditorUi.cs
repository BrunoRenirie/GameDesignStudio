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
    [HideInInspector] public Tile _TileObj;

    private int _DropdownSize = -1;

    [HideInInspector] public int _CurrentTile;
    [SerializeField] private GameObject _BlockScreen;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        _TileManager = TileManager._Instance;
        if (_TileManager._Tiles.Count > 0)
            UpdateDropdown();
    }

    private void Update()
    {
        if (_TileManager._TileMapTiles.Count > 0)
            _TileObj = _TileManager._TileMapTiles[_CurrentTile];

        if (_TileManager._Tiles.Count > 0)
            UpdateDropdown();
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

    public void UpdateDropdown()
    {
        //_Dropdown.ClearOptions();
        //List<TMP_Dropdown.OptionData> dropDownOptions = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < _TileManager._Tiles.Count; i++)
        {
            //TMP_Dropdown.OptionData tileDropdown = new TMP_Dropdown.OptionData();
            //tileDropdown.image = _TileManager._Tiles[i]._Image;
            //tileDropdown.text = _TileManager._Tiles[i]._Name;

            Tile TileObj = new Tile();
            TileObj.sprite = _TileManager._Tiles[i]._Image;
            TileObj.name = _TileManager._Tiles[i]._Name;
            TileObj.colliderType = Tile.ColliderType.Grid;

            int foundTile = -1;
            for (int j = 0; j < _TileManager._TileMapTiles.Count; j++)
            {
                if (TileObj.name == _TileManager._TileMapTiles[j].name)
                {
                    foundTile = j;
                    break;
                }
            }

            if (foundTile == -1)
                _TileManager._TileMapTiles.Add(TileObj);

            //dropDownOptions.Add(tileDropdown);
        }

        //_Dropdown.AddOptions(dropDownOptions);
        //_Dropdown.RefreshShownValue();

        //_DropdownSize = _TileManager._Tiles.Count;
    }

    public void SetBackground(Sprite image)
    {
        _Background.sprite = image;
    }
<<<<<<< HEAD
}
=======

    public void OpenGallery()
    {
        _BlockScreen.SetActive(true);
    }
}
>>>>>>> ebe537e260e46a599a3ab6b0e51c4196900cc957

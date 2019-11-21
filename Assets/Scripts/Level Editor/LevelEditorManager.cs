using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


public class LevelEditorManager : MonoBehaviour
{
    public static LevelEditorManager _Instance;
    private LevelEditorUi _EditorUi;
    private TileManager _TileManager;

    private Tilemap _TileMap;
    [HideInInspector] public GameObject _Player;

    private bool _Editing = true;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        _TileManager = TileManager._Instance;
        _EditorUi = LevelEditorUi._Instance;
        _TileMap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (!_Editing || _TileManager._TileMapTiles.Count == 0)
            return;

        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = 0;
        Vector3Int TilePos = _TileMap.WorldToCell(MousePos);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (_TileManager._Tiles[_EditorUi._CurrentTile]._TileEnum)
            {
                case TilesEnum.Block:

                    _EditorUi._TileObj.colliderType = Tile.ColliderType.Grid;
                    _TileMap.SetTile(TilePos, _EditorUi._TileObj);

                    break;
                case TilesEnum.Player:

                    if (_Player == null)
                    {
                        _Player = Instantiate(_TileManager.PlayerPrefab, MousePos, Quaternion.identity);


                        ObjectTileData playerData = _Player.GetComponent<ObjectTileData>();
                        if (playerData == null)
                        {
                            playerData = _Player.AddComponent<ObjectTileData>();
                            playerData._Tile = _TileManager._Tiles[_EditorUi._CurrentTile];
                        }


                        if (!PlayModeSwitcher._Instance._EntityList.Contains(playerData))
                        {
                            PlayModeSwitcher._Instance._EntityList.Add(playerData);
                        }
                    }
                    else
                    {
                        _Player.transform.position = MousePos;
                    }



                    break;
                case TilesEnum.Enemy:

                    GameObject enemy = Instantiate(_TileManager.EnemyPrefab, MousePos, Quaternion.identity);

                    ObjectTileData enemyData = enemy.GetComponent<ObjectTileData>();
                    if (enemyData == null)
                    {
                        enemyData = enemy.AddComponent<ObjectTileData>();
                        enemyData._Tile = _TileManager._Tiles[_EditorUi._CurrentTile];
                    }

                    if (!PlayModeSwitcher._Instance._EntityList.Contains(enemyData))
                    {
                        PlayModeSwitcher._Instance._EntityList.Add(enemyData);
                    }

                    break;
                case TilesEnum.Checkpoint:

                    break;
                case TilesEnum.Finish:

                    break;
                case TilesEnum.Wallpaper:
                    
                    _EditorUi.SetBackground(_EditorUi._TileObj.sprite);

                    break;
            }
        }
    }

    public void LoadLevel(Dictionary<Vector3, ScriptableTile> LevelToLoad)
    {
        _TileManager._TileMapTiles = new List<Tile>();
        _TileMap.ClearAllTiles();

        _EditorUi.SetTilemapTiles();

        foreach (KeyValuePair<Vector3, ScriptableTile> Data in LevelToLoad)
        {
            Data.Value.LoadImage();

            Tile obj = new Tile();
            obj.name = Data.Value._Name;
            obj.sprite = Data.Value._Image;
            obj.colliderType = Tile.ColliderType.Grid;
            
            bool found = false;
            for (int i = 0; i < _TileManager._Tiles.Count; i++)
            {
                if (Data.Value._Name == _TileManager._Tiles[i]._Name)
                {
                    found = true;
                    break;
                }
            }
            if(!found)
                _TileManager._Tiles.Add(Data.Value);

            int foundTile = -1;
            for (int i = 0; i < _TileManager._TileMapTiles.Count; i++)
            {
                if(obj.name == _TileManager._TileMapTiles[i].name)
                {
                    foundTile = i;
                    break;
                }
            }


            if(foundTile != -1)
                _TileMap.SetTile(new Vector3Int((int)Data.Key.x, (int)Data.Key.y, (int)Data.Key.z), _TileManager._TileMapTiles[foundTile]);
            else
            {
                //_TileManager._TileMapTiles.Add(obj);
                _TileMap.SetTile(new Vector3Int((int)Data.Key.x, (int)Data.Key.y, (int)Data.Key.z), obj);//_TileManager._TileMapTiles[_TileManager._TileMapTiles.Count - 1]);
            }     
        }

        _TileMap.RefreshAllTiles();
        
    }

    public void EnterEditing()
    {
        _Editing = true;
    }
    public void EnterPlay()
    {
        _Editing = false;
    }

    public Dictionary<Vector3, ScriptableTile> GetWorldTiles()
    {
        Dictionary<Vector3, ScriptableTile> tileDict = new Dictionary<Vector3, ScriptableTile>();

        if (_TileMap.GetUsedTilesCount() == 0)
            return new Dictionary<Vector3, ScriptableTile>();

        foreach (Vector3Int pos in _TileMap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, 0);

            if (!_TileMap.HasTile(localPlace)) continue;
            string name = _TileMap.GetTile(localPlace).name;
            ScriptableTile tile = null;
            for (int i = 0; i < _TileManager._Tiles.Count; i++)
            {
                if(name == _TileManager._Tiles[i]._Name)
                {
                    tile = _TileManager._Tiles[i];
                }
            }
            if(tile != null)
                tileDict.Add(localPlace, tile);
        }

        return tileDict;
    }

    public void ResetLevel()
    {
        _TileMap.ClearAllTiles();
    }
}

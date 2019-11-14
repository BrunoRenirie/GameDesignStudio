using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager _Instance;

    public List<ScriptableTile> _Tiles = new List<ScriptableTile>();
    public List<Tile> _TileMapTiles = new List<Tile>();
    public string _LevelName = "Level";

    public GameObject PlayerPrefab, EnemyPrefab;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(gameObject);
            
        _Tiles = new List<ScriptableTile>();
        _TileMapTiles = new List<Tile>();
    }

    public void SaveTiles()
    {
        for (int i = 0; i < _Tiles.Count; i++)
        {
            _Tiles[i].SaveImage();
        }
    }

    public void LoadTiles()
    {
        for (int i = 0; i < _Tiles.Count; i++)
        {
            _Tiles[i].LoadImage();
        }
    }
}

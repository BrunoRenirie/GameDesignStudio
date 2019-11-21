using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public Dictionary<Vector3, ScriptableTile> _TileDict;

    public Dictionary<GameObject, Vector3> _EntityList;

    public List<ScriptableTile> _TileList;
    public List<GameObject> _Entities;

    public void Save()
    {
        ES3.Save<Dictionary<Vector3, ScriptableTile>>("TileDictionary", _TileDict);
        ES3.Save<Dictionary<GameObject, Vector3>>("EntityList", _EntityList);
        ES3.Save<List<ScriptableTile>>("TileList", _TileList);
        //ES3.Save<List<GameObject>>("Entities", _Entities);
    }

    public void Load()
    {
        if(ES3.KeyExists("TileDictionary"))
            _TileDict = ES3.Load<Dictionary<Vector3, ScriptableTile>>("TileDictionary");
        if (ES3.KeyExists("EntityList"))
            _EntityList = ES3.Load<Dictionary<GameObject, Vector3>>("EntityList");
        if (ES3.KeyExists("TileList"))
            _TileList = ES3.Load<List<ScriptableTile>>("TileList");
        /*
        if (ES3.KeyExists("Entities"))
            _Entities = ES3.Load<List<GameObject>>("Entities");
            */
    }
}

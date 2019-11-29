using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TilesEnum
{
    Block,
    Player,
    Enemy,
    Checkpoint,
    Finish,
    Wallpaper,
    Boss
}

[CreateAssetMenu(fileName = "Tile", menuName = "Tiles/Drawable Tile", order = 1)]
public class ScriptableTile : ScriptableObject
{
    public Sprite _Image;
    public string _Name;
    public TilesEnum _TileEnum;
    public Vector3 _Position;
    public List<AnimationSprites> _AnimationList = new List<AnimationSprites>();

    public bool _Custom;

    public void SaveImage()
    {
        if (_Custom)
            return;

        switch (_TileEnum)
        {
            case TilesEnum.Block:

                if (_Image != null)
                    ES3.SaveImage(_Image.texture, _Name + ".png");

                break;
            case TilesEnum.Player:

                _AnimationList.AddRange(TileManager._Instance._PlayerAnimations);

                break;
            case TilesEnum.Enemy:

                _AnimationList.AddRange(TileManager._Instance._EnemyAnimations);

                break;
            case TilesEnum.Checkpoint:
                break;
            case TilesEnum.Finish:
                break;
            case TilesEnum.Wallpaper:
                break;
            case TilesEnum.Boss:
                break;
        }
    }

    public void LoadImage()
    {
        if (_Custom)
            return;

        switch (_TileEnum)
        {
            case TilesEnum.Block:

                _Image = Resources.Load<Sprite>(_Name);

                Texture2D LoadedTexture = null;
                if (!ES3.KeyExists(_Name + ".png"))
                    SaveImage();

                LoadedTexture = ES3.LoadImage(_Name + ".png");

                if (LoadedTexture != null)
                    _Image.texture.LoadImage(LoadedTexture.EncodeToPNG());

                break;
            case TilesEnum.Player:

                break;
            case TilesEnum.Enemy:
                break;
            case TilesEnum.Checkpoint:
                break;
            case TilesEnum.Finish:
                break;
            case TilesEnum.Wallpaper:
                break;
            case TilesEnum.Boss:
                break;
        }

        
    }
}

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

                for (int i = 0; i < _AnimationList.Count; i++)
                {
                    for (int j = 0; j < _AnimationList[i].animationSprites.Count; j++)
                    {
                        //Path = Enum name + Animation name + Animation integer + file extension
                        string path = _TileEnum.ToString() + _AnimationList[i].animation.ToString() + j.ToString() + ".png";

                        if (CompareTexture(_AnimationList[i].animationSprites[j].texture, _AnimationList[i].resetSprite.texture))
                            break;
                        else
                        {
                            ES3.DeleteFile(path);
                            ES3.SaveImage(_AnimationList[i].animationSprites[j].texture, path);
                        }
                    }
                }

                break;
            case TilesEnum.Enemy:

                for (int i = 0; i < _AnimationList.Count; i++)
                {
                    for (int j = 0; j < _AnimationList[i].animationSprites.Count; j++)
                    {
                        //Path = Enum name + Animation name + Animation integer + file extension
                        string path = _TileEnum.ToString() + _AnimationList[i].animation.ToString() + j.ToString() + ".png";

                        if (CompareTexture(_AnimationList[i].animationSprites[j].texture, _AnimationList[i].resetSprite.texture))
                            break;
                        else
                        {
                            ES3.DeleteFile(path);
                            ES3.SaveImage(_AnimationList[i].animationSprites[j].texture, path);
                        }
                    }
                }

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

                _AnimationList = new List<AnimationSprites>();
                _AnimationList.AddRange(TileManager._Instance._PlayerAnimations);

                for (int i = 0; i < _AnimationList.Count; i++)
                {
                    for (int j = 0; j < _AnimationList[i].animationSprites.Count; j++)
                    {
                        //Path = Enum name + Animation name + Animation integer + file extension
                        string path = _TileEnum.ToString() + _AnimationList[i].animation.ToString() + j.ToString() + ".png";
                        if (ES3.FileExists(path)) 
                            _AnimationList[i].animationSprites[j].texture.LoadImage(ES3.LoadImage(path).EncodeToPNG());
                    }
                }

                break;
            case TilesEnum.Enemy:

                _AnimationList = new List<AnimationSprites>();
                _AnimationList.AddRange(TileManager._Instance._EnemyAnimations);

                for (int i = 0; i < _AnimationList.Count; i++)
                {
                    for (int j = 0; j < _AnimationList[i].animationSprites.Count; j++)
                    {
                        //Path = Enum name + Animation name + Animation integer + file extension
                        string path = _TileEnum.ToString() + _AnimationList[i].animation.ToString() + j.ToString();
                        if (ES3.FileExists(path))
                            _AnimationList[i].animationSprites[j].texture.LoadImage(ES3.LoadImage(path + ".png").EncodeToPNG());
                    }
                }

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

    private bool CompareTexture(Texture2D first, Texture2D second)
    {
        Color[] firstPix = first.GetPixels();
        Color[] secondPix = second.GetPixels();
        if (firstPix.Length != secondPix.Length)
        {
            return false;
        }
        for (int i = 0; i < firstPix.Length; i++)
        {
            if (firstPix[i] != secondPix[i])
            {
                return false;
            }
        }

        return true;
    }
}

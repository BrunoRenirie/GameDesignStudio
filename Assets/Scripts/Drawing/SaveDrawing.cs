using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FreeDraw;
using UnityEngine.Events;
using System.IO;

public enum DrawableAnimations
{
    Idle,
    Walk,
    Jumping,
    Shoot,
    Hurt,
    Dead
}

public class SaveDrawing : MonoBehaviour
{

    public static SaveDrawing _Instance;
    private Dictionary<DrawableAnimations, List<Sprite>> _AnimationDictionary;
    private UIManager _UIManager;
    private TileManager _TileManager;
    public SpriteRenderer _SpriteRenderer;
    private Drawable _Drawable;
    private Color[] _Clean_colours_array;
    private Color _ResetColor = new Color(0, 0, 0, 0);

    public int _AnimSpriteCount = 0;
    private int _LastFrameSpriteCount = -1;

    [SerializeField] private PictureGallery PictureGalleryScript;

    private void Awake()
    {
        _Instance = this;   
    }

    private void Start()
    {
        _AnimationDictionary = new Dictionary<DrawableAnimations, List<Sprite>>();
        _TileManager = TileManager._Instance;
        _UIManager = UIManager.Instance;
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _Drawable = GetComponent<Drawable>();
    }

    private void Update()
    {
        if (_TileManager._Tiles.Count <= 0)
            return;

        var list = _TileManager._Tiles[_UIManager._CurrentTile];
        if(list._AnimationList.Count > 1)
        {
            _AnimSpriteCount = Mathf.Clamp(_AnimSpriteCount, 0, list._AnimationList[_UIManager._AnimationDropdown.value].animationSprites.Count - 1);//list._AnimationList[(int)_CurrentAnim].animationSprites.Count - 1);

            if (_AnimSpriteCount != _LastFrameSpriteCount)
                ChangeSprite(_AnimSpriteCount);
            _LastFrameSpriteCount = _AnimSpriteCount;
            
        }
    }

    public void ChangedAnimation()
    {
        ChangeSprite(0);
    }

    public void OrganiseList()
    {
        //Set dropdown menu
        TMP_Dropdown.OptionDataList _OptionList = new TMP_Dropdown.OptionDataList();
        for (int i = 0; i < _TileManager._Tiles[_UIManager._CurrentTile]._AnimationList.Count; i++)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = _TileManager._Tiles[_UIManager._CurrentTile]._AnimationList[i].dropDownName;
            optionData.image = _TileManager._Tiles[_UIManager._CurrentTile]._AnimationList[i].dropDownSprite;
            _OptionList.options.Add(optionData);
        }
        _UIManager.SetAnimationDropdown(_OptionList);
        ChangeSprite(0);
    }

    public void ChangeSprite(int spriteCount)
    {
        _AnimSpriteCount = spriteCount;

        if (_TileManager._Tiles[_UIManager._CurrentTile]._AnimationList.Count > 0) // Set to draw animation dictionary 
        {
            _UIManager._DrawToAnimateEvent.Invoke();
            _SpriteRenderer.sprite = _TileManager._Tiles[_UIManager._CurrentTile]._AnimationList[_UIManager._AnimationDropdown.value].animationSprites[_AnimSpriteCount];//_AnimationDictionary[_CurrentAnim][_AnimSpriteCount];
            _UIManager._AnimCounter.text = (_AnimSpriteCount + 1).ToString() + "/3";
        }
        else // Set single sprite
        {
            _UIManager._AnimateToDrawEvent.Invoke();
            _SpriteRenderer.sprite = _TileManager._Tiles[_UIManager._CurrentTile]._Image;
        }

        _Drawable.SetSprite();
    }

    /// <summary>
    /// Get selected picture from gallery and put it on the drawable surface
    /// </summary>
    public void PhotoToSprite(Sprite _sprite)
    {
        _SpriteRenderer.sprite.texture.LoadImage(_sprite.texture.EncodeToJPG());
    }

    public void RemoveGreenColor(Material _newMaterial)
    {
        _SpriteRenderer.material = _newMaterial;

        byte[] _bytes = _SpriteRenderer.sprite.texture.EncodeToJPG();

        string _photoTime = System.DateTime.Now.ToString("yyyyMMdd") + "-" + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString();

        File.WriteAllBytes(Application.persistentDataPath + "/" + _photoTime + ".jpg", _bytes);
    }
}
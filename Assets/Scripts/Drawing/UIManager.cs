using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public int _CurrentTile;

    [SerializeField] private GameObject _TileGenerationScreen;
    [SerializeField] private TMP_Dropdown _TileDropdown;
    [SerializeField] private TMP_Dropdown _TileEnumDropdown;
    [HideInInspector] public TMP_Dropdown _AnimationDropdown;
    [HideInInspector] private SaveDrawing _SaveDrawing;
    [SerializeField] private TileManager _TileManager;
    [SerializeField] private GameObject _MarkerHolder;
    [Space(10)]
    [SerializeField] private GameObject _TileGenBackground;

    private TMP_InputField _TileNameInput;
    private Camera _Cam;
    public UnityEvent _AnimateToDrawEvent, _DrawToAnimateEvent;
    
    [SerializeField] private List<AnimationSprites> _PlayerAnimations;
    [SerializeField] private List<AnimationSprites> _EnemyAnimations;
    [SerializeField] private GameObject _AnimationElements, _DrawingElements;
    private BoxCollider2D _DrawableObjectCollider;
    private SpriteRenderer _DrawableRenderer;
    [SerializeField] private Material _RegularMaterial, _GreenScreenMaterial;

    [Header("Canvasses")]
    [SerializeField] private Canvas DrawCanvas;
    [SerializeField] private Canvas GalleryCanvas;

    private Animator _TileGenerationAnimator;
    private Animator _MarkerHolderAnimator;
    private Animator _TileGenBackgroundAnimator;
    
    private bool _SetUi;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _TileManager = TileManager._Instance;
        _SaveDrawing = SaveDrawing._Instance;
        _TileNameInput = _TileGenerationScreen.GetComponentInChildren<TMP_InputField>();
        _TileEnumDropdown = _TileGenerationScreen.GetComponentInChildren<TMP_Dropdown>();

        _AnimateToDrawEvent.AddListener(EnableDrawing);
        _DrawToAnimateEvent.AddListener(EnableAnimating);
        _SetUi = false;

        DrawCanvas = GetComponent<Canvas>();

        DrawCanvas.enabled = true;

        GalleryCanvas.enabled = false;
        GalleryCanvas.gameObject.SetActive(true);

        _TileGenerationAnimator = _TileGenerationScreen.GetComponent<Animator>();
        _MarkerHolderAnimator = _MarkerHolder.GetComponent<Animator>();
        _TileGenBackgroundAnimator = _TileGenBackground.GetComponent<Animator>();

        _DrawableObjectCollider = GameObject.FindGameObjectWithTag("Drawable").GetComponent<BoxCollider2D>();

        _Cam = Camera.main;
        _DrawableRenderer = _DrawableObjectCollider.transform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_TileManager._Tiles.Count <= 0)
        {
            _TileGenerationScreen.SetActive(true);
            _TileGenBackground.SetActive(true);
            return;
        }
        else if (!_SetUi)
            SetUI();
    }

    public void GenerateTile()
    {
        ScriptableTile tile = null;

        if (_TileNameInput.text != "")
        {
            tile = (ScriptableTile)ScriptableObject.CreateInstance("ScriptableTile");
            tile._Name = _TileNameInput.text;

            StartCoroutine(CloseTileGenerationScreen());
        }
        else
        {
            Debug.Log("Please add a name.");
            _TileGenerationAnimator.SetTrigger("StartDeny");
            return;
        }
        
        tile._TileEnum = (TilesEnum)_TileEnumDropdown.value;

        switch ((TilesEnum)_TileEnumDropdown.value)
        {
            case TilesEnum.Block:

                //tile._Image = CreateImage(_TileNameInput.text, "Blank");

                break;
            case TilesEnum.Player:

                for (int i = 0; i < _PlayerAnimations.Count; i++)
                {
                    tile._AnimationList.Add(_PlayerAnimations[i]);
                }
                
                //_SaveDrawing.SetAnimationDictionary(tile._AnimationList);

                break;
            case TilesEnum.Enemy:

                for (int i = 0; i < _EnemyAnimations.Count; i++)
                {
                    tile._AnimationList.Add(_EnemyAnimations[i]);
                }
                //_SaveDrawing.SetAnimationDictionary(tile._AnimationList);

                break;
            case TilesEnum.Checkpoint:

                break;
            case TilesEnum.Finish:

                break;
            case TilesEnum.Wallpaper:

                //tile._Image = CreateImage(_TileNameInput.text, "WallPaper");

                break;
        }

        _TileManager._Tiles.Add(tile);

        _SaveDrawing.OrganiseList();
        SetTileDropdown();
        TileValueChanged();

        _TileDropdown.value = _TileManager._Tiles.Count;

        
    }

    public void SwitchSprite(bool forward)
    {
        if (forward)
            _SaveDrawing._AnimSpriteCount++;
        else
            _SaveDrawing._AnimSpriteCount--;
    }
    private void SetUI()
    {
        SetTileDropdown();
        TileValueChanged();
        _SetUi = true;
    }
    public void SetAnimationDropdown(TMP_Dropdown.OptionDataList list)
    {
        _AnimationDropdown.ClearOptions();
        _AnimationDropdown.AddOptions(list.options);

    }
    public void SetTileDropdown()
    {
        TMP_Dropdown.OptionDataList Options = new TMP_Dropdown.OptionDataList();
        
        for (int i = 0; i < _TileManager._Tiles.Count; i++)
        {
            TMP_Dropdown.OptionData item = new TMP_Dropdown.OptionData();

            item.text = _TileManager._Tiles[i]._Name;
            item.image = _TileManager._Tiles[i]._Image;

            Options.options.Add(item);
        }

        _TileDropdown.ClearOptions(); 
        _TileDropdown.AddOptions(Options.options);
    }

    Sprite CreateImage(string imageName, string ResourceName)
    {
        
        //Sprite itemBGSprite = Resources.Load<Sprite>(ResourceName);
        //Texture2D itemBGTex = itemBGSprite.texture;
        //byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        //File.WriteAllBytes("assets/resources/" + imageName + ".png", itemBGBytes);

        //AssetDatabase.Refresh();
        //Sprite image = Resources.Load<Sprite>(imageName);

        //string path = AssetDatabase.GetAssetPath(image);
        //TextureImporter A = (TextureImporter)AssetImporter.GetAtPath(path);
        //A.isReadable = true;
        //A.textureCompression = TextureImporterCompression.Uncompressed;
        //AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        return null;
    }

    public void TileValueChanged()
    {
        _SaveDrawing.ChangeSprite(0);

        switch (_TileManager._Tiles[_CurrentTile]._TileEnum)
        {
            case TilesEnum.Block:
                _DrawableObjectCollider.size = new Vector2(3, 3);
                _DrawableRenderer.material = _RegularMaterial;
                StartCoroutine(LerpCamSize(2));
                break;
            case TilesEnum.Player:
                _DrawableObjectCollider.size = new Vector2(3, 3);
                _DrawableRenderer.material = _GreenScreenMaterial;
                
                _SaveDrawing.OrganiseList();
                StartCoroutine(LerpCamSize(2));
                break;
            case TilesEnum.Enemy:
                _DrawableObjectCollider.size = new Vector2(3, 3);
                _SaveDrawing.OrganiseList();
                StartCoroutine(LerpCamSize(2));
                break;
            case TilesEnum.Checkpoint:
                _DrawableObjectCollider.size = new Vector2(3, 3);
                _DrawableRenderer.material = _RegularMaterial;
                StartCoroutine(LerpCamSize(2));
                break;
            case TilesEnum.Finish:
                _DrawableObjectCollider.size = new Vector2(3, 3);
                _DrawableRenderer.material = _RegularMaterial;
                StartCoroutine(LerpCamSize(2));
                break;
            case TilesEnum.Wallpaper:
                _DrawableObjectCollider.size = new Vector2(19.20f, 10.80f);
                _DrawableRenderer.material = _RegularMaterial;
                StartCoroutine(LerpCamSize(8));
                break; 
        }

        
    }

    public void SwitchTileScreen()
    {
        //_TileGenerationScreen.SetActive(!_TileGenerationScreen.activeSelf);

        if (_TileGenerationScreen.activeSelf)
        {
            StartCoroutine(CloseTileGenerationScreen());
            
            _MarkerHolder.SetActive(true);
        }
        else
        {
            _TileGenerationScreen.SetActive(true);
            _TileGenBackground.SetActive(true);
            CloseMarkerHolder();
        }

    }

    private void EnableDrawing()
    {
        _DrawingElements.SetActive(true);
        _AnimationElements.SetActive(false);
    }

    private void EnableAnimating()
    {
        _DrawingElements.SetActive(false);
        _AnimationElements.SetActive(true);
    }

    public void OpenGallery()
    {
        DrawCanvas.enabled = false;
        GalleryCanvas.enabled = true;
    }

    public void CloseGallery()
    {
        DrawCanvas.enabled = true;
        GalleryCanvas.enabled = false;
    }

    public void CloseMarkerHolder()
    {
        StartCoroutine(StartMarkerHolderClose());
    }

    private IEnumerator CloseTileGenerationScreen()
    {
        _TileGenerationAnimator.SetTrigger("StartExit");
        _TileGenBackgroundAnimator.SetTrigger("StartExit");

        yield return new WaitForSeconds(0.325f);

        _TileGenerationScreen.SetActive(false);
        _TileGenBackground.SetActive(false);

        _MarkerHolder.SetActive(true);
    }

    public IEnumerator StartMarkerHolderClose()
    {
        _MarkerHolderAnimator.SetTrigger("StartExit");

        yield return new WaitForSeconds(0.4f);

        _MarkerHolder.SetActive(false);
    }

    private IEnumerator LerpCamSize(float DesiredSize)
    {
        float timer = 0;
        float time = 1;

        float _CamSize = _Cam.orthographicSize;

        while(true)
        {
            _Cam.orthographicSize = Mathf.Lerp(_CamSize, DesiredSize, timer);

            if(timer > time)
            {
                break;
            }
            timer += 1 * Time.deltaTime;

            yield return null;
        }
    }
}

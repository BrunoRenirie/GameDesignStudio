using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher _Instance;
    private MusicPlayer _MusicPlayer;

    [HideInInspector] public UnityEvent _SceneSaveEvent, _SceneLoadEvent;

    [Header("Ui Elements")]
    [SerializeField] private Image _LoadingScreen;
    [SerializeField] private Image _ProgressBar;
    [SerializeField] private Image _ReturnButton;

    private bool _Starting = true;
    private int _OldScene;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        _MusicPlayer = GetComponentInChildren<MusicPlayer>();

        SceneManager.activeSceneChanged += SaveLevel;

        DontDestroyOnLoad(gameObject);

        _Starting = false;
    }

    public void SwitchScene()
    {
        if (!_Starting)
        {
            if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                _SceneSaveEvent.Invoke();
                if (_SceneLoadEvent.GetPersistentEventCount() > 0)
                    _SceneLoadEvent.Invoke();
            }
        }
        StartCoroutine(LoadNextScene(0));
    }

    public void ReturnButton()
    {
        SwitchScene();
        
    }

    private void SaveLevel(Scene current, Scene next)
    {
        if (next.buildIndex == 0)
            _ReturnButton.gameObject.SetActive(false);
        else
            _ReturnButton.gameObject.SetActive(true);
        /*
        if (SaveManager._Instance != null)
        {
            if (current.buildIndex == 3)
                _SceneSaveEvent.Invoke();
            if (_SceneLoadEvent.GetPersistentEventCount() > 0)
                _SceneLoadEvent.Invoke();
        }
        */
    }

    public IEnumerator LoadNextScene(int sceneToLoad)
    {
        AsyncOperation loadingLevel = SceneManager.LoadSceneAsync(sceneToLoad);

        _LoadingScreen.gameObject.SetActive(true);

        while (!loadingLevel.isDone)
        {
            _ProgressBar.fillAmount = loadingLevel.progress;
            yield return new WaitForEndOfFrame();
        }

        _LoadingScreen.gameObject.SetActive(false);
        _MusicPlayer.LoadLevel(sceneToLoad);
        
    }
}

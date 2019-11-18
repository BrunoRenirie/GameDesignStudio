using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class SceneSwitcher : MonoBehaviour
{
    private TMP_Dropdown _Dropdown;
    public static SceneSwitcher _Instance;

    public UnityEvent _SceneSaveEvent, _SceneLoadEvent;

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
        DontDestroyOnLoad(gameObject);
        _Dropdown = GetComponentInChildren<TMP_Dropdown>();

        _Dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropDownOptions = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            TMP_Dropdown.OptionData scene = new TMP_Dropdown.OptionData();
            
            if(i != SceneManager.GetActiveScene().buildIndex)
            {
                StartCoroutine(LoadUnloadScene(i));
                scene.text = SceneManager.GetSceneByBuildIndex(i).name;
            }
            else
                scene.text = SceneManager.GetSceneByBuildIndex(i).name;

            dropDownOptions.Add(scene);
        }

        _Dropdown.AddOptions(dropDownOptions);
        _Dropdown.value = SceneManager.GetActiveScene().buildIndex;
        _Dropdown.RefreshShownValue();

        _Starting = false;
    }

    public void SwitchScene()
    {
        // Later modulair systeem voor maken
        if (!_Starting)
        {
            if (_Dropdown.options[_Dropdown.value].text == "Level Editor")
            {
                _SceneSaveEvent.Invoke();
                _SceneLoadEvent.Invoke();
            }
            else
                _SceneSaveEvent.Invoke();
        }

        SceneManager.LoadScene(_Dropdown.value);
    }

    private IEnumerator LoadUnloadScene(int scene)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadScene.isDone);
        SceneManager.UnloadSceneAsync(scene);
    }

    public void ReturnButton()
    {
        SceneManager.LoadScene(0);
    }
}

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

        _Starting = false;
    }

    public void SwitchScene()
    {
        // Later modulair systeem voor maken
        if (!_Starting)
        {
            _SceneSaveEvent.Invoke();
            if (_SceneLoadEvent.GetPersistentEventCount() > 0)
                _SceneLoadEvent.Invoke();

        }

        //SceneManager.LoadScene(_Dropdown.value);
    }

    public void ReturnButton()
    {
        SceneManager.LoadScene(0);
    }
}

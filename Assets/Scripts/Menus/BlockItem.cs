using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockItem : MonoBehaviour
{
    [Header("Block Values")]
    public int _BlockNumber;
    [SerializeField] private string _BlockName;
    [Space]
    [SerializeField] private Text _BlockText;

    public void GiveNumber(int number)
    {
        _BlockNumber = number;
    }

    public void GiveName(string name)
    {
        _BlockName = name;
        _BlockText.text = _BlockName;
    }

    public void SelectBlock()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance._CurrentTile = _BlockNumber;
            UIManager.Instance.TileValueChanged();
        }
        else if(LevelEditorUi._Instance != null)
        {
            LevelEditorUi._Instance._CurrentTile = _BlockNumber;
        }

        //CloseScreen();
    }

    private void CloseScreen()
    {
        gameObject.SetActive(false);
    }
}

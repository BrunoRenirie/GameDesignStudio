using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGallery : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _HolderPanel;
    [SerializeField] private GameObject _BlockHolder;
    [Space]
    [SerializeField] private Image _ExampleImage;


    [Header("Blocks")]
    [SerializeField] private List<GameObject> _BlockList = new List<GameObject>();

    private BlockItem _BlockItemScript;


    public void OpenBlockPanel()
    {
        foreach (GameObject block in _BlockList)
        {
            Destroy(block);
        }

        for (int i = 0; i < TileManager._Instance._Tiles.Count; i++)
        {
            Image block = _ExampleImage;
            block.sprite = TileManager._Instance._Tiles[i]._Image;

            Image newBlock = null;

            newBlock = Instantiate(block, new Vector3(0, 0, 0), Quaternion.identity);

            _BlockItemScript = newBlock.GetComponent<BlockItem>();

            _BlockList.Add(newBlock.gameObject);

            newBlock.transform.parent = _BlockHolder.transform;

            _BlockItemScript._BlockNumber = i;
            _BlockItemScript.GiveName(TileManager._Instance._Tiles[i]._Name);

            newBlock.gameObject.SetActive(true);
        }

        _HolderPanel.SetActive(true);
    }

    public void CloseBlockPanel()
    {
        gameObject.SetActive(false);
    }
}

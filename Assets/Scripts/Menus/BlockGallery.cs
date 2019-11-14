using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockGallery : MonoBehaviour
{
    //[SerializeField] private List<Blokken> BlockList = new List<Blokken>();

    [Header("UI")]
    [SerializeField] private UIManager _UImanager;
    [SerializeField] private GameObject _HolderPanel;
    [SerializeField] private GameObject _BlockHolder;
    [Space]
    [SerializeField] private Image _ExampleImage;
    [SerializeField] private TextMeshProUGUI _ExampleText;

    [Header("Blocks")]
    [SerializeField] private List<Image> _BlockList = new List<Image>();


    void Start()
    {
        _HolderPanel.transform.localScale = new Vector3(0, 0, 0);
    }

    void Update()
    {
        
    }

    public void AddBlock(Sprite _block)
    {
        Image newBlock = null;

        newBlock = Instantiate(_ExampleImage, new Vector3(0, 0, 0), Quaternion.identity);

        newBlock.sprite = _block;

        newBlock.transform.SetParent(_BlockHolder.transform);
        newBlock.gameObject.SetActive(true);

        TextMeshProUGUI newText = newBlock.GetComponentInChildren<TextMeshProUGUI>();

        if (newText != null)
            newText.text = newBlock.sprite.name;

        _BlockList.Add(newBlock);
    }

    public void OpenBlockPanel()
    {
        _HolderPanel.transform.localScale = new Vector3(1, 1, 1);

        foreach (Image block in _BlockList)
        {
            block.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
    }

    public void CloseBlockPanel()
    {
        _HolderPanel.transform.localScale = new Vector3(0, 0, 0);

        foreach (Image block in _BlockList)
        {
            block.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}

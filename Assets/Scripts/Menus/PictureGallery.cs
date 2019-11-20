﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleFileBrowser;

public class PictureGallery : MonoBehaviour
{
    [SerializeField] private List<Image> PictureList = new List<Image>();

    [Header("Photo Objects")]
    [SerializeField] private GameObject PhotoHolder;
    [SerializeField] private Image Photo;
    [SerializeField] private LargePhotoScript BigPhotoScript;
    [SerializeField] private GameObject SelectedPhoto;
    [Space(10)]
    [SerializeField] private SaveDrawing SaveDrawingScript;

    [Header("Greenscreen")]
    [SerializeField] private Material RemoveGreenMaterial;

    private int ActivePicCount;

    private Object[] Pictures;

    private int PhotoAmount;

    private Canvas ObjectCanvas;

    private Animator PhotoHolderAnimator;

    private void Awake()
    {
        ObjectCanvas = GetComponent<Canvas>();

        var _info = new DirectoryInfo(Application.persistentDataPath);
        var _photoInfo = _info.GetFiles();

        if (PhotoAmount == _photoInfo.Length)
        {
            //Doe geen fuck.
        }
        else
        {
            foreach (var _item in _photoInfo)
            {
                Image _image = null;

                byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + _item.Name);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);

                Sprite _sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                _image = Instantiate(Photo, new Vector3(0, 0, 0), Quaternion.identity);

                _image.sprite = _sprite;
                _image.sprite.name = _item.Name;

                _image.gameObject.transform.SetParent(PhotoHolder.transform);
                _image.rectTransform.localScale = new Vector2(_image.sprite.texture.width / 550, _image.sprite.texture.height / 550);

                _image.gameObject.SetActive(true);

                PictureList.Add(_image);

                PhotoAmount = _photoInfo.Length;
            }

        }

        PhotoHolderAnimator = PhotoHolder.GetComponent<Animator>();
    }

    private void Update()
    {
        if(ObjectCanvas.enabled == true)
            LoadGallery();
        else
            BigPhotoScript.BigPhoto.gameObject.SetActive(false);
    }

    private void LoadGallery()
    {
        var _info = new DirectoryInfo(Application.persistentDataPath);
        var _photoInfo = _info.GetFiles();

        if (PhotoAmount < _photoInfo.Length)
        {
            int _amountDifference = _photoInfo.Length - PhotoAmount;

            for (int i = PhotoAmount; i < _photoInfo.Length; i++)
            {
                Image _image = null;
                var _item = _photoInfo[i];

                byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + _item.Name);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);

                Sprite _sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                _image = Instantiate(Photo, new Vector3(0, 0, 0), Quaternion.identity);

                _image.sprite = _sprite;
                _image.sprite.name = _item.Name;

                _image.gameObject.transform.SetParent(PhotoHolder.transform);
                _image.rectTransform.localScale = new Vector2(_image.sprite.texture.width / 550, _image.sprite.texture.height / 550);

                _image.gameObject.SetActive(true);

                PictureList.Add(_image);

                PhotoAmount += 1;
            }
        }
    }

    public void EnlargePhoto(Image _photoImage)
    {

        BigPhotoScript.gameObject.SetActive(true);

        BigPhotoScript.BigPhoto.sprite = _photoImage.sprite;
        BigPhotoScript.BigPhoto.rectTransform.localScale = new Vector2(BigPhotoScript.BigPhoto.sprite.texture.width / 650, BigPhotoScript.BigPhoto.sprite.texture.height / 650);

        SelectedPhoto = _photoImage.gameObject;
    }

    public void CloseLargePhoto()
    {
        BigPhotoScript.BeginPhotoClose();
    }

    public void DeletePhoto()
    {
        Sprite _sprite = BigPhotoScript.BigPhoto.sprite;

        File.Delete(Application.persistentDataPath + "/" + _sprite.name);

        Destroy(SelectedPhoto);

        CloseLargePhoto();
    }

    public void UsePhoto()
    {
        SaveDrawingScript.PhotoToSprite(BigPhotoScript.BigPhoto.sprite);
        CloseLargePhoto();
    }

    public void RemoveGreen()
    {
        SaveDrawingScript.RemoveGreenColor(RemoveGreenMaterial);
    }

    public void GalleryEffect()
    {
        PhotoHolderAnimator.SetTrigger("StartEnter");
    }

    /// <summary>
    /// Import from device gallery
    /// </summary>
    public void ImportPicture()
    {
        StartCoroutine(CopyImage());
    }

    IEnumerator CopyImage()
    {
        yield return FileBrowser.WaitForLoadDialog(false, @"content://com.android.externalstorage.documents/tree/primary%3A/document/primary%3APictures", "Kies een foto", "Importeer");

        if (FileBrowser.Success)
        {
            string _photoTime = System.DateTime.Now.ToString("yyyyMMdd") + "-" + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString();

            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);

            FileBrowserHelpers.WriteBytesToFile(FileBrowserHelpers.CreateFileInDirectory(Application.persistentDataPath + "/", _photoTime + ".png"), bytes);
        }
    }
}
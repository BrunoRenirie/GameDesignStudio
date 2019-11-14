using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryImage : MonoBehaviour
{
    private PictureGallery PictureGalleryScript;

    private Image PictureImage;

    void Start()
    {
        PictureGalleryScript = GetComponentInParent<PictureGallery>();

        PictureImage = GetComponent<Image>();
    }

    public void Enlarge()
    {
        PictureGalleryScript.EnlargePhoto(PictureImage);
    }
}

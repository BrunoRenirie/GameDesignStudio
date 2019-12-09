using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("Canvasses")]
    [SerializeField] private GameObject CameraCanvas;
    [SerializeField] private Canvas GalleryCanvas;

    [Header("PhoneCamera")]
    [SerializeField] private GameObject CameraPlane;

    private Animator CameraCanvasAnimator;

    void Start()
    {
        CameraPlane.transform.position = new Vector3(0, 0, 5);

        //CameraCanvas.SetActive(true);
        //GalleryCanvas.enabled = false;

        CameraCanvasAnimator = CameraCanvas.GetComponent<Animator>();

        OpenCamera();
    }

    private void Update()
    {
        if (CameraCanvas.activeSelf)
            CameraPlane.SetActive(true);
        else
            CameraPlane.SetActive(false);
    }

    public void OpenGallery()
    {
        CameraCanvas.SetActive(false);
        GalleryCanvas.enabled = true;
    }

    public void OpenCamera()
    {
        CameraCanvas.SetActive(true);
        GalleryCanvas.enabled = false;

        CameraCanvasAnimator.SetTrigger("StartEnter");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LargePhotoScript : MonoBehaviour
{
    [HideInInspector] public Image BigPhoto;
    private Animator PhotoAnimator;

    void Start()
    {
        BigPhoto = GetComponent<Image>();
        PhotoAnimator = GetComponent<Animator>();   
    }

    public void BeginPhotoClose()
    {
        PhotoAnimator.SetTrigger("StartExit");
    }

    public void ClosePhoto()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    private AudioSource buttonSource;

    void Start()
    {
        buttonSource = gameObject.AddComponent<AudioSource>();

    }
    public void ButtonClickSound(AudioClip buttonAudio)
    {
        buttonSource.clip = buttonAudio;
        buttonSource.Play();
    }
}

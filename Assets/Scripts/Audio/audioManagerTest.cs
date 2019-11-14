using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManagerTest : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            audioManager.Instance.playSFX(buttonClickSFX);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            audioManager.Instance.PlayMusic(music1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            audioManager.Instance.PlayMusic(music2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            audioManager.Instance.PlayMusicWithFade(music1);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            audioManager.Instance.PlayMusicWithFade(music2);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            audioManager.Instance.PlayMusicWithCrossFade(music1, 3);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            audioManager.Instance.PlayMusicWithCrossFade(music2, 3);
                }
}

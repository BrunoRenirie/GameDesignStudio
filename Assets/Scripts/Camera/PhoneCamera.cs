using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class PhoneCamera : MonoBehaviour
{
    [SerializeField] private WebCamTexture CameraTexture;
    [SerializeField] private GameObject CameraPlane;

    private string FrontCamName;
    private bool ActiveFrontCam;

    private void Awake()
    {

        CameraPlane = gameObject;

        ScalePlane();

        ActiveFrontCam = true;

        ChangeActiveCamera();
    }

    /// <summary>
    /// Scale plane to camera viewport
    /// </summary>
    private void ScalePlane()
    {
        Camera _cam = Camera.main;

        float _pos = (_cam.nearClipPlane + 0.01f);

        transform.position = _cam.transform.position + _cam.transform.forward * _pos;
        transform.LookAt(_cam.transform);
        transform.Rotate(90.0f, 0.0f, 0.0f);

        float _h = (Mathf.Tan(_cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * _pos * 5.6f);

        transform.localScale = new Vector3(_h * _cam.aspect, 1.0f, _h);
    }

    public void ChangeActiveCamera()
    {
        ActiveFrontCam = !ActiveFrontCam;

        CameraTexture = null;
        FrontCamName = "";

        if (ActiveFrontCam)
        {
            var _camDevices = WebCamTexture.devices;

            foreach (var _device in _camDevices)
            {
                if (_device.isFrontFacing)
                {
                    FrontCamName = _device.name;
                }
            }

            CameraTexture = new WebCamTexture(FrontCamName);

            //Matrix4x4 mat = Camera.main.projectionMatrix;
            //mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
            //Camera.main.projectionMatrix = mat;
        }
        else
        {
            CameraTexture = new WebCamTexture();
            Camera.main.ResetProjectionMatrix();
        }

        CameraPlane.GetComponent<Renderer>().material.mainTexture = CameraTexture;
        CameraTexture.Play();
    }

    public void TakePicture()
    {
        StartCoroutine(StartPicture());
    }

    private IEnumerator StartPicture()
    {
        yield return new WaitForEndOfFrame();

        Texture2D _photo = new Texture2D(CameraTexture.width, CameraTexture.height);

        _photo.SetPixels(CameraTexture.GetPixels());
        _photo.Apply();

        byte[] _bytes = _photo.EncodeToJPG();

        string _photoTime = System.DateTime.Now.ToString("yyyyMMdd") + "-" + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString();

        File.WriteAllBytes(Application.persistentDataPath + "/" + _photoTime + ".jpg", _bytes);

        Debug.Log("FOTO GEMAAKT!");
    }
}

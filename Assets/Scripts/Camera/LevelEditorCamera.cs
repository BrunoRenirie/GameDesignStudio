using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCamera : MonoBehaviour
{
    private Camera _Cam;
    private LevelEditorUi _EditorUi;

    private Vector3 _StartPos, _TouchStart;

    private float zoomOutMin = 5;
    private float zoomOutMax = 30;
    void Start()
    {
        _Cam = GetComponent<Camera>();
        _EditorUi = LevelEditorUi._Instance;
    }

    void Update()
    {
        if(_EditorUi._CurrentTool == EditorTools.Move)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _TouchStart = _Cam.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * 0.01f);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = _TouchStart - _Cam.ScreenToWorldPoint(Input.mousePosition);
                _Cam.transform.position += direction;
            }
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    void Zoom(float increment)
    {
        _Cam.orthographicSize = Mathf.Clamp(_Cam.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}

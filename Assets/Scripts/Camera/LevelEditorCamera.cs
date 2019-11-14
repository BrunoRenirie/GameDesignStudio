using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCamera : MonoBehaviour
{
    private Camera _Cam;
    private Vector3 _StartPos, _PosDifference;

    void Awake()
    {
        _Cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector3 newPos = _Cam.ScreenToWorldPoint(Input.mousePosition);
                _PosDifference = newPos - _StartPos;
            }

            _StartPos = _Cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else
            _PosDifference = new Vector3();
    }

    private void FixedUpdate()
    {
        transform.Translate(-_PosDifference);
    }
}

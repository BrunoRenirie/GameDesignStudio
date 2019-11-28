using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCamera : MonoBehaviour
{
    private Camera _Cam;
    private Vector3 _StartPos, _PosDifference;

    private LevelEditorUi _EditorUi;

    void Start()
    {
        _Cam = GetComponent<Camera>();
        _EditorUi = LevelEditorUi._Instance;
    }

    void Update()
    {
        return;

        if(_EditorUi._CurrentTool == EditorTools.Move)
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
    }

    private void FixedUpdate()
    {
        return;
        transform.Translate(-_PosDifference);
    }
}

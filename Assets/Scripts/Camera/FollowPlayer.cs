using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _FollowObj;
    [SerializeField]
    private float _FollowSpeed;

    private Vector3 _Offset = new Vector3(0,0,-10);
    private Vector3 _RefVelocity;

    private void OnEnable()
    {
        if (_FollowObj == null)
            _FollowObj = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (_FollowObj != null)
            transform.position = Vector3.SmoothDamp(transform.position, _FollowObj.transform.position + _Offset, ref _RefVelocity, _FollowSpeed);
    }
}

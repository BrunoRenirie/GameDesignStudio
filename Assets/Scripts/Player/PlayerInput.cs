using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Player _Player;
    [SerializeField] private InputButton _LeftMoveButton, _RightMoveButton;

    public void ToPlayMode()
    {
        gameObject.SetActive(true);
    }
    public void ToEditMode()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_Player == null)
            _Player = Player._Instance; //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (_LeftMoveButton.buttonPressed)
        {
            _Player._Velocity.x = -1;
        }
        else if (_RightMoveButton.buttonPressed)
        {
            _Player._Velocity.x = 1;
        }
        else
            _Player._Velocity.x = 0;
    }

    public void JumpNow()
    {
        _Player.Jump();
    }

    public void ShootNow()
    {
        _Player.Shoot(AttackTypes.Projectile);
    }
}
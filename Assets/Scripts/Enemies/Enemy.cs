using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Defeat,
    Jumping,
    Shooting,
    Hurt
}

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _Rb;
    private SpriteRenderer _Renderer;
    private Health _Health;
    private Animator _Anim;
    private BoxCollider2D _Collider;

    [SerializeField]
    private Vector2 _Velocity;
    [SerializeField]
    private LayerMask _GroundMask;
    [SerializeField]
    private float _MovementSpeed, _MeleeDamage;
    private bool _EditMode;
    public EnemyState _State;

    private Health _PlayerHealth;

    private void Start()
    {
        _Renderer = GetComponent<SpriteRenderer>();
        _Rb = GetComponent<Rigidbody2D>();
        _Health = GetComponent<Health>();
        _Anim = GetComponent<Animator>();
        _Collider = GetComponent<BoxCollider2D>();

        PlayModeSwitcher._Instance._SwitchEditMode.AddListener(EditMode);
        PlayModeSwitcher._Instance._SwitchPlaymode.AddListener(PlayMode);

        EditMode();
    }

    void Update()
    {
        if (_EditMode)
            return;

        if (CheckEdge() == false)
        {
            FlipDirection();
        }

        if (_Health._Dead)
            _State = EnemyState.Defeat;

        _Anim.SetInteger("State", (int)_State);        
    }

    void FixedUpdate()
    {
        if(_State == EnemyState.Move && !_EditMode)
            transform.Translate(_Velocity * _MovementSpeed * Time.deltaTime);
    }

    private bool CheckEdge()
    {
        if (_Velocity.x < 0)
        {           
            Debug.DrawRay(transform.position - new Vector3(_Collider.bounds.extents.x, _Collider.bounds.extents.y, 0), Vector2.down , Color.green);
            return Physics2D.Raycast(transform.position - new Vector3(_Collider.bounds.extents.x, _Collider.bounds.extents.y, 0), Vector2.down, .5f, _GroundMask);
        }
        else if (_Velocity.x > 0)
        {
            Debug.DrawRay(transform.position - new Vector3(-_Collider.bounds.extents.x, _Collider.bounds.extents.y, 0), Vector2.down, Color.red);
            return Physics2D.Raycast(transform.position - new Vector3(-_Collider.bounds.extents.x, _Collider.bounds.extents.y, 0), Vector2.down, .5f, _GroundMask);
        }
        else
            return true;
    }
    
    private void FlipDirection()
    {
        _Velocity.x = _Velocity.x > 0 ? -_Velocity.x : Mathf.Abs(_Velocity.x);
        _Renderer.flipX = !_Renderer.flipX;

        if (_Renderer.flipX)
        {
            _Renderer.material.SetTextureOffset("_MainTex", new Vector2(0, 0));
            _Renderer.material.SetTextureScale("_MainTex", new Vector2(1, 1));
            
        }
        else
        {
            _Renderer.material.SetTextureOffset("_MainTex", new Vector2(1, 0));
            _Renderer.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_Health._Dead || _EditMode)
            return;

        if(collision.gameObject.CompareTag("Player"))
        {
            if(_PlayerHealth == null)
                _PlayerHealth = collision.gameObject.GetComponent<Health>();

            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.contacts[i].normal == Vector2.down)
                {
                    _Health.TakeDamage(_Health._hp, AttackTypes.Crush, transform.position);
                }
                if (collision.contacts[i].normal == -_Velocity)
                {
                    _Anim.SetTrigger("Attack");

                    _PlayerHealth.TakeDamage(_MeleeDamage, AttackTypes.Melee, transform.position);
                }
            }
        }
        else
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if(collision.contacts[i].normal == -_Velocity)
                {
                    FlipDirection();
                }
            }
            
        }
    }
    public void EditMode()
    {
        _Rb.isKinematic = true;
        _Rb.velocity = Vector3.zero;
        _EditMode = true;
        _Health.ResetHealth();
    }
    public void PlayMode()
    {
        _Rb.isKinematic = false;
        _EditMode = false;
    }
}

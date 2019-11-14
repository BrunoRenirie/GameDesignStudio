using System;
using System.Collections;
using UnityEngine;

public enum PlayerState
{
    idle,
    moving,
    jumping,
    falling,
    Shoot,
    Duck,
    Hurt,
    Defeat,
    SecondaryAttack
}

public class Player : MonoBehaviour
{
    public static Player _Instance;

    private Rigidbody2D _Rb;
    private SpriteRenderer _Renderer;
    private BoxCollider2D _Collider;
    private Animator _Animator;
    private Health _Health;

    [SerializeField] private LayerMask _GroundMask;
    [SerializeField] private PlayerState _State;
    [SerializeField] private GameObject _Projectile;

    [SerializeField] private float _JumpHeight;
    [SerializeField] private float _MoveSpeed;

    [HideInInspector] public Vector2 _Velocity;
    private bool _Slowing;
    private float _RefFloat;
    private bool _Frozen, _EditMode;

    public event Action<PlayerState> OnStateChange;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        PlayModeSwitcher._Instance._SwitchEditMode.AddListener(EditMode);
        PlayModeSwitcher._Instance._SwitchPlaymode.AddListener(PlayMode);

        _Rb = GetComponent<Rigidbody2D>();
        _Renderer = GetComponent<SpriteRenderer>();
        _Collider = GetComponent<BoxCollider2D>();
        _Animator = GetComponent<Animator>();
        _Health = GetComponent<Health>();

        EditMode();
    }

    private void Update()
    {
        if (_EditMode)
            return;

        //_Velocity.x = Input.GetAxis("Horizontal");

        if (_Rb.velocity.y != 0)
            _Velocity.x = Mathf.SmoothDamp(_Velocity.x, 0, ref _RefFloat, 1);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        StateUpdate();

        if (_Health._Dead)
        {
            _Frozen = true;
            _State = PlayerState.Defeat;
        }

        _Animator.SetInteger("State", (int)_State);
    }

    private void FixedUpdate()
    {
        if (!_Frozen)
            transform.Translate(_Velocity * _MoveSpeed * Time.fixedDeltaTime);
    }

    private void StateUpdate()
    {
        if (_Velocity.x < 0)
        {
            if (_State != PlayerState.moving) OnStateChange(_State);
            _Renderer.flipX = true;
            _State = PlayerState.moving;
            
        }
        else if (_Velocity.x > 0)
        {
            if (_State != PlayerState.moving) OnStateChange(_State);
            _Renderer.flipX = false;
            _State = PlayerState.moving;
         
        }

        if (_Rb.velocity.y > 0 && !Grounded())
        {
            if (_State != PlayerState.jumping) OnStateChange(_State);
            _State = PlayerState.jumping;
            
        }
        if (_Rb.velocity.y < 0 && !Grounded())
        {
            if (_State != PlayerState.falling) OnStateChange(_State);
            _State = PlayerState.falling;
            
        }

        if (_Rb.velocity.y == 0 && _Velocity == Vector2.zero)
        {
            if (_State != PlayerState.idle) OnStateChange(_State);
            _State = PlayerState.idle;
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (_State != PlayerState.Duck) OnStateChange(_State);
            _State = PlayerState.Duck;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_State != PlayerState.Shoot) OnStateChange(_State);
            _State = PlayerState.Shoot;
        }
    }

    public void Jump()
    {
        if(Grounded())
            _Rb.AddForce(Vector2.up * _JumpHeight);
    }

    private bool Grounded()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (_Collider.size.y / 2)), Vector2.down, 0.4f, _GroundMask);
    }

    public void Shoot(AttackTypes projectileType)
    {
        GameObject projectile = null;

        switch(projectileType)
        {
            case AttackTypes.Projectile:
                projectile = Instantiate(_Projectile);
                break;
        }

        Projectile script = projectile.GetComponent<Projectile>();
        if (_Renderer.flipX) //Right
        {
            projectile.transform.position = transform.position + new Vector3(_Collider.size.x / 2, 0, 0);
            Vector3 velocity = new Vector3(1, _Rb.velocity.normalized.y * .5f, 0);
            script.Init(velocity, _Renderer.flipX);
        }
        else // Left
        {
            projectile.transform.position = transform.position - new Vector3(_Collider.size.x / 2, 0, 0);
            Vector3 velocity = new Vector3(-1, _Rb.velocity.normalized.y * .5f, 0);
            script.Init(velocity, _Renderer.flipX);
        }
        
    }

    public void FreezeChar(int value)
    {
        if (value == 0)
            _Frozen = false;
        if (value == 1)
            _Frozen = true;
    }

    public void EditMode()
    {
        _Rb.isKinematic = true;
        _Rb.velocity = Vector3.zero;
        _EditMode = true;
    }
    public void PlayMode()
    {
        _Rb.isKinematic = false;
        _EditMode = false;
    }
}

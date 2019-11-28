using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BosssState
{
    Idle,
    SpecialAttack,
    Charge,
    Move,
    Attack,
    Dead,
    Jumping,
}

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Animator _Anim;
    [SerializeField]
    private Vector2 _Velocity;
    [SerializeField]
    private LayerMask _GroundMask;
    [SerializeField]
    private float _MovementSpeed, _MeleeDamage, _Range;

    private SpriteRenderer _Renderer;
    private Health _Health;
    private BoxCollider2D _Collider;
    private BosssState _State;
    private BosssState _PreviousState;
    private Health _PlayerHealth;
    private bool _IsBusy;
    private void Awake()
    {
        _Renderer = GetComponent<SpriteRenderer>();
        _Health = GetComponent<Health>();
        _Collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _State = BosssState.Move;
        //StartCoroutine(JumpTimer());
    }
    private void Update()
    {
        if (!IsPlayerClose())
        {
            _State = BosssState.Move;
        }


        if (_State == BosssState.Move)
        {
            BorderControl();
            _Anim.SetBool("isWalking", true);
        }
        else
        {
            _Anim.SetBool("isWalking", false);
        }

        if (_State == BosssState.Attack)
        {
            Debug.Log("Attack!");
            _Anim.SetBool("Attack",true);
        }
        else
        {
            _Anim.SetBool("Attack", false);
        }
    }
    void FixedUpdate()
    {
        if (_State == BosssState.Move)
        {
            Move();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_PlayerHealth == null)
                _PlayerHealth = collision.gameObject.GetComponent<Health>();

            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.contacts[i].normal == Vector2.down)
                {
                    _Health.TakeDamage(_Health._hp, AttackTypes.Crush, transform.position);
                }
                if (collision.contacts[i].normal == -_Velocity)
                {
                    _State = BosssState.Attack;
                }
            }
        }
        else
        {
            //probably when an object is in the way
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.contacts[i].normal == -_Velocity)
                {
                    FlipDirection();
                }
            }

        }
    }
    private void BorderControl()
    {
        //Doesn't go out of the scene
        if (CheckEdge() == false)
        {
            //GetComponent<Rigidbody2D>().AddForce(_Velocity + new Vector2(0, 10), ForceMode2D.Impulse);
            FlipDirection();
        }
    }
    private bool CheckEdge()
    {
        if (_Velocity.x < 0)
        {
            Debug.DrawRay(transform.position - new Vector3(_Collider.bounds.extents.x, _Collider.bounds.extents.y, 0), Vector2.down, Color.green);
            return Physics2D.Raycast(transform.position - new Vector3(_Collider.bounds.extents.x, _Collider.bounds.extents.y, 0), Vector2.down, 4f, _GroundMask);
        }
        else if (_Velocity.x > 0)
        {
            Debug.DrawRay(transform.position - new Vector3(-_Collider.bounds.extents.x, _Collider.bounds.extents.y, 0), Vector2.down, Color.red);
            return Physics2D.Raycast(transform.position - new Vector3(-_Collider.bounds.extents.x, _Collider.bounds.extents.y, 0), Vector2.down, 4f, _GroundMask);
        }
        else
            return true;
    }

    private void FlipDirection()
    {
        _Velocity.x = _Velocity.x > 0 ? -_Velocity.x : Mathf.Abs(_Velocity.x);
        _Renderer.flipX = !_Renderer.flipX;
    }

    private bool IsPlayerClose()
    {
        bool thisBool;

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float dist = Vector2.Distance(playerTransform.position, transform.position);

        Debug.Log(dist);

        if (dist > _Range)
        {
            thisBool = false;
        }
        else
        {
            thisBool = true;
        }

        return thisBool;
    }

    private void Move()
    {
        transform.Translate(_Velocity * _MovementSpeed * Time.deltaTime);
    }
}

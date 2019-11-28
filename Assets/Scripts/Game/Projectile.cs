using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileState
{
    Moving,
    Hit,
    HitWall
}

public enum AttackTypes
{
    Projectile,
    Melee,
    Crush,
}


public class Projectile : MonoBehaviour
{
    [SerializeField]
    private ProjectileState _State;
    [SerializeField]
    private AttackTypes _ProjectileType;
    [SerializeField]
    private float _MovementSpeed;
    [SerializeField]
    private float _Damage;

    private Vector3 _Velocity;
    private SpriteRenderer _Renderer;
    private Animator _Anim;

    private void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Renderer = GetComponent<SpriteRenderer>();

        StartCoroutine(DestroyGameobject());
    }
    private void Update()
    {
        _Anim.SetInteger("State", (int)_State);
        transform.Translate(_Velocity * _MovementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            _State = ProjectileState.Hit;
            _Velocity = Vector3.zero;
            Health enemyHealth = other.GetComponent<Health>();
            enemyHealth.TakeDamage(_Damage, _ProjectileType, transform.position);
            Destroy();
        }
        if (other.gameObject.layer == 11)
        {
            _State = ProjectileState.HitWall;
            _MovementSpeed = 0;
        }
    }

    public void Init(Vector3 velocity, bool flipX)
    {
        _Velocity = velocity;
        _Renderer.flipX = flipX;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private IEnumerator DestroyGameobject()
    {
        yield return new WaitForSeconds(10);
        Destroy();
    }
}

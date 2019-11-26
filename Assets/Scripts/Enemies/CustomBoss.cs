using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle,
    Move,
    Attack,
    Charge,
    SpecialAttack,
    StartAttack
}

public class CustomBoss : MonoBehaviour
{
    [SerializeField] int specialAttackChance = 15, jumpChance = 3, jumpForce = 15, damageSelf, damagePlayer;

    [SerializeField]
    float range = 4f, minShootRange = 1f, maxShootRange = 5f,
    specialAttackDuration = 2f,  movementSpeed = 4f;

    [SerializeField]
    private SpriteRenderer renderer;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private LayerMask _GroundMask;
    [SerializeField]
    private GameObject fireballPrefab;

    private Rigidbody2D rb;
    private BossState state;
    private BoxCollider2D collider;
    private Health playerHealth, health;
    private GameObject player;
    private Vector2 velocity = new Vector2(-1, 0);

    private float updateInterval = 0.5f, chargeDuration = 4f, idleDuration = 4f;

    private int layerMaskSelf = ~(1 << 10);
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        collider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<Health>();
        health = GetComponent<Health>();
    }
    private void Start()
    {
        StartCoroutine(IdleState());
    }
    private void Update()
    {
        if (CheckEdge() == false)
        {
            FlipDirection();
        }

        if (transform.position.y < -70f)
        {
            transform.position = player.transform.position + new Vector3(0, 7, 0);
        }

        if (health._Dead == true)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (state == BossState.Move)
        {
            transform.Translate(velocity * movementSpeed * Time.deltaTime);
        }

        if (state == BossState.SpecialAttack)
        {
            Vector3 refv = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + new Vector3(0, 7, 0), ref refv, 0.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth == null)
            {
                playerHealth = collision.gameObject.GetComponent<Health>();
            }

            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.contacts[i].normal == Vector2.down)
                {
                    if (state == BossState.Charge)
                    {
                        health.TakeDamage(damageSelf, AttackTypes.Crush, transform.position);
                        anim.Play("Hurt");
                        anim.ResetTrigger("Hurt");
                        player.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2), ForceMode2D.Impulse);

                        state = BossState.Move;
                        StopAllCoroutines();
                        StartCoroutine(MoveState());
                    }
                    else
                    {
                        player.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.contacts[i].normal == -velocity)
                {
                    FlipDirection();
                }
            }
        }
    }

    private IEnumerator IdleState()
    {
        while (true)
        {
            if (state == BossState.Idle)
            {
                anim.ResetTrigger("SpecialAttack");
                anim.SetTrigger("Idle");
                yield return new WaitForSeconds(idleDuration);
                state = BossState.Move;
                StartCoroutine(MoveState());
                break;
            }
            else
            {
                break;
            }
        }
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator MoveState()
    {
        while (true)
        {
            if (state == BossState.Move)
            {
                anim.ResetTrigger("Idle");
                anim.SetTrigger("Walk");
                Vector2 dir = transform.TransformDirection((player.transform.position - transform.position));

                Transform playerTransform = player.transform;
                float dist = Vector2.Distance(playerTransform.position, transform.position);
                if (dist > minShootRange && dist < maxShootRange)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, layerMaskSelf);
                    if (hit.collider.transform.tag == "Player")
                    {
                        anim.Play("ChargeTrill");
                        yield return new WaitForSeconds(0.9f);
                        GameObject bullet = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
                        bullet.transform.rotation = Quaternion.LookRotation(dir);
                        bullet.GetComponent<Rigidbody2D>().AddForce(dir * 100);
                        yield return new WaitForSeconds(0.2f);
                        GameObject bullet_one = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
                        bullet_one.transform.rotation = Quaternion.LookRotation(dir);
                        bullet_one.GetComponent<Rigidbody2D>().AddForce(dir * 100);
                        yield return new WaitForSeconds(0.2f);
                        GameObject bullet_two = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
                        bullet_two.transform.rotation = Quaternion.LookRotation(dir);
                        bullet_two.GetComponent<Rigidbody2D>().AddForce(dir * 100);
                        yield return new WaitForSeconds(0.2f);
                    }
                }

                yield return new WaitForSeconds(updateInterval);

                if (CanAttack())
                {
                    state = BossState.Attack;
                    StartCoroutine(AttackState());
                    break;
                }
                else if (CanAttack() == false && state == BossState.Move && 1 == Random.Range(0, specialAttackChance + 1))
                {
                    state = BossState.Charge;
                    StartCoroutine(ChargeState());
                    break;
                }

                else if (CanAttack() == false && state == BossState.Move && 1 == Random.Range(0, jumpChance + 1))
                {
                    anim.Play("Jump");
                    rb.AddForce(velocity + new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }
            }
            else
            {
                break;
            }
        }
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator AttackState()
    {
        while (true)
        {
            if (state == BossState.Attack)
            {
                anim.ResetTrigger("Walk");
                anim.SetTrigger("Attack");
                playerHealth.TakeDamage(damagePlayer, AttackTypes.Melee, transform.position);
                yield return new WaitForSeconds(updateInterval);

                if (!CanAttack())
                {
                    anim.ResetTrigger("Attack");
                    state = BossState.Move;
                    StartCoroutine(MoveState());
                    break;
                }
            }
            else
            {
                break;
            }
        }
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator ChargeState()
    {
        while (true)
        {
            if (state == BossState.Charge)
            {
                anim.ResetTrigger("Walk");
                anim.SetTrigger("Charge");
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Charge"))
                {
                    anim.Play("Charge");
                }
                yield return new WaitForSeconds(chargeDuration);
                state = BossState.SpecialAttack;
                StartCoroutine(SpecialAttackState());
                break;
            }
            else 
            {
                break;
            }
        }
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator SpecialAttackState()
    {
        while (true)
        {
            if (state == BossState.SpecialAttack)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Charge"))
                {
                    anim.Play("Charge");
                }
                float oldGravityScale = rb.gravityScale;
                rb.gravityScale = 0;
                anim.ResetTrigger("Charge");
                anim.SetTrigger("SpecialAttack");
                rb.isKinematic = true;
                collider.enabled = false;
                yield return new WaitForSeconds(2.0f);
                Instantiate(fireballPrefab, player.transform.position + new Vector3(0, 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Instantiate(fireballPrefab, player.transform.position + new Vector3(0, 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.3f);
                Instantiate(fireballPrefab, player.transform.position + new Vector3(0, 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
                Instantiate(fireballPrefab, player.transform.position + new Vector3(0, 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
                Instantiate(fireballPrefab, player.transform.position + new Vector3(0, 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.6f);
                Instantiate(fireballPrefab, player.transform.position + new Vector3(0, 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.7f);
                yield return new WaitForSeconds(specialAttackDuration);
                collider.enabled = true;
                rb.isKinematic = false;
                rb.gravityScale = oldGravityScale;
                anim.ResetTrigger("Idle");
                state = BossState.Idle;
                StartCoroutine(IdleState());
                break;
            }
            else
            {
                break;
            }
        }
        yield return new WaitForSeconds(0f);
    }

    private void FlipDirection()
    {
        velocity.x = velocity.x > 0 ? -velocity.x : Mathf.Abs(velocity.x);
        renderer.flipX = !renderer.flipX;
    }
    private bool CanAttack()
    {
        bool thisBool;
        Transform playerTransform = player.transform;
        float dist = Vector2.Distance(playerTransform.position, transform.position);
        if (dist > range)
        {
            thisBool = false;
        }
        else
        {
            thisBool = true;
        }

        return thisBool;
    }

    private bool CheckEdge()
    {
        if (velocity.x < 0)
        {
            Debug.DrawRay(transform.position - new Vector3(collider.bounds.extents.x, collider.bounds.extents.y, 0), Vector2.down, Color.green);
            return Physics2D.Raycast(transform.position - new Vector3(collider.bounds.extents.x, collider.bounds.extents.y, 0), Vector2.down, 30f, _GroundMask);
        }
        else if (velocity.x > 0)
        {
            Debug.DrawRay(transform.position - new Vector3(-collider.bounds.extents.x, collider.bounds.extents.y, 0), Vector2.down, Color.red);
            return Physics2D.Raycast(transform.position - new Vector3(-collider.bounds.extents.x, collider.bounds.extents.y, 0), Vector2.down, 30f, _GroundMask);
        }
        else
            return true;
    }

    private void ShootTowardsPlayer()
    {

        Vector2 dir = transform.TransformDirection((player.transform.position - transform.position));

        Transform playerTransform = player.transform;
        float dist = Vector2.Distance(playerTransform.position, transform.position);
        Debug.Log(dist);
        if (dist > minShootRange && dist < maxShootRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, layerMaskSelf);
            if (hit.collider.transform.tag == "Player")
            {
                GameObject bullet = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
                bullet.transform.rotation = Quaternion.LookRotation(dir);
                bullet.GetComponent<Rigidbody2D>().AddForce(dir * 100);
            }
        }
    }
}

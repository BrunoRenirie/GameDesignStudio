using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Player")
        {
            collision.transform.gameObject.GetComponent<Health>().TakeDamage(damage, AttackTypes.Crush, transform.position);
            Destroy(gameObject);
        }
    }
}
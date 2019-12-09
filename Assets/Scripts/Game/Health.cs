using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public float _hp;
    public bool _Dead;

    private float _maxHP;
    private bool _Invincible;

    private Rigidbody2D _Rb;
    private Image _HealthSlider;

    private void Awake()
    {
        _Rb = GetComponent<Rigidbody2D>();
        _HealthSlider = GetComponentInChildren<Image>();
        _maxHP = _hp;
    }

    private void Update()
    {
        _hp = Mathf.Clamp(_hp, 0, _maxHP);
        if(_hp == 0 && _Dead == false)
        {
            _Dead = true;
        }

        _HealthSlider.fillAmount = 1 + ((_hp - _maxHP) / _maxHP);

        if (LevelEditorManager._Instance._Editing)
            _HealthSlider.gameObject.SetActive(false);
        else
            _HealthSlider.gameObject.SetActive(true);
    }

    public void TakeDamage(float damage, AttackTypes type, Vector2 origin)
    {
        if (_Invincible)
            return;

        _hp -= damage;
        switch (type)
        {
            case AttackTypes.Projectile:

                KnockBack(origin, 30);
                StartCoroutine(HealthCooldown(1));

                break;
            case AttackTypes.Melee:

                KnockBack(origin, 100);
                StartCoroutine(HealthCooldown(1));

                break;
            case AttackTypes.Crush:
                


                break;
        }
    }

    private void KnockBack(Vector2 origin, float multiplier)
    {
        Vector2 force = new Vector2(transform.position.x, transform.position.y) - origin;
        _Rb.AddForce(force * multiplier);
    }
    
    private IEnumerator HealthCooldown(float time)
    {
        _Invincible = true;
        yield return new WaitForSecondsRealtime(time);
        _Invincible = false;
    }

    public void ResetHealth()
    {
        _hp = _maxHP;
    }
}

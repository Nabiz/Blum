using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private BoxCollider2D _attackCollider;
    [SerializeField] private float _attackDuration = 2.0f;
    private bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        _attackCollider = GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
        _attackCollider.enabled = false;
    }

    public void PerformAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackCorutine());
        }

    }
    private IEnumerator AttackCorutine()
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
        _attackCollider.enabled = true;
        yield return new WaitForSeconds(_attackDuration);
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
        _attackCollider.enabled = false;
        isAttacking = false;
    }
}

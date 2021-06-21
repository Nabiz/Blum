using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackDuration = 0.35f;
    private BoxCollider2D attackCollider;
    private bool isAttacking = false;
    void Start()
    {
        attackCollider = GetComponent<BoxCollider2D>();
        attackCollider.enabled = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
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
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        attackCollider.enabled = false;
        isAttacking = false;
    }
}

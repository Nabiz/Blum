using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    private GameObject player;
    private bool isAttacking = false;
    [SerializeField] private float attackDuration = 1f;

    new private void Start()
    {
        player = GameObject.Find("Player");
        health = 3;
        base.Start();
    }

    private void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < 2f && !isAttacking && IsRotatedToPlayer())
            {
                StartCoroutine(PerformAttack());
            }
        }
    }

    private bool IsRotatedToPlayer()
    {
        if ((player.transform.position.x <= transform.position.x && Mathf.Abs(transform.rotation.y) == 1)
            || (player.transform.position.x >= transform.position.x && transform.rotation.y == 0))
            return true;
        else
            return false;
    }
    IEnumerator PerformAttack()
    {
        animator.SetTrigger("IsAttacking");
        isAttacking = true;
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }
}

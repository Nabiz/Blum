using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected List<float> patrolPoints;
    protected int currentPatrolPointIndex = 0;
    [SerializeField] protected float movementSpeed = 1.5f;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected int health = 2;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (patrolPoints.Count != 0)
        {
            StartCoroutine(MoveToPatrolPoint(patrolPoints[currentPatrolPointIndex]));
        }
    }

    protected IEnumerator MoveToPatrolPoint(float patrolPoint)
    {
        float startPosX = rb.position.x;
        float endPosX = patrolPoint;
        float wholeDistance = Mathf.Abs(startPosX - endPosX);
        float starTime = Time.time;
        float coveredDistance = (Time.time - starTime) * movementSpeed;
        float distanceFraction = coveredDistance / wholeDistance;
        
        if (startPosX > endPosX)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        
        while (distanceFraction < 1)
        {
            animator.SetBool("IsRunning", true);
            coveredDistance = (Time.time - starTime) * movementSpeed;
            distanceFraction = coveredDistance / wholeDistance;
            rb.position = new Vector2(startPosX + distanceFraction * (endPosX - startPosX), rb.position.y);
            yield return null;
        }
        animator.SetBool("IsRunning", false);

        if (patrolPoints.Count > 1)
        {
            currentPatrolPointIndex++;
            if (currentPatrolPointIndex == patrolPoints.Count)
                currentPatrolPointIndex = 0;
            StartCoroutine(MoveToPatrolPoint(patrolPoints[currentPatrolPointIndex]));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            health--;
            animator.SetTrigger("IsHitted");
            if (health <= 0)
                Destroy(gameObject);
        }
    }
}

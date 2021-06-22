using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<float> patrolPoints;
    private int currentPatrolPointIndex = 0;
    [SerializeField] private float movementSpeed = 1.5f;
    private Rigidbody2D rb;
    private int health = 2;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (patrolPoints.Count != 0)
        {
            StartCoroutine(MoveToPatrolPoint(patrolPoints[currentPatrolPointIndex]));
        }

    }

    IEnumerator MoveToPatrolPoint(float patrolPoint)
    {
            float startPosX = rb.position.x;
            float endPosX = patrolPoint;
            float wholeDistance = Mathf.Abs(startPosX - endPosX);
            float starTime = Time.time;

            float coveredDistance = (Time.time - starTime) * movementSpeed;
            float distanceFraction = coveredDistance / wholeDistance;

            while(distanceFraction < 1)
            {
                coveredDistance = (Time.time - starTime) * movementSpeed;
                distanceFraction = coveredDistance / wholeDistance;
                rb.position = new Vector2(startPosX + distanceFraction * (endPosX - startPosX), rb.position.y);
                yield return null;
            }

        if (patrolPoints.Count > 1)
        {
            currentPatrolPointIndex++;
            if(currentPatrolPointIndex == patrolPoints.Count)
                currentPatrolPointIndex = 0;
            StartCoroutine(MoveToPatrolPoint(patrolPoints[currentPatrolPointIndex]));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            health--;
            if (health <= 0)
                Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<float> _patrolPoints;
    private int _currentPatrolPointIndex = 0;
    [SerializeField] private float _movementSpeed;
    private Rigidbody2D _rigidbody;
    private int _health = 2;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_patrolPoints.Count != 0)
        {
            StartCoroutine(MoveToPatrolPoint(_patrolPoints[_currentPatrolPointIndex]));
        }

    }

    IEnumerator MoveToPatrolPoint(float patrolPoint)
    {
            float startPosX = _rigidbody.position.x;
            float endPosX = patrolPoint;
            float wholeDistance = Mathf.Abs(startPosX - endPosX);
            float starTime = Time.time;

            float coveredDistance = (Time.time - starTime) * _movementSpeed;
            float distanceFraction = coveredDistance / wholeDistance;

            while(distanceFraction < 1)
            {
                coveredDistance = (Time.time - starTime) * _movementSpeed;
                distanceFraction = coveredDistance / wholeDistance;
                _rigidbody.position = new Vector2(startPosX + distanceFraction * (endPosX - startPosX), _rigidbody.position.y);
                yield return null;
            }
        if (_patrolPoints.Count > 1)
        {
            _currentPatrolPointIndex++;
            if (_currentPatrolPointIndex == _patrolPoints.Count)
                _currentPatrolPointIndex = 0;
            StartCoroutine(MoveToPatrolPoint(_patrolPoints[_currentPatrolPointIndex]));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            _health--;
            if (_health <= 0)
                Destroy(gameObject);
        }
    }
}

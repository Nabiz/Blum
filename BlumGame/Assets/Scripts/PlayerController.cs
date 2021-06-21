using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _movement_speed = 10.0f;
    [SerializeField] private float _jump_force = 10.0f;
    [SerializeField] private GUIManager _guiManager;
    private Rigidbody2D _rigidbody;
    private GroundDetector _groundDetector;
    private Attack _attack;
    private bool _isFacedRight = true;
    public bool _isHitted;
    public Animator _animator;

    private int _health = 3;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = transform.Find("GroundDetector").GetComponent<GroundDetector>();
        _attack = transform.Find("Attack").GetComponent<Attack>();
        _animator = GetComponent<Animator>();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _groundDetector.IsGrounded() && !_groundDetector.IsRecoiled())
        {
            _groundDetector.SetGrounded(false);
            _animator.SetBool("IsJumping", true);
            _rigidbody.AddForce(_jump_force * Vector2.up, ForceMode2D.Impulse);
        }
        if(Input.GetKeyDown(KeyCode.X) && _groundDetector.IsGrounded())
        {
            _attack.PerformAttack();
            _animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (!_groundDetector.IsRecoiled() && !_attack.isAttacking)
        {
            float velocity = horizontalInput * Time.fixedDeltaTime * _movement_speed;
            _rigidbody.position +=  velocity * Vector2.right;
            _animator.SetFloat("Speed", Mathf.Abs(velocity));
        }

        if (_isFacedRight && horizontalInput < -0.1f && !_attack.isAttacking)
        {
            _isFacedRight = false;
            gameObject.transform.Rotate(0f, 180f, 0f);
            
        }
        else if(!_isFacedRight && horizontalInput > 0.1f && !_attack.isAttacking)
        {
            _isFacedRight = true;
            gameObject.transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            --_health;
            _groundDetector.SetRecoiled(true);
            _guiManager.LoseHealth();
            if (_health == 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                _isHitted = true;
                _animator.SetBool("IsHitted", true);
                GameObject enemy = collision.gameObject;
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(new Vector2(Mathf.Sign(transform.position.x - enemy.transform.position.x) * 4f, 3f), ForceMode2D.Impulse);
            }

        }
    }

    IEnumerator Die()
    {
        _animator.SetTrigger("IsDead");
        _rigidbody.Sleep();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

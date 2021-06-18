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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = transform.Find("GroundDetector").GetComponent<GroundDetector>();
        _attack = transform.Find("Attack").GetComponent<Attack>();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _groundDetector.IsGrounded() && !_groundDetector.IsRecoiled())
        {
            _groundDetector.SetGrounded(false);
            _rigidbody.AddForce(_jump_force * Vector2.up, ForceMode2D.Impulse);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            _attack.PerformAttack();
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if(!_groundDetector.IsRecoiled())
            _rigidbody.position += horizontalInput * Time.fixedDeltaTime * _movement_speed * Vector2.right;
        if(_isFacedRight && horizontalInput < -0.1f)
        {
            _isFacedRight = false;
            gameObject.transform.Rotate(0f, 180f, 0f);
        }
        else if(!_isFacedRight && horizontalInput > 0.1f)
        {
            _isFacedRight = true;
            gameObject.transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            _groundDetector.SetRecoiled(true);
            _guiManager.LoseHealth();
            Debug.Log(Mathf.Sign(transform.position.x - enemy.transform.position.x));
            _rigidbody.AddForce(new Vector2(Mathf.Sign(transform.position.x - enemy.transform.position.x) * 4f, 3f), ForceMode2D.Impulse);

        }
    }
}

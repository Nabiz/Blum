using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 6.0f;
    [SerializeField] private float jumpForce = 8.0f;
    
    [SerializeField] private GUIManager guiManager;
    
    private Rigidbody2D rb;
    private Attack attack;

    public bool isGrounded = true;
    private bool isFacedRight = true;
    private bool isHitted = false;

    public Animator animator;

    private int health = 3;

    public void ResetStatuses()
    {
        rb.velocity = Vector2.zero;

        isHitted = false;
        isGrounded = true;

        animator.SetBool("IsGrounded", true);
        animator.SetBool("IsHitted", false);
        animator.SetBool("IsJumping", false);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attack = transform.Find("Attack").GetComponent<Attack>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isHitted && !attack.IsAttacking())
            Jump();

        if(Input.GetKeyDown(KeyCode.X) && isGrounded)
            Attack();
    }

    private void FixedUpdate()
    {
        CaptureHorizontalMovement();
    }

    private void Jump()
    {
        isGrounded = false;
        animator.SetBool("IsJumping", true);
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }
    private void Attack()
    {
        attack.PerformAttack();
        animator.SetTrigger("Attack");
    }

    private void CaptureHorizontalMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (!isHitted && !attack.IsAttacking())
        {
            float velocity = horizontalInput * Time.fixedDeltaTime * movementSpeed;
            rb.position += velocity * Vector2.right;
            animator.SetFloat("Speed", Mathf.Abs(velocity));
        }

        if (isFacedRight && horizontalInput < -0.1f && !attack.IsAttacking() && !isHitted)
        {
            isFacedRight = false;
            gameObject.transform.Rotate(0f, 180f, 0f);

        }
        else if (!isFacedRight && horizontalInput > 0.1f && !attack.IsAttacking() && !isHitted)
        {
            isFacedRight = true;
            gameObject.transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeHit(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack"))
        {
            TakeHit(collision.gameObject);
        }
    }

    private void TakeHit(GameObject colisionGameObject)
    {
        if (!isHitted)
        {
            --health;
            isHitted = true;
            guiManager.LoseHealth();
        }

        if (health == 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            animator.SetBool("IsHitted", true);
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(Mathf.Sign(transform.position.x - colisionGameObject.transform.position.x) * 5f, 2f), ForceMode2D.Impulse);
        }
    }

    IEnumerator Die()
    {
        animator.SetTrigger("IsDead");
        rb.Sleep();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

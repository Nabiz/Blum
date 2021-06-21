using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private bool _isGrounded = true;
    private bool _isRecoiled = true;
    PlayerController playerController;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }
    public void SetGrounded(bool value)
    {
        _isGrounded = value;
    }

    public bool IsRecoiled()
    {
        return _isRecoiled;
    }

    public void SetRecoiled(bool value)
    {
        _isRecoiled = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            playerController._isHitted = false;
            playerController._animator.SetBool("IsHitted", false);
            _isGrounded = true;
            _isRecoiled = false;
            GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponentInParent<Animator>().SetBool("IsJumping", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveForce = 5f;
    private Rigidbody2D rb;
    private Vector3 startPos;
    private Quaternion startRot;
    private bool isMoving = false;
    private bool hasTouchedGround = false;

    private GameManager gameManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        startRot = transform.rotation;

        gameManager = FindAnyObjectByType<GameManager>();

        // Set car không bị rơi xuống khi chưa bắt đầu
        rb.bodyType = RigidbodyType2D.Kinematic;

        
    }

    private void FixedUpdate()
    {
        if (isMoving && rb.bodyType == RigidbodyType2D.Dynamic && hasTouchedGround)
        {
            rb.AddForce(Vector2.right * moveForce);
        }
    }
    public void SetKinematic()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    /// <summary>
    /// SAU KHI NHẤN START, THÌ SẼ CÓ 1 GIÂY NỔ MÁY TRƯỚC KHI CHẠY
    /// </summary>
    public void StartMovement()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero; // Ngăn rơi lệch
        rb.angularVelocity = 0f;
        isMoving = false;
        hasTouchedGround = false;

        AudioManager.Instance.PlaySFX("start");

        StartCoroutine(StartCarAfterDelay(1f));
    }


    /// <summary>
    /// CHẠY XE
    /// </summary>
    private IEnumerator StartCarAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isMoving = true;

        AudioManager.Instance.PlaySFX("running");
    }

    public void StopMovement()
    {
        isMoving = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        AudioManager.Instance.StopSFX("running");
    }

    public void ResetPosition()
    {
        StopMovement();
        transform.position = startPos;
        transform.rotation = startRot;
        SetKinematic();
        hasTouchedGround = false;  
       
    }

    public void Boost(float boostForce, float duration)
    {
        StartCoroutine(BoostCoroutine(boostForce, duration));
    }

    private System.Collections.IEnumerator BoostCoroutine(float boostForce, float duration)
    {
        float originalForce = moveForce;
        moveForce += boostForce;
        yield return new WaitForSeconds(duration);
        moveForce = originalForce;
    }

    public void JumpPad(float jumpForce)
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Gắn tag cho đường vẽ là "Ground"
        if (collision.collider.CompareTag("Ground"))
        {
            hasTouchedGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("End"))
        {
            gameManager.HitEnd();
        }
    }
}

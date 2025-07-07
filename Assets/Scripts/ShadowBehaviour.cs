using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehaviour : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    private float jumpForce = 10;
    private Animator animator;
    private float moveSpeed = 6;
    private bool isGrounded = false;
    private float moveInput = 0f;
    private float shadowDuration = 10f; // 影子持续时间10秒

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // 启动自动销毁协程
        StartCoroutine(DestroyAfterTime());
    }

    // 协程：10秒后自动销毁shadow并通知player
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(shadowDuration);
        
        // 查找player并通知它回到正常状态
        GameObject player = GameObject.Find("Player");
        GameObject playerFake = GameObject.Find("PlayerFake");

        if (player != null)
        {
            PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null)
            {
                playerBehaviour.ReturnToPlayer();
            }
        }

        if (playerFake != null)
        {
            PlayerBehaviour playerBehaviour = playerFake.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null)
            {
                playerBehaviour.ReturnToPlayer();
            }
        }
        
        // 销毁shadow
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsWalking", true);
            transform.localScale = new Vector3(-3, 3, 1);
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsWalking", true);
            transform.localScale = new Vector3(3, 3, 1);
            moveInput = 1f;
        }
        else
        {
            animator.SetBool("IsWalking", false);
            moveInput = 0f;
        }
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Debug.Log("Jump");
            Jump();
        }
    }

    void MoveLeft()
    {
        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
    }

    void MoveRight()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    void Jump()
    {
        animator.SetBool("IsJumping", true);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                animator.SetBool("IsJumping", false);
                break;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = false;

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                animator.SetBool("IsJumping", false);
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tilemap")
        {
            isGrounded = false;
            animator.SetBool("IsJumping", true);
        }
    }
}

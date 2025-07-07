using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBehaviour : MonoBehaviour
{
    //输入是模拟的list，list代表每帧的各个按键状况, list对应这一帧的W A D E G是否按下
    public List<bool[]> simulatedInputs; // 每帧的输入，bool[0]=W, bool[1]=A, bool[2]=D, bool[3]=E, bool[4]=G
    private int currentFrame = 0;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveSpeed = 6f;
    private float jumpForce = 10f;
    private bool isGrounded = false;
    private bool lastWInput = false;
    private bool lastEInput = false;
    private bool lastGInput = false;
    private float echoDuration = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // simulatedInputs 需要在外部赋值
        StartCoroutine(DestroyAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (simulatedInputs == null || currentFrame >= simulatedInputs.Count) return;
        var input = simulatedInputs[currentFrame];

        // 移动
        float moveInput = 0f;
        if (input[1]) // A
        {
            moveInput = -1f;
            animator.SetBool("IsWalking", true);
            transform.localScale = new Vector3(-3, 3, 1);
        }
        else if (input[2]) // D
        {
            moveInput = 1f;
            animator.SetBool("IsWalking", true);
            transform.localScale = new Vector3(3, 3, 1);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 跳跃用input[0]模拟key(W), 只在keydown时触发
        if (input[0] && isGrounded)
        {
            animator.SetBool("IsJumping", true);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 使用交互物品（E键keydown）
        if (IsEKeyDown(input))
        {
           Interact();
        }

        if (IsGKeyDown(input))
        {
            Destroy(gameObject);
        }

        currentFrame++;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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
        isGrounded = false;
        animator.SetBool("IsJumping", true);
    }

    private bool IsNearBeacon()
    {
        // Implementation of IsNearBeacon method
        return false; // Placeholder return, actual implementation needed
    }

    private void SwitchShadow()
    {
        // Implementation of SwitchShadow method
    }

    // 检测W键keydown
    private bool IsWKeyDown(bool[] input)
    {
        bool result = input[0] && !lastWInput;
        lastWInput = input[0];
        return result;
    }

    // 检测E键keydown
    private bool IsEKeyDown(bool[] input)
    {
        bool result = input[3] && !lastEInput;
        lastEInput = input[3];
        return result;
    }

    private bool IsGKeyDown(bool[] input)
    {
        bool result = input[4] && !lastGInput;
        lastGInput = input[4];
        return result;
    }
    private void Interact()
    {
        //开关

    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(echoDuration);
        
        Destroy(gameObject);
    }
}

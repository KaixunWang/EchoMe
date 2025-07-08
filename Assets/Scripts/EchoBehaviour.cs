using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBehaviour : MonoBehaviour
{
    //输入是模拟的list，list代表每帧的各个按键状况, list对应这一帧的W A D E G是否按下
    public List<bool[]> simulatedInputs; // 每帧的输入，bool[0]=W, bool[1]=A, bool[2]=D, bool[3]=E, bool[4]=G
    public BeaconBehaviour beaconBehaviour;
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
        
    }
    void FixedUpdate(){
        isGrounded = CheckGrounded();
        if(isGrounded){
            animator.SetBool("IsJumping", false);
        }
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
        if (IsWKeyDown(input) && isGrounded)
        {
            animator.SetBool("IsJumping", true);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Echo jumped at position: " + transform.position);
            Debug.Log("Echo jump force applied: " + jumpForce);
        }

        // 使用交互物品（E键keydown）
        if (IsEKeyDown(input))
        {
           Interact();
        }

        if (IsGKeyDown(input))
        {
            beaconBehaviour.SetHasEcho(false);
            Destroy(gameObject);
        }

        currentFrame++;
    }

    public bool CheckGrounded(){
        //3raycast
        Collider2D col = GetComponent<Collider2D>();
        float colliderWidth = 0.8f;
        float colliderHeight = 0.5f;
        Vector3 basePos = col.bounds.center + Vector3.down * (colliderHeight / 2f - 0.01f);
        Vector3 left = basePos + Vector3.left * (colliderWidth / 2f - 0.05f);
        Vector3 center = basePos;
        Vector3 right = basePos + Vector3.right * (colliderWidth / 2f - 0.05f);
        float groundCheckDistance = 0.65f;
        int groundLayer = LayerMask.GetMask("Ground");
        return Physics2D.Raycast(left, Vector2.down, groundCheckDistance, groundLayer) ||
               Physics2D.Raycast(center, Vector2.down, groundCheckDistance, groundLayer) ||
               Physics2D.Raycast(right, Vector2.down, groundCheckDistance, groundLayer);
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
        beaconBehaviour.SetHasEcho(false);
        Destroy(gameObject);
    }
}

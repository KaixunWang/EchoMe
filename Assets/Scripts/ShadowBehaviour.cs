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
    private BeaconBehaviour beaconBehaviour = null;
    
    private int currentFrame = 0;
    private List<bool[]> input;
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

        if (player != null)
        {
            PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null)
            {
                playerBehaviour.ReturnToPlayer();
            }
        }
        beaconBehaviour.SwitchPlayer(player.GetComponent<PlayerBehaviour>().nearBeaconPosition);
        // 销毁shadow
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if(input == null){
            Debug.LogError("Input list is not set. Please set the input list from BeaconBehaviour.");
            return;
        }
        input.Add(new bool[5] { false, false, false, false, false }); // 初始化当前帧的输入状态
        // 获取当前帧的输入
        // input[currentFrame] 是一个 bool 数组，代表每帧的 W, A, D, E, G 键状态
        // W: input[currentFrame][0], A: input[currentFrame][1], D: input[currentFrame][2], E: input[currentFrame][3], G: input[currentFrame][4]
        var inputState = input[currentFrame];
        if (Input.GetKey(KeyCode.A))
        {
            inputState[1] = true; // A键按下
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputState[2] = true; // D键按下
        }
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
        
            inputState[0] = true; // W键按下
        }
        if(Input.GetKeyDown(KeyCode.G)){ //立刻销毁shadow
            inputState[4] = true; // G键按下
        }
        
        currentFrame++;
    }

    void Update()
    {
        isGrounded = CheckGrounded();
        if(isGrounded){
            animator.SetBool("IsJumping", false);
        }
        
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
        if(Input.GetKeyDown(KeyCode.G)){ //立刻销毁shadow
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
                if (playerBehaviour != null)
                {
                    playerBehaviour.ReturnToPlayer();
                }
            }
            beaconBehaviour.SwitchPlayer(player.GetComponent<PlayerBehaviour>().nearBeaconPosition);
            Debug.Log("Shadow destroyed by G key");
            Destroy(gameObject);
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

    public bool CheckGrounded(){
        //3raycast
        Collider2D col = GetComponent<Collider2D>();
        float colliderWidth = 0.8f;
        float colliderHeight = 0.5f;
        Vector3 basePos = col.bounds.center + Vector3.down * (colliderHeight / 2f - 0.01f);
        Vector3 left = basePos + Vector3.left * (colliderWidth / 2f - 0.05f);
        Vector3 center = basePos;
        Vector3 right = basePos + Vector3.right * (colliderWidth / 2f - 0.05f);
        float groundCheckDistance = 1.2f;
        int groundLayer = LayerMask.GetMask("Ground");
        return Physics2D.Raycast(left, Vector2.down, groundCheckDistance, groundLayer) ||
               Physics2D.Raycast(center, Vector2.down, groundCheckDistance, groundLayer) ||
               Physics2D.Raycast(right, Vector2.down, groundCheckDistance, groundLayer);
    }

    public void setBeaconBehaviour(BeaconBehaviour p)
    {
        beaconBehaviour = p;
        input=p.getInput();
        input.Clear();
    }
}

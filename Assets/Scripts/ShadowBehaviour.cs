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
    }

    void Update()
    {
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
            animator.SetBool("IsWalking", true);
            transform.localScale = new Vector3(-3, 3, 1);
            moveInput = -1f;
            inputState[1] = true; // A键按下
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsWalking", true);
            transform.localScale = new Vector3(3, 3, 1);
            moveInput = 1f;
            inputState[2] = true; // D键按下
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
            inputState[0] = true; // W键按下
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
            inputState[4] = true; // G键按下
            Debug.Log("Shadow destroyed by G key");
            Destroy(gameObject);
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
            Destroy(gameObject);
        }
        currentFrame++;
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
        if (collision.gameObject.name == "GroundMap")
        {
            isGrounded = false;
            animator.SetBool("IsJumping", true);
        }
    }

    public void setBeaconBehaviour(BeaconBehaviour p)
    {
        beaconBehaviour = p;
        input=p.getInput();
        input.Clear();
    }
}

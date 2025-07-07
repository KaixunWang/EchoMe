using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public bool isGrounded = false;
    [SerializeField]
    private Rigidbody2D rb;
    private float jumpForce = 10;
    private Animator animator;
    private float moveSpeed = 6;

    private float moveInput = 0f;
    private bool isNearBeacon = false;

    private bool isNearSwitch = false;
    private Cainos.PixelArtPlatformer_Dungeon.Switch switchObject = null;
    private BoxesBehavior boxes = null;

    public bool getState()
    {
        return animator.GetBool("IsShadow");
    }
    public Vector3 nearBeaconPosition;
    private BeaconBehaviour beaconBehaviour;

    void Start()
    {
        QualitySettings.vSyncCount = 0;           // 关闭 VSync
        Application.targetFrameRate = 60;         // 手动锁帧
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void SwitchShadow()
    {
        Debug.Log("Switching shadow");
        animator.SetBool("IsShadow", true);
        animator.SetBool("IsWalking", false);
        beaconBehaviour.SwitchShadow(nearBeaconPosition);
    }
    void FixedUpdate()
    {
        // 检查是否处于影子状态
        bool isShadow = animator.GetBool("IsShadow");

        if (!isShadow)
        {
            // 只有在非影子状态下才应用移动速度
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            // 影子状态下停止水平移动，但保持垂直速度（重力）
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Update()
    {
        // 检查是否处于影子状态，如果是则禁用移动
        bool isShadow = animator.GetBool("IsShadow");
        isGrounded = CheckGrounded();
        if(isGrounded){
            animator.SetBool("IsJumping", false);
        }
        if (!isShadow)
        {
            // 只有在非影子状态下才允许移动
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

            // 只有在非影子状态下才允许跳跃
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                Debug.Log("Jump");
                Jump();
            }
        }
        else
        {
            // 影子状态下停止所有移动
            animator.SetBool("IsWalking", false);
            moveInput = 0f;
        }

        // 切换影子的按键在任何状态下都可以使用
        if (Input.GetKeyDown(KeyCode.E) && isNearBeacon && !isShadow)
        {
            Debug.Log("E pressed");
            SwitchShadow();
        }


        if (Input.GetKeyDown(KeyCode.E) && isNearSwitch &&switchObject != null)
        {
            Debug.Log("E pressed near switch");
            switchObject.IsOn = !switchObject.IsOn; // 切换开关状态

        if(Input.GetKeyDown(KeyCode.R) && isNearBeacon && !isShadow && !beaconBehaviour.HasEcho()){
            SummonEcho();

        }
    }

    public void MoveLeft()
    {
        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
    }

    public void MoveRight()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    public void Jump()
    {
        animator.SetBool("IsJumping", true);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    public void Interact(){

    }
    public void SummonEcho(){
        beaconBehaviour.SwitchPlayer(nearBeaconPosition);
    }

    // 当shadow销毁时调用，让player回到正常状态
    public void ReturnToPlayer()
    {
        Debug.Log("Returning to player");

        // 重置动画状态
        animator.SetBool("IsShadow", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);

        // 重置移动输入
        moveInput = 0f;

        // 如果是player获取主摄像机并设置回player
        if (gameObject.name == "Player")
        {
            GameObject mainCamera = GameObject.Find("Main Camera");
            if (mainCamera != null)
            {
                CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
                if (cameraFollow != null)
                {
                    cameraFollow.target = transform;
                    Debug.Log("Camera target set back to player");
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.name == "Boxes")
        {
            boxes = collision.gameObject.GetComponent<BoxesBehavior>();
            Debug.Log("Collided with Boxes");
            boxes.SetSpeed(moveSpeed); // 设置盒子的移动速度
            if (collision.contacts[0].normal.x > 0)
            {
                Debug.Log("Collision on right side, pushing left");
                // 如果碰撞发生在右侧，向左推动
                boxes.PushLeft();
            }
            else if (collision.contacts[0].normal.x < 0)
            {
                Debug.Log("Collision on left side, pushing right");
                // 如果碰撞发生在左侧，向右推动
                boxes.PushRight();
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Beacon")
        {
            
            Debug.Log("Beacon Updated");
            isNearBeacon = true;
            nearBeaconPosition = other.gameObject.transform.position;
            beaconBehaviour = other.gameObject.GetComponent<BeaconBehaviour>();
        }
        
        if (other.gameObject.name == "Switch")
        {
            Debug.Log("Player is near Switch");
            isNearSwitch = true;
            switchObject = other.gameObject.GetComponent<Cainos.PixelArtPlatformer_Dungeon.Switch>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Beacon")
        {
            isNearBeacon = false;
            nearBeaconPosition = Vector3.zero;
        }
        
        if (other.gameObject.name == "Switch")
        {
            Debug.Log("Player is near Switch");
            isNearSwitch = false;
            if (switchObject != null)
            {
                switchObject = null; // 清除引用
            }
        }
    }

    // 新增：设置moveInput的public方法
    public void SetMoveInput(float input)
    {

        moveInput = input;
    }

    // 新增：获取isNearBeacon的public方法
    public bool IsNearBeacon()
    {
        return isNearBeacon;
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
        Debug.DrawRay(left, Vector2.down * groundCheckDistance, Color.red);
        Debug.DrawRay(center, Vector2.down * groundCheckDistance, Color.green);
        Debug.DrawRay(right, Vector2.down * groundCheckDistance, Color.blue);
        return Physics2D.Raycast(left, Vector2.down, groundCheckDistance, groundLayer) ||
               Physics2D.Raycast(center, Vector2.down, groundCheckDistance, groundLayer) ||
               Physics2D.Raycast(right, Vector2.down, groundCheckDistance, groundLayer);
    }
}

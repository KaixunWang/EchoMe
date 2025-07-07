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
    private Vector3 nearBeaconPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    void SwitchShadow()
    {
        Debug.Log("Switching shadow");
        animator.SetBool("IsShadow", true);
        animator.SetBool("IsWalking", false);

        
        // 尝试加载 Shadow prefab
        GameObject shadowPrefab = Resources.Load<GameObject>("Prefab/Shadow");
        if (shadowPrefab == null)
        {
            Debug.LogError("Failed to load Shadow prefab from Resources/Prefab/Shadow");
            return;
        }
        
        // 实例化 Shadow
        GameObject shadow = Instantiate(shadowPrefab, nearBeaconPosition, Quaternion.identity);
        if (shadow == null)
        {
            Debug.LogError("Failed to instantiate Shadow prefab");
            return;
        }
        
        Debug.Log("Shadow instantiated successfully at: " + shadow.transform.position);

        // 获取主摄像机并设置目标
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found in scene!");
            return;
        }
        
        CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
        if (cameraFollow == null)
        {
            Debug.LogError("CameraFollow component not found on Main Camera!");
            return;
        }

        mainCamera.transform.position = new Vector3(shadow.transform.position.x, shadow.transform.position.y, -10);
        cameraFollow.target = shadow.transform;
        
        Debug.Log("Camera target set to shadow");
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
        Debug.Log("Collision exit with " + collision.gameObject.name);
        if (collision.gameObject.name == "GroundMap")
        {
            isGrounded = false;
            animator.SetBool("IsJumping", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Beacon")
        {
            isNearBeacon = true;
            nearBeaconPosition = other.gameObject.transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Beacon")
        {
            isNearBeacon = false;
            nearBeaconPosition = Vector3.zero;
        }
    }
}

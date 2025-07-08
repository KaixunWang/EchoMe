using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class ShadowBehaviour : MonoBehaviour
// {
//     [SerializeField]
//     private Rigidbody2D rb;
//     private float jumpForce = 10;
//     private Animator animator;
//     private float moveSpeed = 6;
//     private bool isGrounded = false;
//     private float moveInput = 0f;
//     private float shadowDuration = 10f; // 影子持续时间10秒
//     private BeaconBehaviour beaconBehaviour = null;
    
//     private int currentFrame = 0;
//     private List<bool[]> input;
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         // 启动自动销毁协程
//         StartCoroutine(DestroyAfterTime());
//     }
    
//     // 协程：10秒后自动销毁shadow并通知player
//     IEnumerator DestroyAfterTime()
//     {
//         yield return new WaitForSeconds(shadowDuration);
        
//         // 查找player并通知它回到正常状态
//         GameObject player = GameObject.Find("Player");

//         if (player != null)
//         {
//             PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
//             if (playerBehaviour != null)
//             {
//                 playerBehaviour.ReturnToPlayer();
//             }
//         }
//         //beaconBehaviour.SwitchPlayer(player.GetComponent<PlayerBehaviour>().nearBeaconPosition);
//         // 销毁shadow
//         Destroy(gameObject);
//     }

//     void FixedUpdate()
//     {
//         rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
//         if(input == null){
//             Debug.LogError("Input list is not set. Please set the input list from BeaconBehaviour.");
//             return;
//         }
//         input.Add(new bool[5] { false, false, false, false, false }); // 初始化当前帧的输入状态
//         // 获取当前帧的输入
//         // input[currentFrame] 是一个 bool 数组，代表每帧的 W, A, D, E, G 键状态
//         // W: input[currentFrame][0], A: input[currentFrame][1], D: input[currentFrame][2], E: input[currentFrame][3], G: input[currentFrame][4]
//         var inputState = input[currentFrame];
        
//         // 记录所有按键状态，不使用else if避免冲突
//         if (Input.GetKey(KeyCode.A))
//         {
//             inputState[1] = true; // A键按下
//         } 
//         if (Input.GetKey(KeyCode.D))
//         {
//             inputState[2] = true; // D键按下
//         }
//         if (Input.GetKeyDown(KeyCode.W))
//         {
//             inputState[0] = true; // W键按下
//         }
//         if (Input.GetKeyDown(KeyCode.E))
//         {
//             inputState[3] = true; // E键按下
//         }
//         if(Input.GetKeyDown(KeyCode.G)){ //立刻销毁shadow
//             inputState[4] = true; // G键按下
//         }
        
//         currentFrame++;
//     }

//     void Update()
//     {
//         isGrounded = CheckGrounded();
//         if(isGrounded){
//             animator.SetBool("IsJumping", false);
//         }
        
//         if (Input.GetKey(KeyCode.A))
//         {
//             animator.SetBool("IsWalking", true);
//             transform.localScale = new Vector3(-3, 3, 1);
//             moveInput = -1f;
//         }
//         if (Input.GetKey(KeyCode.D))
//         {
//             animator.SetBool("IsWalking", true);
//             transform.localScale = new Vector3(3, 3, 1);
//             moveInput = 1f;
//         }
//         if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
//         {
//             animator.SetBool("IsWalking", false);
//             moveInput = 0f;
//         }
//         if (Input.GetKeyDown(KeyCode.W) && isGrounded)
//         {
//             Debug.Log("Jump");
//             Jump();
//         }
//         if(Input.GetKeyDown(KeyCode.G)){ //立刻销毁shadow
//             GameObject player = GameObject.Find("Player");
//             if (player != null)
//             {
//                 PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
//                 if (playerBehaviour != null)
//                 {
//                     playerBehaviour.ReturnToPlayer();
//                 }
//             }
//             //beaconBehaviour.SwitchPlayer(player.GetComponent<PlayerBehaviour>().nearBeaconPosition);
//             Debug.Log("Shadow destroyed by G key");
//             Destroy(gameObject);
//         }
        
//     }

//     void MoveLeft()
//     {
//         transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
//     }

//     void MoveRight()
//     {
//         transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
//     }

//     void Jump()
//     {
//         animator.SetBool("IsJumping", true);
//         rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//     }
//     public void Interact(){
        
//     }

//     public bool CheckGrounded(){
//         //3raycast
//         Collider2D col = GetComponent<Collider2D>();
//         float colliderWidth = 0.8f;
//         float colliderHeight = 0.5f;
//         Vector3 basePos = col.bounds.center + Vector3.down * (colliderHeight / 2f - 0.01f);
//         Vector3 left = basePos + Vector3.left * (colliderWidth / 2f - 0.05f);
//         Vector3 center = basePos;
//         Vector3 right = basePos + Vector3.right * (colliderWidth / 2f - 0.05f);
//         float groundCheckDistance = 0.65f;
//         int groundLayer = LayerMask.GetMask("Ground");
//         return Physics2D.Raycast(left, Vector2.down, groundCheckDistance, groundLayer) ||
//                Physics2D.Raycast(center, Vector2.down, groundCheckDistance, groundLayer) ||
//                Physics2D.Raycast(right, Vector2.down, groundCheckDistance, groundLayer);
//     }

//     public void setBeaconBehaviour(BeaconBehaviour p)
//     {
//         beaconBehaviour = p;
//         input=p.getInput();
//         input.Clear();
//     }
// }
public class ShadowBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float shadowDuration = 10f;
    
    private bool isGrounded = false;
    private float moveInput = 0f;
    private BeaconBehaviour beaconBehaviour = null;

    private bool isNearSwitch = false;
    private Cainos.PixelArtPlatformer_Dungeon.Switch switchObject = null;
    private BoxesBehavior boxes = null;
    
    // 基于时间的输入记录
    private List<TimeBasedInputEvent> inputEvents;
    private float recordStartTime;
    private Dictionary<InputType, bool> currentInputStates;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // 初始化输入状态字典
        currentInputStates = new Dictionary<InputType, bool>
        {
            {InputType.W, false},
            {InputType.A, false},
            {InputType.D, false},
            {InputType.E, false},
            {InputType.G, false}
        };
        
        recordStartTime = Time.time;
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
        //beaconBehaviour.SwitchPlayer(player.GetComponent<PlayerBehaviour>().nearBeaconPosition);
        // 销毁shadow
        Destroy(gameObject);
    }
    void Update()
    {
        isGrounded = CheckGrounded();
        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
        
        // 记录输入事件（基于时间）
        RecordInputEvents();
        
        // 处理实际输入
        HandleInput();
    }
    
    void FixedUpdate()
    {
        // 使用固定的物理更新
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
    
    void RecordInputEvents()
    {
        if (inputEvents == null)
        {
            Debug.LogError("Input events list is not set. Please set it from BeaconBehaviour.");
            return;
        }
        
        float currentTime = Time.time - recordStartTime;
        Vector2 currentPosition = transform.position;
        
        // 检查每个输入键的状态变化
        CheckInputStateChange(KeyCode.W, InputType.W, currentTime, currentPosition);
        CheckInputStateChange(KeyCode.A, InputType.A, currentTime, currentPosition);
        CheckInputStateChange(KeyCode.D, InputType.D, currentTime, currentPosition);
        CheckInputStateChange(KeyCode.E, InputType.E, currentTime, currentPosition);
        CheckInputStateChange(KeyCode.G, InputType.G, currentTime, currentPosition);
    }
    
    void CheckInputStateChange(KeyCode keyCode, InputType inputType, float currentTime, Vector2 position)
    {
        bool currentState = Input.GetKey(keyCode);
        bool previousState = currentInputStates[inputType];
        
        // 检测状态变化
        if (currentState != previousState)
        {
            inputEvents.Add(new TimeBasedInputEvent(currentTime, inputType, currentState, position));
            currentInputStates[inputType] = currentState;
        }
    }
    
    void HandleInput()
    {
        // 移动输入
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
        
        // 跳跃
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            DestroyImmediate();
        }
        if (Input.GetKeyDown(KeyCode.E) && isNearSwitch && switchObject != null)
        {
            Debug.Log("E pressed near switch");
            switchObject.IsOn = !switchObject.IsOn; // 切换开关状态
        }
        
        // 立即销毁
        if (Input.GetKeyDown(KeyCode.G))
        {
            DestroyImmediate();
        }
    }
    
    void Jump()
    {
        animator.SetBool("IsJumping", true);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    void DestroyImmediate()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null)
            {
                playerBehaviour.ReturnToPlayer();
            }
        }
        
        Debug.Log("Shadow destroyed by G key");
        Destroy(gameObject);
    }
    
    public bool CheckGrounded()
    {
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
    
    public void setBeaconBehaviour(BeaconBehaviour beacon)
    {
        beaconBehaviour = beacon;
        inputEvents = beacon.getInput();
        inputEvents.Clear();
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
        if (other.gameObject.name == "Switch")
        {
            Debug.Log("Player is near Switch");
            isNearSwitch = true;
            switchObject = other.gameObject.GetComponent<Cainos.PixelArtPlatformer_Dungeon.Switch>();
        }
        if (other.gameObject.name == "Board")
        {
            Debug.Log("Player is near Board");
            BoardBehavior board = other.gameObject.GetComponent<BoardBehavior>();
            board.IsOpened = true; // 切换门的开关状态
            if (board != null)
            {
                board.TriggerDoor(); // 触发门的开关
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Switch")
        {
            Debug.Log("Player is out Switch");
            isNearSwitch = false;
            if (switchObject != null)
            {
                switchObject = null; // 清除引用
            }
        }
        if (other.gameObject.name == "Board")
        {
            Debug.Log("Player is near Board");
            BoardBehavior board = other.gameObject.GetComponent<BoardBehavior>();
            board.IsOpened = false; // 切换门的开关状态
            if (board != null)
            {
                board.TriggerDoor(); // 触发门的开关
            }
        }
    }

}

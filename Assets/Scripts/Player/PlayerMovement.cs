using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats_SO stats;
    private PlayerEnergy playerEnergy;
    private Rigidbody rb;
    public SpriteRenderer sr;

    private PlayerActions actions; // Input system
    public Vector2 moveDirection;
    private bool isRunning = false;
    private bool isWalking = false; 
    [SerializeField] private PlayerSpineNode spineNode; 
    public Vector2 lastMoveDirection;

    private string currentAnimation = ""; // 当前动画状态

    private void Awake()
    {
        actions = new PlayerActions();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (spineNode == null)
        {
            Debug.LogError("SpineNode is not assigned!");
        }
        playerEnergy = GetComponent<PlayerEnergy>();
        spineNode.StartAni();
    }

    void Update()
    {
        ReadMovement();
        CheckMovementState();  // 检查玩家当前的运动状态（走路或奔跑）
        UpdateAnimation();     // 更新动画
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveSpeed = isRunning ? stats.runSpeed : stats.speed;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.y * moveSpeed); 

        // 奔跑状态
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    // 读取输入
    private void ReadMovement()
    {
        moveDirection = actions.Movement.Move.ReadValue<Vector2>().normalized; 
    }

    // 检查运动状态并更新能量消耗状态
    private void CheckMovementState()
    {
        if (moveDirection != Vector2.zero)
        {
            isWalking = !isRunning;
            playerEnergy.SetRunning(isRunning);
            playerEnergy.SetWalking(isWalking);
        }
        else
        {
            isWalking = false;
            isRunning = false;
            playerEnergy.SetWalking(false);
            playerEnergy.SetRunning(false);
        }
    }

    // 更新动画
    private void UpdateAnimation()
    {
        string animationName = "";

        if (moveDirection != Vector2.zero)
        {
            lastMoveDirection = moveDirection; 

            // 根据角色的移动方向设置动画
            if (moveDirection.x > 0) // 向右
            {
                animationName = isRunning ? PlayerSpineAniName.RunR : PlayerSpineAniName.WalkR;
            }
            else if (moveDirection.x < 0) // 向左
            {
                animationName = isRunning ? PlayerSpineAniName.RunL : PlayerSpineAniName.WalkL;
            }
            else if (moveDirection.y > 0) // 向上
            {
                animationName = isRunning ? PlayerSpineAniName.RunU : PlayerSpineAniName.WalkU;
            }
            else if (moveDirection.y < 0) // 向下
            {
                animationName = isRunning ? PlayerSpineAniName.RunD : PlayerSpineAniName.WalkD;
            }
        }
        else
        {
            // Idle动画，判断朝向
            if (lastMoveDirection.x > 0) animationName = PlayerSpineAniName.IdleR;
            else if (lastMoveDirection.x < 0) animationName = PlayerSpineAniName.IdleL;
            else if (lastMoveDirection.y > 0) animationName = PlayerSpineAniName.IdleU;
            else animationName = PlayerSpineAniName.IdleD;
        }

        // 播放动画，如果状态发生变化
        if (!string.IsNullOrEmpty(animationName) && animationName != currentAnimation)
        {
            spineNode.PlayAnime(animationName, true);
            currentAnimation = animationName; // 更新当前动画状态
        }
    }

    // 启用和禁用移动输入
    public void DisableMovementInput()
    {
        actions.Movement.Disable();
    }

    public void EnableMovementInput()
    {
        actions.Movement.Enable();
    }
    
    // Unity methods
    private void OnEnable()
    {
        actions.Enable();
        spineNode.StartAni();
    }

    private void OnDisable()
    {
        actions.Disable();
        spineNode.StopAni();
    }
}

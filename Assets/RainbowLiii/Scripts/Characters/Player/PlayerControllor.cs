using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    Idle,
    Move,
    Stop,
    Jump,
    Fall,
    WallSlide,
}
public class PlayerControllor : MonoBehaviour,IStateMachineOwner
{
    public Character character;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PlayerInput input;
    private StateMachine stateMachine;
    [HideInInspector] public Rigidbody2D rb;
    public float currentMoveSpeed = 0f;
    public float currentJumpSpeed = 0f;
    public PlayerState currentState;
    [Header("地面检测")]
    public Transform groundCheck;
    public float groundCheckLength;
    public float groundCheckHeight;
    public LayerMask groundLayer;
    [Header("墙壁检测")]
    public Transform wallCheck_R;
    public float wallCheckLength;
    public float wallCheckHeight;
    public LayerMask wall;
    [HideInInspector] public bool wallSign;
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.Init(this);
        input = new PlayerInput();
        input.Enable();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ChangeState(PlayerState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(IsWall());
    }
    void FixedUpdate()
    {
        
    }
    public void ChangeState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                stateMachine.ChangeState<Player_Idle>();
                break;
            case PlayerState.Move:
                stateMachine.ChangeState<Player_Move>();
                break;
            case PlayerState.Stop:
                stateMachine.ChangeState<Player_Stop>();
                break;
            case PlayerState.Jump:
                stateMachine.ChangeState<Player_Jump>();
                break;
            case PlayerState.Fall:
                stateMachine.ChangeState<Player_Fall>();
                break;
            case PlayerState.WallSlide:
                stateMachine.ChangeState<Player_WallSlide>();
                break;
        }
    }
    public void PlayAnimation(string animationName)
    {
        anim.CrossFadeInFixedTime(animationName, 0f);
    }
    public bool IsGrounded()
    {
        Collider2D[] colliders = new Collider2D[4];
        //int count = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundCheckRadius, colliders, groundLayer);
        int count = Physics2D.OverlapBoxNonAlloc(groundCheck.position, new Vector2(groundCheckLength, groundCheckHeight), 0f, colliders, groundLayer);
        if (count > 0)
        {
            return true;
        }
        return false;
    }
    public bool IsWall()
    {
        Collider2D[] colliders = new Collider2D[4];
        int count = Physics2D.OverlapBoxNonAlloc(wallCheck_R.position, new Vector2(wallCheckLength, wallCheckHeight), 0f, colliders, wall);
        if (count > 0)
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckLength, groundCheckHeight));
        Gizmos.DrawWireCube(wallCheck_R.position, new Vector3(wallCheckLength, wallCheckHeight));
    }
}

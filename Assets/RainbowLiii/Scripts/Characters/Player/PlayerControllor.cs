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
    Dash,
    Attack,
}
public class PlayerControllor : MonoBehaviour,IStateMachineOwner,IDamage
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
    [HideInInspector] public bool wallCool;
    [Header("攻击组合")]
    public SkillConfigTable katanaConfig;
    public SkillConfigTable punchConfig;
    [Header("当前技能")]
    public SkillConfigTable currenctSkill;
    [HideInInspector] public SkillConfig currentSkillConfig;
    [HideInInspector] public bool canSwitchSkill;
    public Transform hitBox;
    public float currentHitBoxLength;
    public float currentHitBoxHeight;
    private bool showHitBox;
    public LayerMask enemyLayer;
    [HideInInspector] public string currenthitName;
    [HideInInspector] public float currentknockback;
    [HideInInspector] public float currentupward;
    [Header("顿帧时间")]
    public float frameTime;
    // Start is called before the first frame update
    void Awake()
    {
        stateMachine = new StateMachine();
        stateMachine.Init(this);
        input = new PlayerInput();
        input.Enable();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        wallCool = true;
        currenctSkill = katanaConfig;
        showHitBox = false;
        ChangeState(PlayerState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(IsWall());
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currenctSkill = katanaConfig;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currenctSkill = punchConfig;
        }
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
            case PlayerState.Dash:
                stateMachine.ChangeState<Player_Dash>();
                break;
            case PlayerState.Attack:
                stateMachine.ChangeState<Player_NormalAttack>();
                break;
        }
    }
    public virtual void StartAttack(SkillConfig skillConfig)
    {
        canSwitchSkill = false;
        currentSkillConfig = skillConfig;
        PlayAnimation(currentSkillConfig.animationName);
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
    public void AttackEvent()
    {
        showHitBox = true;
        Collider2D[] colliders = new Collider2D[10];
        int count = 0;
        if (transform.localScale.x > 0)
        {
            count = Physics2D.OverlapBoxNonAlloc(transform.position + hitBox.position, new Vector2(currentHitBoxLength, currentHitBoxHeight), 0f, colliders, enemyLayer);
        }
        else
        {
            count = Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x - hitBox.position.x, transform.position.y + hitBox.position.y), new Vector2(currentHitBoxLength, currentHitBoxHeight), 0f, colliders, enemyLayer);
        }
        if (count > 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i]!= null && colliders[i].TryGetComponent(out IDamage damage))
                {
                    StartCoroutine(AnimationFreezing(frameTime));
                    damage.TakeDamage(0,currenthitName,currentknockback,currentupward,gameObject);
                }
            }
        }
        
    }
    public void WallSlideCool()
    {
        StartCoroutine(WallSlideCoolTime());
    }
    private IEnumerator AnimationFreezing(float time)
    {
        anim.speed = 0f;
        yield return new WaitForSeconds(time);
        anim.speed = 1f;
    }
    private IEnumerator WallSlideCoolTime()
    {
        wallCool = false;
        yield return new WaitForSeconds(character.wallCoolTime);
        wallCool = true;
    }
    public void CanSwitchSkill()
    {
        canSwitchSkill = true;
        showHitBox = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckLength, groundCheckHeight));
        Gizmos.DrawWireCube(wallCheck_R.position, new Vector3(wallCheckLength, wallCheckHeight));
        Gizmos.color = Color.red;
        if (showHitBox)
        {
            if (transform.localScale.x > 0)
            {
                Gizmos.DrawWireCube(transform.position + hitBox.position, new Vector3(currentHitBoxLength, currentHitBoxHeight));
            }
            else
            {
                Gizmos.DrawWireCube(new Vector2(transform.position.x - hitBox.position.x, transform.position.y + hitBox.position.y), new Vector3(currentHitBoxLength, currentHitBoxHeight));
            }
        }  
    }

    public void TakeDamage(float damage, string hitAnim, float knockback, float upward)
    {
        
    }

    public void TakeDamage(float damage, string hitAnim, float knockback, float upward, GameObject attacker)
    {
        
    }
}

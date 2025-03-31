using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
/// <summary>
/// 待机状态
/// </summary>
#region 待机状态
public class Player_Idle : PlayerStateBase
{
    public override void Enter()
    {
        player.currentMoveSpeed = 0f;
        player.currentState = PlayerState.Idle;
        player.PlayAnimation("Idle");
    }
    public override void Update()
    {
        if (player.input.Input.LAttack.triggered)
        {
            player.ChangeState(PlayerState.Attack);
        }
        if (player.input.Input.Jump.triggered && player.IsGrounded())
        {
            player.ChangeState(PlayerState.Jump);
        }
        if (!player.IsGrounded())
        {
            player.ChangeState(PlayerState.Fall);
        }
        if (player.input.Input.Dash.triggered)
        {
            player.ChangeState(PlayerState.Dash);
        }
        if (player.input.Input.Move.ReadValue<Vector2>().magnitude >= 0.5f)
        {
            player.ChangeState(PlayerState.Move);
        }
    }
    public override void FixedUpdate()
    {
        
    }
    public override void Exit()
    {

    }
}
#endregion
/// <summary>
/// 移动状态
/// </summary>
#region 移动状态
public class Player_Move : PlayerStateBase
{
    public override void Enter()
    {
        player.currentState = PlayerState.Move;
        player.PlayAnimation("Move");
    }
    public override void Update()
    {
        player.transform.Translate(player.character.runSpeed* new Vector2(player.transform.localScale.x,0) * Time.deltaTime);
        player.currentMoveSpeed = player.character.runSpeed;
        //player.rb.velocity = new Vector2(player.character.runSpeed * player.transform.localScale.x, 0);
        if (player.input.Input.LAttack.triggered)
        {
            player.ChangeState(PlayerState.Attack);
        }
        if (player.input.Input.Jump.triggered && player.IsGrounded())
        {
            player.ChangeState(PlayerState.Jump);
        }
        if (!player.IsGrounded())
        {
            player.ChangeState(PlayerState.Fall);
        }
        if (player.input.Input.Dash.triggered)
        {
            player.ChangeState(PlayerState.Dash);
        }
        if (player.input.Input.Move.ReadValue<Vector2>().x < -0.01f)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(player.input.Input.Move.ReadValue<Vector2>().x > 0.01f)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            player.ChangeState(PlayerState.Stop);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        
    }
}
#endregion
/// <summary>
/// 急停状态
/// </summary>
#region 急停状态
public class Player_Stop : PlayerStateBase
{
    public override void Enter()
    {
        player.currentState = PlayerState.Stop;
        player.PlayAnimation("Stop");
    }
    public override void Update()
    {
        if(CheckAnimationName("Stop",out float currentTime) && currentTime >= 1f)
        {
            player.ChangeState(PlayerState.Idle);
        }
        if (player.input.Input.LAttack.triggered)
        {
            player.ChangeState(PlayerState.Attack);
        }
        if (player.input.Input.Jump.triggered && player.IsGrounded())
        {
            player.ChangeState(PlayerState.Jump);
        }
        if (player.input.Input.Dash.triggered)
        {
            player.ChangeState(PlayerState.Dash);
        }
        if (player.input.Input.Move.ReadValue<Vector2>().magnitude >= 0.5f)
        {
            player.ChangeState(PlayerState.Move);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {

    }
}
#endregion
/// <summary>
/// 跳跃状态
/// </summary>
#region 跳跃状态
public class Player_Jump : PlayerStateBase
{
    public override void Enter()
    {
        player.currentState = PlayerState.Jump;
        player.PlayAnimation("Jump");
        player.currentJumpSpeed = player.character.jumpSpeed;
    }
    public override void Update()
    {
        player.transform.Translate(new Vector2(player.transform.localScale.x * player.currentMoveSpeed, player.currentJumpSpeed) * Time.deltaTime);
        player.currentJumpSpeed -= player.character.gravity * Time.deltaTime;
        if (player.input.Input.Move.ReadValue<Vector2>().magnitude >= 0.5f)
        {
            if (player.input.Input.Move.ReadValue<Vector2>().x < -0.01f)
            {
                player.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (player.input.Input.Move.ReadValue<Vector2>().x > 0.01f)
            {
                player.transform.localScale = new Vector3(1, 1, 1);

            }
        }
        if (player.input.Input.Dash.triggered)
        {
            player.ChangeState(PlayerState.Dash);
        }
        if (player.IsWall()&&!player.wallSign&&player.wallCool)
        {
            player.ChangeState(PlayerState.WallSlide);
        }
        if (player.currentJumpSpeed <= 0f)
        {
            player.ChangeState(PlayerState.Fall);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        
    }
}
#endregion
/// <summary>
/// 降落状态
/// </summary>
#region 降落状态
public class Player_Fall : PlayerStateBase
{
    private enum FallState { Fall, Land }
    private FallState fallState;
    private FallState Fallstate
    {
        get => fallState;
        set
        {
            fallState = value;
            switch (fallState)
            {
                case FallState.Fall:
                    player.PlayAnimation("JumpFall");
                    break;
                case FallState.Land:
                    player.PlayAnimation("Land");
                    break;
            }
        }
    }
    
    public override void Enter()
    {
        player.currentState = PlayerState.Fall;
        
        Fallstate = FallState.Fall;
    }
    public override void Update()
    {
        switch (fallState)
        {
            case FallState.Fall:
                FallUpdate();
                break;
            case FallState.Land:
                LandUpdate();
                break;
        }
        
    }
    public void FallUpdate()
    {
        player.transform.Translate(new Vector2(player.currentMoveSpeed* player.transform.localScale.x, player.currentJumpSpeed) * Time.deltaTime);
        player.currentJumpSpeed -= player.character.gravity * Time.deltaTime;
        if (player.IsWall() && player.wallSign && player.wallCool)
        {
            player.ChangeState(PlayerState.WallSlide);
        }
        if (player.IsGrounded())
        {
            Fallstate = FallState.Land;
            player.currentJumpSpeed = 0f;
        }
    }
    public void LandUpdate()
    {
        if(CheckAnimationName("Land",out float currentTime) && currentTime >= 1f)
        {
            player.ChangeState(PlayerState.Idle);
        }
        if (player.input.Input.Jump.triggered && player.IsGrounded())
        {
            player.ChangeState(PlayerState.Jump);
        }
        if (player.input.Input.Move.ReadValue<Vector2>().magnitude >= 0.5f)
        {
            player.ChangeState(PlayerState.Move);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        player.wallSign = false;
    }
}
#endregion
/// <summary>
/// 攀墙状态
/// </summary>
#region 攀墙状态
public class Player_WallSlide : PlayerStateBase
{
    public override void Enter()
    {
        player.WallSlideCool();
        player.currentState = PlayerState.WallSlide;
        player.currentJumpSpeed = player.character.jumpSpeed;
        player.wallSign = true;
        player.PlayAnimation("WallSlide");
    }
    public override void Update()
    {
        player.transform.Translate(new Vector2(0, -player.character.fallSpeed) * Time.deltaTime);
        if (player.input.Input.Jump.triggered)
        {
            player.transform.localScale = new Vector3(-player.transform.localScale.x, 1, 1);
            player.ChangeState(PlayerState.Jump);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {

    }
}
#endregion
/// <summary>
/// 冲刺状态
/// </summary>
#region 冲刺状态
public class Player_Dash : PlayerStateBase
{
    private float tempTime;
    public override void Enter()
    {
        tempTime = 0f;
        player.currentState = PlayerState.Dash;
        player.PlayAnimation("Dash");
    }
    public override void Update()
    {
        player.transform.Translate(new Vector2(player.character.dashSpeed * player.transform.localScale.x,0) * tempTime);
        if (tempTime < player.character.dashTime)
        {
            tempTime += Time.deltaTime;
        }
        else
        {
            player.ChangeState(PlayerState.Idle);
        }
        if (player.input.Input.LAttack.triggered)
        {
            player.ChangeState(PlayerState.Attack);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        tempTime = 0f;
    }
}
#endregion
/// <summary>
/// 普攻状态
/// </summary>
#region 普攻状态
public class Player_NormalAttack : PlayerStateBase
{
    private int normalSkillsIndex = 0;
    private int NormalSkillsIndex
    {
        get => normalSkillsIndex;
        set
        {
            if (value >= player.currenctSkill.skillConfigs.Length)
            {
                normalSkillsIndex = 0;
            }
            else normalSkillsIndex = value;
        }
    }
    public override void Enter()
    {
        player.currentState = PlayerState.Attack;
        NormalSkillsIndex = 0;
        StartAttack();
    }
    public void StartAttack()
    {
        Turning();
        player.currentHitBoxHeight = player.currenctSkill.skillConfigs[NormalSkillsIndex].heigth;
        player.currentHitBoxLength = player.currenctSkill.skillConfigs[NormalSkillsIndex].length;
        player.hitBox.position = player.currenctSkill.skillConfigs[NormalSkillsIndex].hitboxPos;
        player.StartAttack(player.currenctSkill.skillConfigs[NormalSkillsIndex]);
    }
    public override void Update()
    {
        if(player.input.Input.LAttack.triggered && player.canSwitchSkill)
        {
            player.currentHitBoxHeight = 0;
            player.currentHitBoxLength = 0;
            player.canSwitchSkill = false;
            NormalSkillsIndex++;
            StartAttack();
        }
        if(CheckAnimationName(player.currenctSkill.skillConfigs[NormalSkillsIndex].animationName,out float currentTime)&&currentTime >= 1)
        {
            player.ChangeState(PlayerState.Idle);
        }
        player.transform.Translate(new Vector2(player.currenctSkill.skillConfigs[NormalSkillsIndex].attackMove * player.transform.localScale.x, 0) * Time.deltaTime);
    }
    public void Turning()
    {
        if (player.input.Input.Move.ReadValue<Vector2>().x < -0.01f)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.input.Input.Move.ReadValue<Vector2>().x > 0.01f)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        
    }
}
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 待机状态
/// </summary>
#region 待机状态
public class Player_Idle : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("Idle");
    }
    public override void Update()
    {
        if(player.input.Input.Move.ReadValue<Vector2>().magnitude >= 0.5f)
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
        player.PlayAnimation("Move");
    }
    public override void Update()
    {
        player.transform.Translate(player.character.runSpeed* new Vector2(player.transform.localScale.x,0) * Time.deltaTime);
        //player.rb.velocity = new Vector2(player.character.runSpeed * player.transform.localScale.x, 0);
        if(player.input.Input.Move.ReadValue<Vector2>().x < -0.01f)
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
        player.PlayAnimation("Stop");
    }
    public override void Update()
    {
        if(CheckAnimationName("Stop",out float currentTime) && currentTime >= 1f)
        {
            player.ChangeState(PlayerState.Idle);
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

    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {

    }
}
#endregion
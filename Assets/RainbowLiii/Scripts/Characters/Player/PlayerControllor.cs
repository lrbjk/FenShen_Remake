using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    Idle,
    Move,
    Stop,
    Jump,
}
public class PlayerControllor : MonoBehaviour,IStateMachineOwner
{
    public Character character;
    public Animator anim;
    public PlayerInput input;
    private StateMachine stateMachine;
    public Rigidbody2D rb;
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
        Debug.Log(rb.velocity);
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
        }
    }
    public void PlayAnimation(string animationName)
    {
        anim.CrossFadeInFixedTime(animationName, 0f);
    }
}

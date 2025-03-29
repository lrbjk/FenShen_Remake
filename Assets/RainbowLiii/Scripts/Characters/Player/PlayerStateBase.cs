using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : StateBase
{
    protected PlayerControllor player;
    public override void Init(IStateMachineOwner owner)
    {
        base.Init(owner);
        player = (PlayerControllor)owner;
    }
    protected virtual bool CheckAnimationName(string stateName, out float currentTime)
    {
        AnimatorStateInfo info = player.anim.GetCurrentAnimatorStateInfo(0);
        currentTime = info.normalizedTime;
        return info.IsName(stateName);
    }
}

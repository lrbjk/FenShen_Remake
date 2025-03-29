using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    [Header("血量")]
    public int hp;
    [Header("防御")]
    public int defence;
    [Header("能量")]
    public int enegry;
    [Header("重力")]
    public float gravity;
    [Header("速度")]
    public int speed;
    [Header("奔跑速度")]
    public int runSpeed;
    [Header("跳跃速度")]
    public int jumpSpeed;
    [Header("冲刺速度")]
    public int dashSpeed;
    [Header("攻击力")]
    public int attack;
    [Header("暴击率")]
    public int critical;
}

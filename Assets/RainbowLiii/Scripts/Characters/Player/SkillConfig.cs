using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[CreateAssetMenu(fileName = "New SkillConfig", menuName = "SkillConfig/New SkillConfig")]
public class SkillConfig : ScriptableObject
{
    [Header("动画名")]
    public string animationName;
    [Header("碰撞箱设置")]
    public Vector2 hitboxPos;
    public float length;
    public float heigth;
    [Header("攻击位移")]
    public float attackMove;
    [Header("动作值")]
    public float damageRaito;
    [Header("击中动画")]
    public string hitName;
    [Header("音效")]
    public string audioName;
}

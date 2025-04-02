using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForTest : MonoBehaviour,IDamage
{
    private Animator anim;
    private float currentKnockback;
    private float currentUpward;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckAnimationName("Hurt",out float currentTime)&&currentTime <= 1f)
        {
            transform.Translate(new Vector2(-transform.localScale.x * currentKnockback, currentUpward) * Time.deltaTime);
        }
    }
    protected virtual bool CheckAnimationName(string stateName, out float currentTime)
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        currentTime = info.normalizedTime;
        return info.IsName(stateName);
    }
    public void TakeDamage(float damage, string hitAnim, float knockback, float upward)
    {
        anim.Play(hitAnim);
        currentKnockback = knockback;
        currentUpward = upward;
    }

    public void TakeDamage(float damage, string hitAnim, float knockback, float upward, GameObject attacker)
    {
        TakeDamage(damage, hitAnim, knockback, upward);
        transform.localScale = new Vector3(-attacker.transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}

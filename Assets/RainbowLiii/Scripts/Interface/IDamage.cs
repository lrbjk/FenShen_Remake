using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
   public void TakeDamage(float damage,string hitAnim,float knockback,float upward);
   public void TakeDamage(float damage,string hitAnim,float knockback,float upward,GameObject attacker);
}

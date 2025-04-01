using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForTest : MonoBehaviour,IDamage
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage, string hitAnim)
    {
        anim.Play(hitAnim);
    }
}

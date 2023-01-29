using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCombat : MonoBehaviour
{
    private Animator anim;
    private InputHandler input;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float LastClickedTime = 0;
    float maxComboDelay = 0.5f;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        input = GetComponent<InputHandler>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f
                && anim.GetCurrentAnimatorStateInfo(1).IsName("Attack 1"))
        {
            anim.SetBool("attack1", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f
                && anim.GetCurrentAnimatorStateInfo(1).IsName("Attack 2"))
        {
            anim.SetBool("attack2", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f
                && anim.GetCurrentAnimatorStateInfo(1).IsName("Attack 3"))
        {
            anim.SetBool("attack3", false);
            noOfClicks = 0;
        }

        if (Time.time - LastClickedTime > maxComboDelay && anim.GetCurrentAnimatorStateInfo(1).IsName("Empty (Not Dodging)"))
        {
            //Debug.Log("Time.time - LastClickedTime is greater than the maxComboDelay");
            noOfClicks = 0;
        }

        if (noOfClicks == 0)
        {
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", false);
        }


        if (Time.time > nextFireTime)
        {

            if (input.swordKey)
            {
                LastClickedTime = Time.time;
                noOfClicks++;
                if (noOfClicks == 1)
                {
                    anim.SetBool("attack1", true);
                }
                noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

                if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f
                    && anim.GetCurrentAnimatorStateInfo(1).IsName("Attack 1"))
                {
                    anim.SetBool("attack1", false);
                    anim.SetBool("attack2", true);
                }

                if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f
                    && anim.GetCurrentAnimatorStateInfo(1).IsName("Attack 2"))
                {
                    anim.SetBool("attack2", false);
                    anim.SetBool("attack3", true);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSwordCombat : MonoBehaviour
{
    private Animator anim;
    private InputHandler input;

    [Header("UI Elements")]
    public GameObject comboBarObj;
    public GameObject comboBarBreakLeft;
    public GameObject comboBarBreakRight;
    public GameObject comboBarTickObj;

    //public bool canAttack;
    //public bool comboAdvance;
    //public bool comboEnded;

    [Header("Combat Settings")]
    [Range(0f, .99f)] public float comboAdvancementWindowStart = .5f;
    [Range(0f, .99f)] public float comboAdvancementWindowEnd = .99f;
    public float cooldownTime = 1f;
    float tempCooldownTime;

    // Animator Bools:
    // canAttack, isAttcking, comboAdvance, comboBreak, endCombo playerClick

    // Animator Floats:
    // curAnimTime;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        input = GetComponent<InputHandler>();

        anim.SetBool("canAttack", true);
        anim.SetFloat("cooldown", cooldownTime);

        if (comboAdvancementWindowStart > comboAdvancementWindowEnd)
        {
            anim.SetBool("canAttack", false);
            Debug.Log("Fuckin dummy, you filthy rat. You thought you could get away with it, didn't you?" +
                "You really thought I wouldn't put a check in that makes sure the start is lower than the end" +
                "of the advancement window. Fuck you + ligma");
        }
    }

    

    private void Update()
    {
        if (input.swordKey)
        {
            anim.SetBool("playerClick", true);
        }
        else
        {
            anim.SetBool("playerClick", false);
        }

        if (anim.GetBool("isAttacking"))
        {
            DoComboUI();

            anim.SetFloat("curAnimTime", anim.GetCurrentAnimatorStateInfo(1).normalizedTime - anim.GetAnimatorTransitionInfo(1).duration);
            
            tempCooldownTime = cooldownTime;
            anim.SetFloat("cooldown", cooldownTime);            

            // If outside the combo advancement window
            if (anim.GetFloat("curAnimTime") < comboAdvancementWindowStart ||
                anim.GetFloat("curAnimTime") >= comboAdvancementWindowEnd)
            {
                anim.SetBool("comboWindow", false);
            }
            // If inside the combo advancement window
            else if (anim.GetFloat("curAnimTime") >= comboAdvancementWindowStart &&
                anim.GetFloat("curAnimTime") < comboAdvancementWindowEnd)
            {
                anim.SetBool("comboWindow", true);
            }            

            if (anim.GetBool("playerClick") && !anim.GetBool("comboWindow"))
            {
                anim.SetBool("comboBreak", true);
            }

            if (anim.GetBool("playerClick") && anim.GetBool("comboWindow") && !anim.GetBool("comboBreak"))
            {
                anim.SetBool("comboAdvance", true);
            }
        }
        else if (!anim.GetBool("isAttacking"))
        {
            comboBarObj.SetActive(false);
        }

        // If the player can't attack, make it able to after the cooldown elapses.
        if (!anim.GetBool("canAttack"))
        {
            if ((anim.GetFloat("cooldown")) == cooldownTime)
            {
                tempCooldownTime = cooldownTime;
            }
            
            anim.SetFloat("cooldown", tempCooldownTime -= Time.deltaTime);
            Debug.Log(tempCooldownTime);
            if (anim.GetFloat("cooldown") <= 0f)
            {
                anim.SetBool("canAttack", true);
            }
        }
    }

    public void DoComboUI()
    {
        // Activates the combo bar UI while an attack animation is playing. Sets the left and right
        // images to fill relative to the window of combo advancement set in the inspector and moves
        // the lil square indicator along the bar 

        comboBarObj.SetActive(true);
        comboBarBreakLeft.GetComponent<Image>().fillAmount = comboAdvancementWindowStart;
        comboBarBreakRight.GetComponent<Image>().fillAmount = 1 - comboAdvancementWindowEnd;

        float tickPos = anim.GetFloat("curAnimTime");
        comboBarTickObj.GetComponent<RectTransform>().anchorMin = new Vector2(tickPos, 0.5f);
        comboBarTickObj.GetComponent<RectTransform>().anchorMax = new Vector2(tickPos, 0.5f);
    }
}

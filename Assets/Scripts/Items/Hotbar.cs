using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ItemSystem;
using UIManager;


public class Hotbar : MonoBehaviour
{
    public static Hotbar instance;

    private Animator anim;
    [SerializeField] private UIHotbar UIhotbar;

    [Header("Hotbar Keys")]
    public KeyCode HealthPotion = KeyCode.Alpha1;
    public KeyCode ManaPotion = KeyCode.Alpha2;
    public KeyCode BuffPotion = KeyCode.Alpha3;
    public KeyCode MonsterMeat = KeyCode.Alpha4;
    public KeyCode Item = KeyCode.Mouse1;
    public KeyCode Magic = KeyCode.Q;


    [Header("Current Magic")]
    public float MagicCooldownDuration = 5f;
    public UseItem useMagic;

    [Header("Current Item")]
    public float ItemCooldownDuration = 5f;
    public UseItem useItem;

    [Header("Health Potion")]
    public float HealthPotionCooldownDuration = 5f;
    public UseItem useHealthPotion;

    [Header("Mana Potion")]
    //mana potion
    public float ManaPotionCooldownDuration = 5f;
    public UseItem useManaPotion;

    [Header("Buff Potion")]
    //buff potion
    public float BuffPotionCooldownDuration = 5f;
    public UseItem useBuffPotion;

    [Header("Monster Meat")]
    //monster meat
    public float MeatCooldownDuration = 5f;
    public UseItem useMonsterMeat;

    private float ResetCooldown;

    private void Awake()
    {
        if (Hotbar.instance == null)
        {
            Hotbar.instance = this;
        }
        else if (Hotbar.instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UIhotbar = UIPlayerManager.UI.GetComponent<UIHotbar>();
        anim = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        if (useMagic != null)
        {
            //Checks player input for magic key; Checks to make sure player is not on cooldown or using item
            if (Input.GetKeyDown(Magic) && GameManager.PlayerManager.pm.usingItem == false) // && UIhotbar.magiccooldownImage.fillAmount == 0
            {
                //Plays specific fireball animation in layer
                anim.Play("Fireball", 2);

                //Uses item and starts cooldown
                GameManager.PlayerManager.pm.usingItem = true;
                GameManager.PlayerManager.pm.CurrentMana -= 10f;
                StartCoroutine(StartCoolDown(MagicCooldownDuration, UIhotbar.magiccooldownImage));
            }
        }




        if (useItem != null)
        {
            //Checks player input for item key; 
            if (Input.GetKeyDown(Item) && GameManager.PlayerManager.pm.usingItem == false) //&& UIhotbar.itemcooldownImage.fillAmount == 0
            {

                if (useItem.itemName == "Bow")
                {
                    anim.Play("Bow", 1);
                }
                else
                {
                    anim.Play("Boomerang", 1);
                }



                GameManager.PlayerManager.pm.usingItem = true;
                GameManager.PlayerManager.pm.CurrentStamina -= 15f;
                StartCoroutine(StartCoolDown(ItemCooldownDuration, UIhotbar.itemcooldownImage));
            }


            PotionUse();
            if(UIhotbar)
            {
                UIhotbar.PotionAmountText();
            }

        }


    }

    //Uses current item
    public void UseItem()
    {
        useItem.OnUseItem(GameManager.PlayerManager.pm.transform);
    }

    //Uses current magic
    public void UseMagic()
    {
        useMagic.OnUseItem(GameManager.PlayerManager.pm.transform);
    }

    //Uses potion on hotbar key press
    private void PotionUse()
    {
        //Health Potion
        if (Input.GetKeyUp(HealthPotion))
        {
            useHealthPotion.OnUseItem(GameManager.PlayerManager.pm.transform);
            HealthPotionCooldown();

        }
        //Mana Potion
        if (Input.GetKeyUp(ManaPotion))
        {
            useManaPotion.OnUseItem(GameManager.PlayerManager.pm.transform);
            ManaPotionCooldown();
        }
        //Buff Potion
        if (Input.GetKeyUp(BuffPotion))
        {
            useBuffPotion.OnUseItem(GameManager.PlayerManager.pm.transform);
            BuffPotionCooldown();
        }
        //Monster Meat
        if (Input.GetKeyUp(MonsterMeat))
        {
            useMonsterMeat.OnUseItem(GameManager.PlayerManager.pm.transform);
            MonsterMeatCooldown();
        }
    }

    public void HealthPotionCooldown()
    {
        if (GameManager.PlayerManager.pm.HealthPotionAmount != 0 && UIhotbar.healthcooldownImage.fillAmount == 0)
        {
            StartCoroutine(StartCoolDown(HealthPotionCooldownDuration, UIhotbar.healthcooldownImage));
            GameManager.PlayerManager.pm.HealthPotionAmount -= 1;
            UIhotbar.healthPotionAmounTXT.text = "x" + GameManager.PlayerManager.pm.HealthPotionAmount;
        }

    }
    public void ManaPotionCooldown()
    {
        if (GameManager.PlayerManager.pm.ManaPotionAmount != 0 && UIhotbar.manacooldownImage.fillAmount == 0)
        {
            StartCoroutine(StartCoolDown(ManaPotionCooldownDuration, UIhotbar.manacooldownImage));
            GameManager.PlayerManager.pm.ManaPotionAmount -= 1;
            UIhotbar.manaPotionAmountTXT.text = "x" + GameManager.PlayerManager.pm.ManaPotionAmount;
        }

    }

    public void BuffPotionCooldown()
    {
        if (GameManager.PlayerManager.pm.BuffPotionAmount != 0 && UIhotbar.buffcooldownImage.fillAmount == 0)
        {
            StartCoroutine(StartCoolDown(BuffPotionCooldownDuration, UIhotbar.buffcooldownImage));
            GameManager.PlayerManager.pm.BuffPotionAmount -= 1;
            UIhotbar.buffPotionAmountTXT.text = "x" + GameManager.PlayerManager.pm.BuffPotionAmount;
        }

    }

    public void MonsterMeatCooldown()
    {
        if (GameManager.PlayerManager.pm.MonsterMeatAmount != 0 && UIhotbar.meatcooldownImage.fillAmount == 0)
        {
            StartCoroutine(StartCoolDown(MeatCooldownDuration, UIhotbar.meatcooldownImage));
            GameManager.PlayerManager.pm.MonsterMeatAmount -= 1;
            UIhotbar.meatAmountTXT.text = "x" + GameManager.PlayerManager.pm.MonsterMeatAmount;
        }

    }

    //Starts cooldown for specific hotbar and its cooldown
    public IEnumerator StartCoolDown(float duration, Image fill)
    {
        GameManager.PlayerManager.pm.usingItem = false;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float reset = t / duration;
            fill.fillAmount = reset;
            yield return null;
        }

        fill.fillAmount = 0;
    }

}

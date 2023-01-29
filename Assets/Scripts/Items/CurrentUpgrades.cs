using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using UnityEngine.UI;

public class CurrentUpgrades : MonoBehaviour
{
    public UseItem CurrentItem;
    public UseItem CurrentMagic;

    public Hotbar hotbar;

    public int CurrentItemLVL;
    public int CurrentMagicLVL;

    public int itemSkillRequirement = 1;
    public int magicSkillRequirement = 1;

    // Start is called before the first frame update
    void Start()
    {
        hotbar = GetComponentInChildren<Hotbar>();
        CurrentMagic = hotbar.useMagic;
        CurrentItem = hotbar.useItem;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkillRequirement();

        //Makes current
        if (CurrentMagic != hotbar.useMagic)
        {
            CurrentMagic = hotbar.useMagic;
        }

        if (CurrentItem != hotbar.useItem)
        {
            CurrentItem = hotbar.useItem;
        }
    }

    public void IncreaseItemLevel()
    {
        if(hotbar.useItem.itemLVL == 0)
        {
            LevelSystem.instance.skillPoints -= itemSkillRequirement;
            hotbar.useItem.itemLVL += 1;
            CurrentItemLVL = hotbar.useItem.itemLVL;
            //itemLVLTXT.text = "Lvl: " + hotbar.useItem.itemLVL;
        }
        else if(hotbar.useItem.itemLVL == 1)
        {
            LevelSystem.instance.skillPoints -= itemSkillRequirement;
            hotbar.useItem.itemLVL += 1;
            CurrentItemLVL = hotbar.useItem.itemLVL;
            //itemLVLTXT.text = "Lvl: " + hotbar.useItem.itemLVL;
        }
        else if (hotbar.useItem.itemLVL == 2)
        {
            LevelSystem.instance.skillPoints -= itemSkillRequirement;
            hotbar.useItem.itemLVL += 1;
            CurrentItemLVL = hotbar.useItem.itemLVL;
            //itemLVLTXT.text = "Lvl: " + hotbar.useItem.itemLVL;
        }
    }

    public void IncreaseMagicLevel()
    {

        if (hotbar.useMagic.itemLVL == 0)
        {
            LevelSystem.instance.skillPoints -= magicSkillRequirement;
            hotbar.useMagic.itemLVL += 1;
            CurrentMagicLVL = hotbar.useMagic.itemLVL;
            //magicLVLTXT.text = "Lvl: " + hotbar.useMagic.itemLVL;
        }
        else if (hotbar.useMagic.itemLVL == 1)
        {
            LevelSystem.instance.skillPoints -= magicSkillRequirement;
            hotbar.useMagic.itemLVL += 1;
            CurrentMagicLVL = hotbar.useMagic.itemLVL;
            //magicLVLTXT.text = "Lvl: " + hotbar.useMagic.itemLVL;
        }
        else if (hotbar.useMagic.itemLVL == 2)
        {
            LevelSystem.instance.skillPoints -= magicSkillRequirement;
            hotbar.useMagic.itemLVL += 1;
            CurrentMagicLVL = hotbar.useMagic.itemLVL;
            //magicLVLTXT.text = "Lvl: " + hotbar.useMagic.itemLVL;
        }
    }

    private void UpdateSkillRequirement()
    {
        //Checks current item level
        if (hotbar.useItem != null)
        {
            if (hotbar.useItem.itemLVL == 0)
            {
                //skill points required to unlock upgrade
                itemSkillRequirement = 1;
            }
            else if (hotbar.useItem.itemLVL == 1)
            {
                itemSkillRequirement = 3;
            }
            else if (hotbar.useItem.itemLVL == 2)
            {
                itemSkillRequirement = 5;
            }
        }

        if(hotbar.useMagic != null)
        {
            if (hotbar.useMagic.itemLVL == 0)
            {
                magicSkillRequirement = 1;
            }
            else if (hotbar.useMagic.itemLVL == 1)
            {
                magicSkillRequirement = 3;
            }
            else if (hotbar.useMagic.itemLVL == 2)
            {
                magicSkillRequirement = 5;
            }
        }
    }

}

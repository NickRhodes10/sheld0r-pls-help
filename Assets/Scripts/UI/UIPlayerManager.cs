using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameManager;

namespace UIManager
{
    public class UIPlayerManager : MonoBehaviour
    {
        public static UIPlayerManager UI;

        [Header("Main Stats UI")]
        //Insert all UI StatBars that track player stats
        [SerializeField] private Image[] healthbar;
        [SerializeField] private Image[] staminabar;
        [SerializeField] private Image[] manabar;

        [SerializeField] private TMP_Text[] LevelTXT;                                 //All level texts in game
        [SerializeField] private TMP_Text[] ExpTXT;                                   //All Exp texts in game
        [SerializeField] private Image[] ExpSlider;

        [SerializeField] private TMP_Text GoldAmountTXT;

        [SerializeField] private TMP_Text skillpointTXT;

        private float staminaFILLAMOUNT;
        private float healthFILLAMOUNT;
        private float manaFillAMOUNT;

        private float _MAXFILLAMOUNT = 1.0f;

        //Displays in menu
        [Header("Item/Magic UI")]
        [SerializeField] private Image[] DisplayItemImage;
        [SerializeField] private Image[] DisplayMagicImage;

        [SerializeField] private Button menuItemUpgrade;
        [SerializeField] private Button menuMagicUpgrade;

        [SerializeField] private TMP_Text itemLVLTXT;
        [SerializeField] private TMP_Text magicLVLTXT;

        private void Awake()
        {
            if (UI == null)
            {
                UI = this;
            }
            else if (UI != this)
            {
                Destroy(this);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            itemLVLTXT.text = "Lvl: " + GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().CurrentItemLVL;
            magicLVLTXT.text = "Lvl: " + GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().CurrentMagicLVL;

            //sets the healthbar and staminabar to max
            foreach (Image bar in healthbar)
            {
                bar.fillAmount = _MAXFILLAMOUNT;
            }

            foreach (Image bar in staminabar)
            {
                bar.fillAmount = _MAXFILLAMOUNT;
            }

            foreach (Image bar in manabar)
            {
                bar.fillAmount = _MAXFILLAMOUNT;
            }

        }

        // Update is called once per frame
        void Update()
        {
            //Displays current fill amount
            staminaFILLAMOUNT = DamageConversion(GameManager.PlayerManager.pm.CurrentStamina, GameManager.PlayerManager.pm.MaxStamina);
            healthFILLAMOUNT = DamageConversion(GameManager.PlayerManager.pm.CurrentHealth, GameManager.PlayerManager.pm.MaxHealth);
            manaFillAMOUNT = DamageConversion(GameManager.PlayerManager.pm.CurrentMana, GameManager.PlayerManager.pm.MaxMana);


            DisplayStatConversion(); //Converts health, stamina, and mana into fillamount for health, stamina, and mana bars
            StatsCheck(); // Checks to make sure numbers dont go below or above 0 and 100

            GoldAmountTXT.text = "Gold: " + GameManager.PlayerManager.pm.goldAmount;

            UpdateLVLSlidertxt(); //Updates all sliders to current stat

            skillpointTXT.text = "Skill Points: " + LevelSystem.instance.skillPoints;
            if(GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().hotbar.useItem != null)
            {
                itemLVLTXT.text = "Lvl: " + GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().hotbar.useItem.itemLVL;
            }
           

            //checks to make sure there is an item, if not put nothing image
            if (GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().hotbar.useItem == null)
            {
                foreach (var item in DisplayItemImage)
                {
                    item.sprite = null;
                }
            }
            else
            {
                foreach (var item in DisplayItemImage)
                {
                    item.sprite = GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().hotbar.useItem.itemIcon; ;
                }
            }

            if (GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().hotbar.useMagic == null)
            {
                foreach (var item in DisplayMagicImage)
                {
                    item.sprite = null;
                }
            }
            else
            {
                foreach (var item in DisplayMagicImage)
                {
                    item.sprite = GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().hotbar.useMagic.itemIcon; ;
                }
            }


        }


        //Converts regular values into values that can be used for the health and staminabar
        private float DamageConversion(float curStat, float maxStat)
        {
            var conversion = (curStat * _MAXFILLAMOUNT) / maxStat;

            return conversion;
        }

        private void StatsCheck()
        {
            //Makes sure health doesn't go over max and under 0
            if (GameManager.PlayerManager.pm.CurrentHealth > GameManager.PlayerManager.pm.MaxHealth)
            {
                GameManager.PlayerManager.pm.CurrentHealth = GameManager.PlayerManager.pm.MaxHealth;
            }
            if (GameManager.PlayerManager.pm.CurrentHealth < 0)
            {
                GameManager.PlayerManager.pm.CurrentHealth = 0;
            }

            //Makes sure stamina doesn't go over max and under 0
            if (GameManager.PlayerManager.pm.CurrentStamina > GameManager.PlayerManager.pm.MaxStamina)
            {
                GameManager.PlayerManager.pm.CurrentStamina = GameManager.PlayerManager.pm.MaxStamina;
            }
            if (GameManager.PlayerManager.pm.CurrentStamina < 0)
            {
                GameManager.PlayerManager.pm.CurrentStamina = 0;
            }

            //Makes sure mana doesn't go over max and under 0
            if (GameManager.PlayerManager.pm.CurrentMana > GameManager.PlayerManager.pm.MaxMana)
            {
                GameManager.PlayerManager.pm.CurrentMana = GameManager.PlayerManager.pm.MaxMana;
            }
            if (GameManager.PlayerManager.pm.CurrentMana < 0)
            {
                GameManager.PlayerManager.pm.CurrentMana = 0;
            }
        }

        //MAIN STATS slider conversion
        private void DisplayStatConversion()
        {
            //converts amount and fluidly change health with lerp
            foreach (var bar in healthbar)
            {
                if (bar.fillAmount != DamageConversion(GameManager.PlayerManager.pm.CurrentHealth, GameManager.PlayerManager.pm.MaxHealth))
                {
                    bar.fillAmount = Mathf.Lerp(bar.fillAmount, DamageConversion(GameManager.PlayerManager.pm.CurrentHealth, GameManager.PlayerManager.pm.MaxHealth), 0.005f);
                }
                if (bar.fillAmount >= DamageConversion(GameManager.PlayerManager.pm.CurrentHealth, GameManager.PlayerManager.pm.MaxHealth) - 0.05)
                {
                    bar.fillAmount = DamageConversion(GameManager.PlayerManager.pm.CurrentHealth, GameManager.PlayerManager.pm.MaxHealth);
                }

            }

            foreach (var bar in staminabar)
            {
                if (bar.fillAmount != DamageConversion(GameManager.PlayerManager.pm.CurrentStamina, GameManager.PlayerManager.pm.MaxStamina))
                {
                    bar.fillAmount = Mathf.Lerp(bar.fillAmount, DamageConversion(GameManager.PlayerManager.pm.CurrentStamina, GameManager.PlayerManager.pm.MaxStamina), 0.005f);
                }

                if (bar.fillAmount >= DamageConversion(GameManager.PlayerManager.pm.CurrentStamina, GameManager.PlayerManager.pm.MaxStamina) - 0.05)
                {
                    bar.fillAmount = DamageConversion(GameManager.PlayerManager.pm.CurrentStamina, GameManager.PlayerManager.pm.MaxStamina);
                }

            }

            foreach (var bar in manabar)
            {
                if (bar.fillAmount != DamageConversion(GameManager.PlayerManager.pm.CurrentMana, GameManager.PlayerManager.pm.MaxMana))
                {
                    bar.fillAmount = Mathf.Lerp(bar.fillAmount, DamageConversion(GameManager.PlayerManager.pm.CurrentMana, GameManager.PlayerManager.pm.MaxMana), 0.005f);
                }
                if (bar.fillAmount >= DamageConversion(GameManager.PlayerManager.pm.CurrentMana, GameManager.PlayerManager.pm.MaxMana) - 0.05)
                {
                    bar.fillAmount = DamageConversion(GameManager.PlayerManager.pm.CurrentMana, GameManager.PlayerManager.pm.MaxMana);
                }
            }

        }

        private void UpdateLVLtxt() //Updates all text UI
        {
            for (int i = 0; i < LevelTXT.Length; i++)
            {
                LevelTXT[i].text = "Level: " + GameManager.PlayerManager.pm.GetComponent<LevelSystem>().level;
            }

            for (int i = 0; i < ExpTXT.Length; i++)
            {
                ExpTXT[i].text = "EXP: " + GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experience + "/" + GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experienceToNextLevel;
            }
        }

        private void UpdateLVLSlidertxt()
        {
            //Grabs each slider and checks if slider equals current stat, if not increase stat to current
            foreach (var slider in ExpSlider)
            {
                if (slider.fillAmount != (float)GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experience / (float)GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experienceToNextLevel)
                {
                    slider.fillAmount = Mathf.Lerp(slider.fillAmount,
                        ((float)GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experience / (float)GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experienceToNextLevel), 0.005f);
                }

                if (slider.fillAmount >= (float)GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experience / (float)GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experienceToNextLevel - 0.005)
                {
                    slider.fillAmount = (float)GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experience / (float)GameManager.PlayerManager.pm.GetComponent<LevelSystem>().experienceToNextLevel;
                }
            }

        }

        private void UpdateUpgradeSkillRequirement()
        {
            if (GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().hotbar.useItem.itemLVL != 3)
            {
                menuItemUpgrade.GetComponentInChildren<TMPro.TMP_Text>().text = "Skill Points: " + GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().itemSkillRequirement;
            }
            else
            {
                menuItemUpgrade.GetComponentInChildren<TMPro.TMP_Text>().text = "Max";
            }

            if (GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().hotbar.useMagic.itemLVL != 3)
            {
                menuMagicUpgrade.GetComponentInChildren<TMPro.TMP_Text>().text = "Skill Points: " + GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().magicSkillRequirement;
            }
            else
            {
                menuMagicUpgrade.GetComponentInChildren<TMPro.TMP_Text>().text = "Max";
            }

        }

        private void SkillPointCheck()
        {

            if (GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().itemSkillRequirement <= LevelSystem.instance.skillPoints)
            {
                menuItemUpgrade.interactable = true;
            }
            else
            {
                menuItemUpgrade.interactable = false;
            }

            if (GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().magicSkillRequirement <= LevelSystem.instance.skillPoints)
            {
                menuMagicUpgrade.interactable = true;
            }
            else
            {
                menuMagicUpgrade.interactable = false;
            }

        }


    }
}



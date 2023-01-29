using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;
using TMPro;

public class StatsScript : MonoBehaviour
{
    public TMP_Text[] EXP;
    public TMP_Text[] Level;
    public TMP_Text Health;
    public TMP_Text Stamina;
    public TMP_Text Mana;
    public TMP_Text AttackDamage;
    public TMP_Text HealthPotion;
    public TMP_Text ManaPotion;

    private void Update()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {

        foreach (var EXPtext in EXP)
        {
            EXPtext.text = "EXP: " + LevelSystem.instance.experience + "/" + LevelSystem.instance.experienceToNextLevel;
        }

        foreach (var levelTXT in Level)
        {
            levelTXT.text = "Level: " + LevelSystem.instance.level;
        }
        Health.text = "Health: " + GameManager.PlayerManager.pm.CurrentHealth + "/" + GameManager.PlayerManager.pm.MaxHealth;
        Stamina.text = "Stamina: " + GameManager.PlayerManager.pm.CurrentStamina + "/" + GameManager.PlayerManager.pm.MaxStamina;
        Mana.text = "Mana: " + GameManager.PlayerManager.pm.CurrentMana + "/" + GameManager.PlayerManager.pm.MaxMana;
        HealthPotion.text = "Heal: " + GameManager.PlayerManager.pm.HealthPotionHeal;
        ManaPotion.text = "Mana Heal: " + GameManager.PlayerManager.pm.ManaPotionHeal;
    }
   
}

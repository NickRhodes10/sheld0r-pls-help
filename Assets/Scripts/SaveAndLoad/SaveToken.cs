using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerSaving
{
    //STATS TO SAVE UPON ENTERING NEW SCENE
    public class SaveToken : MonoBehaviour
    {
        [System.Serializable]
        public class PlayerSaveToken
        {
            public int Level;
            public int EXP;
            public int MaxEXP;

            public float CurrentHealth;
            public float MaxHealth;
            public float CurrentStamina;
            public float MaxStamina;
            public float CurrentMana;
            public float MaxMana;

            public int SkillPoints;

            public int HealthPotionAmount;
            public int ManaPotionAmount;
            public int BuffPotionAmount;

            public float HealthPotionHeal;
            public float ManaPotionHeal;

            public int MeatAmount;
            public int GoldAmount;

            public PlayerSaveToken(GameManager.PlayerManager manager)
            {
                Level = manager.GetComponent<LevelSystem>().level;
                EXP = manager.GetComponent<LevelSystem>().experience;
                MaxEXP = manager.GetComponent<LevelSystem>().experienceToNextLevel;

                CurrentHealth = manager.CurrentHealth;
                MaxHealth = manager.MaxHealth;
                CurrentStamina = manager.CurrentStamina;
                MaxStamina = manager.MaxStamina;
                CurrentMana = manager.CurrentMana;
                MaxMana = manager.MaxMana;

                SkillPoints = manager.skillPoints;

                HealthPotionAmount = manager.HealthPotionAmount;
                ManaPotionAmount = manager.ManaPotionAmount;
                BuffPotionAmount = manager.BuffPotionAmount;

                HealthPotionHeal = manager.HealthPotionHeal;
                ManaPotionHeal = manager.ManaPotionHeal;

                MeatAmount = manager.meatAmount;
                GoldAmount = manager.goldAmount;
            }
        }
    }
}


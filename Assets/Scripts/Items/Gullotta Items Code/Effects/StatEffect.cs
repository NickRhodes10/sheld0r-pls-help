using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectSystem;

[CreateAssetMenu(menuName = "Item Effect/Potion")]
public class StatEffect : EffectBase
{
    public int amount;
    public StatType statType;

    public override void UseEffect(Transform user)
    {
        //Make this better - Gullota
        if(user.gameObject.GetComponent<GameManager.PlayerManager>() == null)
        {
            return;
        }

        

          //make better might want enemies to use this as well - Gullota
          //Select stat type to affect 
        switch (statType)
        {
            case StatType.Health:
                GameManager.PlayerManager.pm.CurrentHealth += GameManager.PlayerManager.pm.HealthPotionHeal;
                break;
            case StatType.Mana:
                GameManager.PlayerManager.pm.CurrentMana += GameManager.PlayerManager.pm.ManaPotionHeal;
                break;
            case StatType.Stamina:
                GameManager.PlayerManager.pm.CurrentStamina += amount;
                break;
        }
    }

    public enum StatType
    {
        Health,
        Mana,
        Stamina
    }
}

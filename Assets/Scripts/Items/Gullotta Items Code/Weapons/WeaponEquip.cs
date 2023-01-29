using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class WeaponEquip : EquipItem
    {
        public DamageType damageType;
        public Vector2Int damageRange = new Vector2Int(1, 2);

        public virtual void OnAttack(/*character*/)
        {
            if (damageType.HasFlag(DamageType.Fire))
            {

            }
        }
    }

    //001011
    [System.Flags]
    public enum DamageType
    {
        Slash = 1,
        Stab = 2,
        Blunt = 4,
        Fire = 8,
        Lightning = 16,
        Frost = 32
    }
}
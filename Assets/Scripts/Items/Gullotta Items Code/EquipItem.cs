using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class EquipItem : ItemBase
    {
        public virtual EquipSlotType GetSlotType { get { return EquipSlotType.Head; } }

        public virtual void OnEquip(/* character */)
        {
            //Do x on equip
        }

        public virtual void OnUnequip(/* character */)
        {
            //do y on unequip
        }

        public enum EquipSlotType
        {
            Head,
            Chest,
            Legs,
            Feet,
            HandWeapon
        }
    }
}
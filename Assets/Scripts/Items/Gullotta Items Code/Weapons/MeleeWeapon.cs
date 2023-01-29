using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Item Sytem/Weapon/Melee")]
    public class MeleeWeapon : WeaponEquip
    {
        public WeaponType weaponType;
        public override EquipSlotType GetSlotType { get { return EquipSlotType.HandWeapon; } }

        public enum WeaponType
        {
            Sword,
            Axe,
            Spear
        }
    }
}
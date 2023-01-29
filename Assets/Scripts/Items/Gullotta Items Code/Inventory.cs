using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using UnityEngine.UI;
using System;

//Should be standalone in character, unless you have a real reason to keep it as a mono
public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemToken[] _itemsInInventory;

    private void Awake()
    {
        if (_itemsInInventory == null || _itemsInInventory.Length == 0)
        {
            //No idea what I am doing anymore!
            int amount = 3 * (Enum.GetValues(typeof(EquipItem.EquipSlotType)).Length + 1);

            _itemsInInventory = new ItemToken[amount];
        }
    }

    /*
    private int SortByType(ItemToken A, ItemToken B)
    {

    }
    */
}

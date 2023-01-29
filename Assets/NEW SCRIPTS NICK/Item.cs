using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
      public string itemName;
      public int itemLevel;

      public virtual void Use()
      {
            Debug.Log("Using " + itemName);
      }

      public void LevelUp()
      {
            itemLevel++;
            Debug.Log(itemName + " has leveled up to level " + itemLevel);
            OnLevelUp();
      }

      public virtual void OnLevelUp()
      {
            // Override this method in child classes to add functionality when the item levels up
      }
}

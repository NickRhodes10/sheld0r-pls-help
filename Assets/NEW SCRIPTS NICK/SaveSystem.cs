using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
      public static void SaveItems(List<Item> items)
      {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/items.bin";
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, items);
            stream.Close();
      }

      public static List<Item> LoadItems()
      {
            string path = Application.persistentDataPath + "/items.bin";
            if (File.Exists(path))
            {
                  BinaryFormatter formatter = new BinaryFormatter();
                  FileStream stream = new FileStream(path, FileMode.Open);

                  List<Item> items = formatter.Deserialize(stream) as List<Item>;
                  stream.Close();

                  return items;
            }
            else
            {
                  Debug.LogError("Save file not found in " + path);
                  return null;
            }
      }
}
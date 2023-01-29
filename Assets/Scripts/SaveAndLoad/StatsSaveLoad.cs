using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace PlayerSaving
{
    public class StatsSaveLoad
    {
        public static void Save(GameManager.PlayerManager gm)
        {
            string filePath = Application.persistentDataPath + "/PlayerStats.pps";
            FileStream stream = new FileStream(filePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            PlayerSaving.SaveToken.PlayerSaveToken data = new PlayerSaving.SaveToken.PlayerSaveToken(gm);
            bf.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerSaving.SaveToken.PlayerSaveToken Load()
        {
            string filePath = Application.persistentDataPath + "/Playerstats.pps";
            if (File.Exists(filePath))
            {
                FileStream stream = new FileStream(filePath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                PlayerSaving.SaveToken.PlayerSaveToken data = (PlayerSaving.SaveToken.PlayerSaveToken)bf.Deserialize(stream);
                stream.Close();
                return data;

            }

            return null;

        }


    }
}


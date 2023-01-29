using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSaving;



//Fake manager mimicking simple saves
    public class FakePlayerManager : MonoBehaviour
    {
        public static FakePlayerManager instance;

        public int Level = 1;
        public int Health = 10;
        public int Damage = 2;

        private void Awake()
        {
            if (FakePlayerManager.instance == null)
            {
                FakePlayerManager.instance = this;
            }
            else if (FakePlayerManager.instance != this)
            {
                Destroy(this);
            }
        }

    }



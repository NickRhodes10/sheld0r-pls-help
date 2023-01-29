using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapGeneration
{
    public class GenerationStages : MonoBehaviour
    {
        [HideInInspector] public int roomsGenerated = 0;
        [HideInInspector] public int curWFCRooms = 0;
        [HideInInspector] public int finWFCRooms = 0;
        [HideInInspector] public bool startEndFound = false;
        private bool spaceManagerCalled = false;

        private float finWFCPercent = 0f;
        private float finRoomGenFloat = 0f;
        private float finStartEndFloat = 0f;
        private float finSpaceManagerFloat = 0f;

        public float completionPercent = 0f;        

        public GameObject loadingScreen;
        public Image loadingBar;
        private float loadedTimer = 1.5f;

        public static GenerationStages instance;

        void Start()
        {
            if (GenerationStages.instance == null)
                GenerationStages.instance = this;
            if (GenerationStages.instance != this)
                Destroy(this);
        }

        void Update()
        {
            if (roomsGenerated > 0)
            {
                finRoomGenFloat = 1f;
                finWFCPercent = ((float)finWFCRooms) / ((float)roomsGenerated);
            }

            if (startEndFound == true)
            {
                finStartEndFloat = 1f;
            }

            if (roomsGenerated > 0 && curWFCRooms == 0 && spaceManagerCalled == false)
            {
                Debug.Log("Ready for SpaceManager @ " + Time.time);
                SpaceManager.instance.CalculateSpace();
                finSpaceManagerFloat = 1f;
                spaceManagerCalled = true;
            }

            completionPercent = (finWFCPercent + finRoomGenFloat + finStartEndFloat + finSpaceManagerFloat) / 4f;
            loadingBar.fillAmount = completionPercent;

            if (completionPercent == 1 && loadingScreen.activeInHierarchy)
            {
                loadedTimer -= Time.deltaTime;
                if (loadedTimer <= 0)
                {
                    loadingScreen.SetActive(false);
                }                
            }
        }
    }
}
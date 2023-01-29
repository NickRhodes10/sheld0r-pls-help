using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu(menuName = "WFC/Tileset")]
    public class Tileset : ScriptableObject
    {
        public Module[] modules;

        public void SetNeighbors()
        {
            TileConnector.SetNeighbors(this);

            /*
            for (int i = 0; i < modules.Length; i++)
            {
                List<Module> north = new List<Module>();
                List<Module> east = new List<Module>();
                List<Module> south = new List<Module>();
                List<Module> west = new List<Module>();

                for (int m = 0; m < modules.Length; m++)
                {
                    string curModuleDirections = modules[i].name.Substring(modules[i].name.Length - 4);
                    string moduleToEvaluate = modules[m].name.Substring(modules[i].name.Length - 4);

                    if(
                        (curModuleDirections.Contains("N") && moduleToEvaluate.Contains("S")) ||
                        curModuleDirections.Contains("N") == false && moduleToEvaluate.Contains("S") == false)
                    {
                        north.Add(modules[m]);
                    }

                    if (
                        (curModuleDirections.Contains("E") && moduleToEvaluate.Contains("W")) ||
                        curModuleDirections.Contains("E") == false && moduleToEvaluate.Contains("W") == false)
                    {
                        east.Add(modules[m]);
                    }

                    if (
                        (curModuleDirections.Contains("S") && moduleToEvaluate.Contains("N")) ||
                        curModuleDirections.Contains("S") == false && moduleToEvaluate.Contains("N") == false)
                    {
                        south.Add(modules[m]);
                    }

                    if (
                        (curModuleDirections.Contains("W") && moduleToEvaluate.Contains("E")) ||
                        curModuleDirections.Contains("W") == false && moduleToEvaluate.Contains("E") == false)
                    {
                        west.Add(modules[m]);
                    }
                }

                modules[i].north = north.ToArray();
                modules[i].east = east.ToArray();
                modules[i].south = south.ToArray();
                modules[i].west = west.ToArray();
            }
                        */
        }
    }
}

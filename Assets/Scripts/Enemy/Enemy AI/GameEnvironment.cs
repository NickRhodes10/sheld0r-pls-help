using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//public sealed class GameEnvironment
//{
//      private static GameEnvironment instance;
//      private List<GameObject> checkpoints = new List<GameObject>();
//      public List<GameObject> Checkpoints { get { return checkpoints; } }

//      public static GameEnvironment Singleton
//      {
//            get
//            {
//                  if(instance == null)
//                  {
//                        instance = new GameEnvironment();                                                               // Create instance of GameEnvironment
//                        instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));                 // Find Waypoints and add to list

//                        instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();        // Alphabetical order of waypoints
//                  }

//                  return instance;                                                                                      
//            }
//      }
//}

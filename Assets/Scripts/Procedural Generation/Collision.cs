using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGeneration;
using MathsHelper;

namespace CollisionHelper
{

  public class Collision3D
  {


    public static bool LineIntersectRoomBounds(Vector2 A, Vector2 B, Room room)
    {
      Vector2 midPoint = new Vector2(A.x, B.y);

      //Horizontal
      if(LineCalculator.isIntersectingLine2D(
        A,
        midPoint,
        new Vector2(room.Position.x - room.GetWidth, room.Position.y - room.GetHeight),
        new Vector2(room.Position.x + room.GetWidth, room.Position.y - room.GetHeight)
        ))
      {
        return true;
      }

      if (LineCalculator.isIntersectingLine2D(
        A,
        midPoint,
        new Vector2(room.Position.x - room.GetWidth, room.Position.y + room.GetHeight),
        new Vector2(room.Position.x + room.GetWidth, room.Position.y + room.GetHeight)
        ))
      {
        return true;
      }

      //Vertical
      if (LineCalculator.isIntersectingLine2D(
        midPoint,
        B,
        new Vector2(room.Position.x - room.GetWidth, room.Position.y - room.GetHeight),
        new Vector2(room.Position.x - room.GetWidth, room.Position.y + room.GetHeight)
        ))
      {
        return true;
      }

      if (LineCalculator.isIntersectingLine2D(
        midPoint,
        B,
        new Vector2(room.Position.x + room.GetWidth, room.Position.y - room.GetHeight),
        new Vector2(room.Position.x + room.GetWidth, room.Position.y + room.GetHeight)
        ))
      {
        return true;
      }

      return false;

    }

    public static bool DetectRoomCollision(List<Room> rooms, UIntRange seperationRange)
    {
      bool collisionFound = false;

      for (int x = 0; x < rooms.Count; x++)
      {
        for (int y = 0; y < rooms.Count; y++)
        {
            if (x == y) // if same room, continue
            {
              continue;
            }

            
            if (SeperateRooms(rooms[x], rooms[y], seperationRange) == true) // if not same room, seperate rooms
            {
              collisionFound = true;
            }
        }
      }

      return collisionFound;

    }

    
    public static bool SeperateRooms(Room A, Room B, UIntRange seperationRange)
    {
      if(A == B) // if same room, return false;
      {
        return false;
      }

      
      if(A.Position.x - A.GetWidth < B.Position.x + B.GetWidth &&
          A.Position.x + A.GetWidth > B.Position.x - B.GetWidth &&
          A.Position.y - A.GetHeight < B.Position.y + B.GetHeight &&
          A.Position.y + A.GetHeight > B.Position.y - B.GetHeight)
      {
        int rng = seperationRange.GetRandomIntValue;

        if(B.Position.x - A.Position.x > B.Position.y - A.Position.y)
        {
          if(A.Position.x > B.Position.x)
          {
            A.Position += new Vector2(rng, 0);
            B.Position += new Vector2(-rng, 0);
          }
          else
          {
            A.Position += new Vector2(-rng, 0);
            B.Position += new Vector2(rng, 0);
          }
        }
        else
        {
          if (A.Position.y > B.Position.y)
          {
            A.Position += new Vector2(0, rng);
            B.Position += new Vector2(0, -rng);
          }
          else
          {
            A.Position += new Vector2(0, -rng);
            B.Position += new Vector2(0, rng);
          }
        }

        return true;
      }

      return false;

    }

  }

}



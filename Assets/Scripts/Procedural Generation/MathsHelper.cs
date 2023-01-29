using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MathsHelper
{
    [System.Serializable]

    public class UIntRange // Unsigned Integer, always positive.
    {
        [SerializeField] private uint _minValue;
        [SerializeField] private uint _maxValue;

        public uint GetMinValue { get { return _minValue; } }
        public uint GetMaxValue { get { return _maxValue; } }

        public uint GetRandomUintValue { get { return (uint)Random.Range((int)_minValue, (int)_maxValue + 1); } } // Generate random UInt between min and max value
        public int GetRandomIntValue { get { return Random.Range((int)_minValue, (int)_maxValue + 1); } } // Generate random integer between min and max value
    }

    public class LineCalculator
    {
        public static bool isIntersectingLine2D(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            bool isIntersecting = false;

            float denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);

            if (denominator != 0)
            {
                float u_a = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
                float u_b = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;

                if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                {
                    isIntersecting = true;
                }

            }

            return isIntersecting;

        }
    }

    public class GenericNumbers
    {
        public static float Distance(float a, float b)
        {
            return Mathf.Abs(b - a);
        }
    }

}


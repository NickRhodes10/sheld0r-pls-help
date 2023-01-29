using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectSystem
{
    public abstract class EffectBase : ScriptableObject
    {
        public abstract void UseEffect(Transform user);
    }
}
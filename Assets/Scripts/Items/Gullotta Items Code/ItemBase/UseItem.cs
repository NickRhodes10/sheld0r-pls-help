using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectSystem;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Item/Use Item")]
    public class UseItem : ItemBase
    {
        public EffectBase[] effectsOnUse;

        //Call method to use item and all item effects on the player's transform
        public void OnUseItem(Transform user)
        {
            for (int i = 0; i < effectsOnUse.Length; i++)
            {
                effectsOnUse[i].UseEffect(user);
            }
        }
    }
}
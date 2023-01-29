using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItemPickup : MonoBehaviour
{
      private void OnTriggerEnter(Collider other)
      {
            if (other.gameObject.layer == LayerMask.NameToLayer("Resource"))
            {
                  var resource = other.gameObject.GetComponent<Resources>();

                  if (resource != null)
                  {
                        switch (resource.ResourceData.ResourceEnum)
                        {
                              case ResourceTypeEnum.MonsterMeat:
                                    GameManager.PlayerManager.pm.meatAmount += resource.ResourceData.GetAmount();
                                    resource.PickupResource();
                                    break;
                              case ResourceTypeEnum.Gold:
                                    GameManager.PlayerManager.pm.goldAmount += resource.ResourceData.GetAmount();
                                    resource.PickupResource();
                                    break;
                              case ResourceTypeEnum.Health:
                                    GameManager.PlayerManager.pm.HealthPotionAmount += resource.ResourceData.GetAmount();
                                    resource.PickupResource();
                                    break;
                              case ResourceTypeEnum.Mana:
                                    GameManager.PlayerManager.pm.ManaPotionAmount += resource.ResourceData.GetAmount();
                                    resource.PickupResource();
                                    break;
                        }
                  }


            }
      }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotion : ProjectileBase
{

      public Transform user;
      public Transform attackDirection;

      public Vector3 objectPool;
      public GameObject ExplosionPrefab;
      public float explosionTime;
      public bool travelTime;
      public float TravelTime;
      public bool explodes;
      public float explodeRadius;
      public float speed;
      public float comebackTime;
      public bool comesBack;
      private float timeTracker;
      private float timeTracker2;
      private bool Hit;

      [Header("Fireball Settings By Level: ")]
      [SerializeField] public int fireballDamage = 5;
      [SerializeField] public int fireballLevel = 1;

      [Header("Boomerang Settings By Level: ")]
      [SerializeField] private int boomerangLevel = 1;
      [SerializeField] private float stunLengthLvl1 = 1f;
      [SerializeField] private float stunLengthLvl2 = 1.5f;


      private bool Exploded = false;

      private GameObject fireballExplosion;

      private Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);


      // Update is called once per frame
      void Update()
      {

        
        fireballLevel = GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().CurrentMagicLVL;
        boomerangLevel = GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>().CurrentItemLVL;
        //Checks comesback bool and starts timer
        //Looks at player when time is reached
            if (comesBack == true)
            {
                  comebackTime -= Time.deltaTime;

                  if (comebackTime <= 0f)
                  {
                        transform.LookAt(user.position + new Vector3(0, 1, 0));
                  }

            }

            //Travels for a duration if player is not using and explodes bool is false
            if (travelTime == true && explodes == false && GameManager.PlayerManager.pm.usingItem == true)
            {
                  TravelTime -= Time.deltaTime;
                  //broken
                  if (TravelTime <= 0f)
                  {
                        //do something
                        transform.position = objectPool;
                  }
            }

            //If game object doesn't explode, continue to travel
            if (Exploded != true)
            {
                  transform.position += (transform.forward * speed * Time.deltaTime);
            }

            if (explodes)
            {
                  Explodes();
            }

      }

      private void OnCollisionEnter(Collision collision)
      {
            //Debug.Log(gameObject.transform.localScale.magnitude); Disabled by Patrick temporarily
            if (explodes)
            {
                  //Damages enemy
                  if (collision.gameObject.tag == "Enemy")
                  {
                        Hit = true;
                        collision.gameObject.GetComponent<IHittable>().GetHit(fireballDamage);                          // Apply Damage to enemy
                        
                        if(fireballLevel == 3)
                        {
                              fireballExplosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);      // Create explosion prefab
                              Destroy(fireballExplosion, 0.25f);
                        }


                  }

                  //Fireball reset to object pool and explodes
                  if (collision.gameObject.tag == "Wall")
                  {
                        Hit = true;
                        //fireballExplosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
                        transform.position = objectPool;
                        Exploded = true;

                       if (fireballLevel == 3)
                       {
                            fireballExplosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);      // Create explosion prefab
                            Destroy(fireballExplosion, 0.25f);
                       }
                  }

            }

            
            if (comesBack == true)
            {

                  //Colliding with something returns time to 0
                  if (collision.gameObject.tag != "Enemy" || collision.gameObject.tag != "Player")
                  {
                        comebackTime = 0;
                  }

                  //Pools object and sets to not using
                  if (collision.gameObject.tag == "Player")
                  {
                        GameManager.PlayerManager.pm.usingItem = false;
                        transform.position = objectPool;
                        gameObject.SetActive(false);

                  }

                  //Stuns enemy
                  if (collision.gameObject.tag == "Enemy" && boomerangLevel == 1)
                  {
                        collision.gameObject.GetComponent<EnemyStats>().GetStunned(stunLengthLvl1);
                  }
                  else if (collision.gameObject.tag == "Enemy" && boomerangLevel == 2)
                  {
                        collision.gameObject.GetComponent<EnemyStats>().GetStunned(stunLengthLvl2);
                  }

                  //Destroys Destructables
                  if (collision.gameObject.tag == "Destructable")
                  {
                        collision.gameObject.GetComponent<IHittable>().GetHit(1);
                  }

            }
            else if (comesBack == false && explodes == false)
            {
                  if (collision.gameObject.tag != "Player")
                  {
                        GameManager.PlayerManager.pm.usingItem = false;
                        transform.position = objectPool;
                        gameObject.SetActive(false);
                  }
            }

      }

      public void Explodes()
      {
            //explosion prefab time
            if (Exploded == true)
            {
                  //Checks the explosion time, once reached, destroy explosion
                  if (timeTracker2 >= explosionTime)
                  {
                        Destroy(fireballExplosion);
                        timeTracker2 -= explosionTime;
                        Hit = false;
                        Exploded = false;
                        GameManager.PlayerManager.pm.usingItem = false;
                        gameObject.SetActive(false);
                  }

                  if (Exploded == true)
                  {
                        timeTracker2 += Time.deltaTime;
                  }
                  else
                  {
                        timeTracker2 = 0;
                  }


            }
            //explosion after travel time
            if (Exploded != true && travelTime == true && Hit == false)
            {
                  //tracks time and if time is reached, explode fireball and reset position to object pool
                  if (timeTracker >= TravelTime)
                  {
                        Debug.Log("explosion");
                        if(fireballLevel == 3)
                        {
                            fireballExplosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
                        }

                        transform.position = objectPool;
                        Exploded = true;
                  }

                  //tracks the time from use
                  if (Exploded != true)
                  {
                        timeTracker += Time.deltaTime;
                  }
                  else
                  {
                        timeTracker = 0;
                  }

            }

            //Explodes fireball by incrasing scale to explode radius
            if (fireballExplosion != null)
            {
                  if (fireballExplosion.transform.localScale.magnitude < explodeRadius)
                  {
                        fireballExplosion.transform.localScale += scale;
                        Debug.Log(fireballExplosion.transform.localScale.magnitude);
                  }
            }
      }

}

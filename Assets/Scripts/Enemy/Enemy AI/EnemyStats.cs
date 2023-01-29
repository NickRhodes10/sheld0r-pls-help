using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour, IHittable
{
      [SerializeField] public int currentHealth;
      [SerializeField] public int maxHealth = 5;

      [SerializeField] public bool isDead;

      ResourceDropper resourceDropper;
      NavMeshAgent agent;
      
      Animator anim;
      AI ai;

      private void Start()
      {
            currentHealth = maxHealth;
            anim = GetComponent<Animator>();
            ai = GetComponent<AI>();
            agent = GetComponent<NavMeshAgent>();
            resourceDropper = GetComponentInChildren<ResourceDropper>();
      }

      private void Update()
      {
            if (isDead)
            {
                  StartCoroutine(Die());

            }
      }

      // Can be used later to scale enemy damage, health, or any other value decided.
      public void GetHit(int damage)
      {
            if(isDead == false)
            {
                  GetComponent<EnemyHealthUI>().healthSlider.enabled = true;
                  currentHealth -= damage;

                  if(currentHealth <= 0)
                  {
                      GetComponent<EnemyHealthUI>().healthSlider.enabled = true;
                      isDead = true;
                  }
            }
      }

      IEnumerator Die()
      {
            
            anim.SetTrigger("isDead");
            agent.isStopped = true;
            yield return new WaitForSeconds(3f);
            resourceDropper.DropItem();
            LevelSystem.instance.AddExperience(30);
            Destroy(gameObject);
      }

      public void GetStunned(float length)
      {
            if(length > 0)
            {
                  agent.speed = 0f;
                  anim.ResetTrigger("isWalking");
                  anim.ResetTrigger("isRunning");
                  anim.ResetTrigger("isAttacking");
                  anim.ResetTrigger("isIdle");
                  length -= Time.deltaTime;
            }

            if(length <= 0)
            {
                  agent.speed = 2f;
            }

      }
}

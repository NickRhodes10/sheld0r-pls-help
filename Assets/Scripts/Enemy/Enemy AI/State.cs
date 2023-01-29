using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class State
{
      public enum STATE
      {
            IDLE, PATROL, PURSUE, ATTACK, DEAD
      };

      public enum EVENT
      {
            ENTER, UPDATE, EXIT
      };

      public STATE name;
      protected EVENT stage;
      protected GameObject npc;
      protected Animator anim;
      protected Transform player;
      protected State nextState;
      protected NavMeshAgent agent;
      protected AI ai;

      [SerializeField] float visDist = 20.0f;                                                        // How far the AI can spot the player from
      [SerializeField] float visAngle = 65.0f;                                                       // AI FOV
      [SerializeField] float attackDist = 1.5f;                                                      // Distance from player needed to attack

      public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
      {
            npc = _npc;
            agent = _agent;
            anim = _anim;
            stage = EVENT.ENTER;
            player = _player;
      }

      public virtual void Enter() { stage = EVENT.UPDATE; }
      public virtual void Update() { stage = EVENT.UPDATE; }
      public virtual void Exit() { stage = EVENT.EXIT; }

      public State Process()
      {
            if (stage == EVENT.ENTER) Enter();
            if (stage == EVENT.UPDATE) Update();
            if (stage == EVENT.EXIT)
            {
                  Exit();
                  return nextState;
            }
            return this;
      }

      /// <summary>
      /// CanSeePlayer() returns a boolean which indicates whether or not the player is
      /// withing the AI FOV and within his range of sight.
      /// </summary>
      /// <returns></returns>
      public bool CanSeePlayer()
      {
            if (player != null)
            {

                  Vector3 direction = player.position - npc.transform.position;           // Determine direction vector from player to AI
                  float angle = Vector3.Angle(direction, npc.transform.forward);          // Determine the angle between the AI's forward vector and our direction vector

                  if (direction.magnitude < visDist && angle < visAngle)                   // If the length of the vector connecting player and AI is less than our visible distance float && the player is within AI FOV,
                  {
                        npc.GetComponent<EnemyHealthUI>().healthSlider.gameObject.SetActive(true);
                        return true;
                  }
                  npc.GetComponent<EnemyHealthUI>().healthSlider.gameObject.SetActive(false);
                  return false;
            }

            return false;

      }

      /// <summary>
      /// CanAttackPlayer returns a boolean which indicates whether or not the player is
      /// within the AI attack range.
      /// </summary>
      /// <returns></returns>
      public bool CanAttackPlayer()
      {
            Vector3 direction = player.position - npc.transform.position;           // Determine direction vector from player to AI
            attackDist = npc.GetComponent<AI>().attackRange;
            if (direction.magnitude < attackDist)                                    // If the length of the direction vector connecting player and AI is less than our attack distance float,
            {
                  return true;
            }

            return false;
      }

}

/// <summary>
/// Idle Class inherits from our State class, meaning we have to implement an Enter(), Update(),
/// and Exit() function(s). When we enter Idle, the AI should perform the Idle animation. In order
/// to break out of this state, one of two things must happen. If the CanSeePlayer() returns true,
/// the agent should begin chasing the player. Else we will have a 10% chance to begin patrolling 
/// between Checkpoints. 
/// </summary>
public class Idle : State
{
      public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
      {
            name = STATE.IDLE;                                                      // Assign the name of our State to be equal to STATE.IDLE
      }

      public override void Enter()
      {
            anim.SetTrigger("isIdle");                                              // Begin idle animation
                                                                                    //Debug.Log("IDLING");
            player = GameObject.FindGameObjectWithTag("Player").transform;
            base.Enter();
      }

      public override void Update()
      {
            if (CanSeePlayer())                                                     // If CanSeePlayer() returns true,
            {
                  nextState = new Pursue(npc, agent, anim, player);                 // Assign our State 'nextState' to a new pursue state, passing the parameters from our idle state
                  stage = EVENT.EXIT;                                               // Assign our current Event 'stage' to be exit
            }
            else if (Random.Range(0, 100) < 10)                                      // Else we have a 10% chance to begin patrolling
            {
                  nextState = new Wander(npc, agent, anim, player);                 // Assign our 'State' to a new patrol state, passing the parameters from our idle state
                  stage = EVENT.EXIT;                                               // Assign our current Event 'stage' to be exit
            }
      }

      public override void Exit()
      {
            anim.ResetTrigger("isIdle");                                            // When exiting this state, Reset Animator Controller Trigger
            base.Exit();
      }
}

/// <summary>
/// Patrol Class inherits from State class as well. The functionality provided by this class
/// allows the AI agent to walk between Checkpoints based on the shortest path. In order to break
/// out of this state, the agent must be able to see the player. 
/// </summary>
//public class Patrol : State
//{
//      int currentIndex = -1;
//      private List<GameObject> checkpoints = new List<GameObject>();

//      public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
//      {
//            name = STATE.PATROL;
//            agent.speed = 2;
//            agent.isStopped = false;
//      }

//      public override void Enter()
//      {                                                                                                     // Upon entering the Patrol State,
//            player = GameObject.FindGameObjectWithTag("Player").transform;
//            checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
//            checkpoints = checkpoints.OrderBy(waypoint => waypoint.name).ToList();

//            if (player != null)
//            {
//                  float lastDist = Mathf.Infinity;                                                                // Assign lastDist to infinity, this way any point compared will be guaranteed to be shorter than our initial value.
//                  for (int i = 0; i < checkpoints.Count; i++)                            // Loop through each GameObject with the tag "Checkpoint"
//                  {
//                        GameObject thisWP = checkpoints[i];                             // thisWP = current index of our checkpoint list
//                        float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);     // Determine distance from AI agent to the current index checkpoint


//                        if (distance < lastDist)                                                                   // If this distance is less than our previously evaluated checkpoint,
//                        {
//                              currentIndex = i - 1;                                                               // Change our current index to be one less than checked this iteration,
//                              lastDist = distance;                                                                // Assign lastDist to our distance calculated for this checkpoint.
//                        }
//                  }
//            }



//            anim.SetTrigger("isWalking");                                                                   // Trigger the Walking animation
//                                                                                                            //Debug.Log("PATROLLING");
//            base.Enter();
//      }

//      public override void Update()
//      {
//            // LOOP THROUGH CHECKPOINTS, MOVING AI AGENT BETWEEN THEM.

//            if (agent.remainingDistance < 1)                                                                 // If the AI agent is less than 1 unit from his current destination,
//            {
//                  if (currentIndex >= checkpoints.Count - 1)                       // If the currentIndex evaluated is greater than the number of checkpoints in our list - 1, 
//                  {
//                        currentIndex = 0;                                                                   // Set currentIndex to the beginning of the list
//                  }
//                  else
//                  {
//                        currentIndex++;                                                                     // Else incriment the currentIndex by 1, evaluating the next checkpoint
//                  }

//                  agent.SetDestination                                                                      // Move NavMeshAgent to the checkpoints position
//                        (checkpoints[currentIndex].transform.position);
//            }



//            if (CanSeePlayer())                                                                             // If the AI agent can see the player,
//            {
//                  nextState = new Pursue(npc, agent, anim, player);                                         // Update our nextState to pursue the player
//                  stage = EVENT.EXIT;                                                                       // Run code to exit the Patrol State.
//            }
//      }

//      public override void Exit()
//      {
//            anim.ResetTrigger("isWalking");                                                                 // Reset Walking animation
//            base.Exit();
//      }
//}


/// <summary>
/// Pursue Class inherits from the State class as well. The purpose of this class is to make the AI agent
/// chase the player around the map, while the player is withing the AI FOV. In order to break out of this state,
/// one of two conditions must be met. If the AI agent can attack the player, we will move to the Attack State.
/// Else if the agent no longer sees the player, the agent will return to patrolling the area.
/// </summary>
public class Pursue : State
{
      public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
      {
            name = STATE.PURSUE;
            agent.speed = 5;
            agent.isStopped = false;
      }

      public override void Enter()
      {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            anim.SetTrigger("isRunning");                                                                   // Upon entering the Pursue State, trigger the Running animation
                                                                                                            //Debug.Log("PURSUING PLAYER");
            base.Enter();
      }

      public override void Update()
      {
            agent.SetDestination(player.position);                                                          // While in the Pursue State, always be setting the destination to the players position
            if (agent.hasPath)                                                                              // If the agent has a path,
            {
                  if (CanAttackPlayer())                                                                    // And if the agent is within attack range,
                  {
                        nextState = new Attack(npc, agent, anim, player);                                   // Move to the Attack State
                        stage = EVENT.EXIT;
                  }
                  else if (!CanSeePlayer())                                                                 // If the agent can no longer see the player,
                  {
                        nextState = new Wander(npc, agent, anim, player);                                   // Move to the wander state
                        stage = EVENT.EXIT;
                  }
            }
      }

      public override void Exit()
      {
            UnityEngine.UI.Slider healthSlider = npc.GetComponent<EnemyHealthUI>().healthSlider;
            healthSlider.enabled = false;
            anim.ResetTrigger("isRunning");                                                                 // When exiting the Pursue state, reset the running animation trigger
            base.Exit();
      }
}


/// <summary>
/// Attack Class inherits from State class as well. The purpose of the Attack state is to perform an attack
/// on the player. This state can be broken out of if the AI agent can no longer attack the player, if this is true,
/// the agent will resume pursuing the player.
/// </summary>
public class Attack : State
{
      EnemyStats enemyStats;
      GameManager.PlayerManager pm;
      float rotationSpeed = 10.0f;
    //AudioSource attackAudio;
    

      public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
      {
            name = STATE.ATTACK;
            //attackAudio = npc.GetComponent<AudioSource>();
      }

      public override void Enter()
      {
            agent.isStopped = true;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            pm = player.GetComponent<GameManager.PlayerManager>();
            anim.SetTrigger("isAttacking");                                                                 // When entering the attack state, play attack animation
                                                                                                            //Debug.Log("ATTACKING PLAYER");                                                                                                            // Stop the AI agent from moving
                                                                                                            //attackAudio.Play();
            base.Enter();
      }

      public override void Update()
      {
            Vector3 direction = player.position - npc.transform.position;                                   // Determine the direction vector between the player and the AI agent
            float angle = Vector3.Angle(direction, npc.transform.forward);                                  // Determing the angle between the direction vector and the AI agents forward vector
            direction.y = 0;                                                                                // Lock the Y axis to prevent any tilting
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,                               // Rotate the AI agent toward the direction calculated
                                     Quaternion.LookRotation(direction),
                                     Time.deltaTime * rotationSpeed);

            agent.isStopped = true;

            if (!CanAttackPlayer())                                                                         // If the AI agent no longer attack the player,
            {
                  nextState = new Pursue(npc, agent, anim, player);                                         // Resume the pursue state
                  stage = EVENT.EXIT;
            }
            else if (pm.CurrentHealth <= 0)
            {
                  nextState = new Wander(npc, agent, anim, player);
                  stage = EVENT.EXIT;
            }
            //else if (enemyStats.currentHealth <= 0)
            //{
            //      nextState = new Dead(npc, agent, anim, player);
            //      stage = EVENT.EXIT;
            //}
      }

      public override void Exit()
      {
            anim.ResetTrigger("isAttacking");                                                               // Upon exiting the Attack State, reset the animation trigger for the attack
                                                                                                            //attackAudio.Stop();
            base.Exit();
      }

}


public class Wander : State
{
      float wanderRadius = 25f;
      float wanderTimer = 3f;

      Transform target;
      float timer;

      public Wander(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
      {
            name = STATE.PATROL;
            agent.speed = 2;
            agent.isStopped = false;
      }

      public override void Enter()
      {
            anim.SetTrigger("isWalking");
            player = GameObject.FindGameObjectWithTag("Player").transform;
            timer = wanderTimer;
            base.Enter();
      }

      public override void Update()
      {
            timer += Time.deltaTime;
            int numberOfRays = 5;
            float angle = 25f;
            float rayRange = 3f;
            int count = 0;

            for (int i = 0; i < numberOfRays; i++)
            {
                  var rotation = agent.transform.rotation;
                  var rotationMod = Quaternion.AngleAxis((i / ((float)numberOfRays - 1)) * angle * 2 - angle, agent.transform.up);
                  var direction = rotation * rotationMod * Vector3.forward;


                  var ray = new Ray(agent.transform.position + new Vector3(0, 1, 0), direction);
                  RaycastHit hitInfo;

                  Debug.DrawRay(agent.transform.position + new Vector3(0, 1, 0), direction);

                  if (Physics.Raycast(ray, out hitInfo, rayRange, 11))
                  {
                        if (hitInfo.collider.gameObject.tag == "Wall" || hitInfo.collider.gameObject.tag == "Destructable")
                        {
                              if (hitInfo.distance < 1 && count == 0)
                              {
                                    agent.SetDestination(SetNewPos());
                                    count++;
                              }

                        }
                  }
            }


            if (timer >= wanderTimer)
            {
                  agent.SetDestination(SetNewPos());

                  if (agent.remainingDistance < 1.0f)
                  {
                        nextState = new Idle(npc, agent, anim, player);
                        stage = EVENT.EXIT;

                  }

                  agent.isStopped = false;
                  timer = 0;
                  anim.SetTrigger("isWalking");
            }

            if (CanSeePlayer())                                                                             // If the AI agent can see the player,
            {
                  nextState = new Pursue(npc, agent, anim, player);                                         // Update our nextState to pursue the player
                  stage = EVENT.EXIT;                                                                       // Run code to exit the Patrol State.
            }

      }

      public Vector3 SetNewPos()
      {
            Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
            return newPos;
      }

      public override void Exit()
      {
            anim.ResetTrigger("isWalking");                                                                 // Reset Walking animation
            base.Exit();
      }

      public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
      {
            Vector3 randomDirection = Random.insideUnitSphere * dist;
            randomDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);
            return navHit.position;
      }


}
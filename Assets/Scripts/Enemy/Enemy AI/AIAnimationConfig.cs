using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class AIAnimationConfig : MonoBehaviour
{
    [SerializeField] float slerpSpeed = 2.5f;
    Animator anim;
    NavMeshAgent agent;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnAnimatorMove()
    {
        agent.updatePosition = true;
        agent.updateRotation = false;
        agent.velocity = anim.deltaPosition / Time.deltaTime;

        Quaternion newRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, slerpSpeed * Time.deltaTime);
    }

}

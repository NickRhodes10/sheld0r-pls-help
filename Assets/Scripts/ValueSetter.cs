using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSetter : StateMachineBehaviour
{
    [Header("On Enter")]
    public BoolVariable[] boolsOnEnter;
    public FloatVariable[] floatsOnEnter;

    [Header("On Update")]
    public BoolVariable[] boolsOnUpdate;
    public FloatVariable[] floatsOnUpdate;

    [Header("On Exit")]
    public BoolVariable[] boolsOnExit;
    public FloatVariable[] floatsOnExit;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < boolsOnEnter.Length; i++)
        {
            animator.SetBool(boolsOnEnter[i].name, boolsOnEnter[i].value);
        }

        for (int i = 0; i < floatsOnEnter.Length; i++)
        {
            animator.SetFloat(floatsOnEnter[i].name, floatsOnEnter[i].value);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < boolsOnUpdate.Length; i++)
        {
            animator.SetBool(boolsOnUpdate[i].name, boolsOnUpdate[i].value);
        }

        for (int i = 0; i < floatsOnUpdate.Length; i++)
        {
            animator.SetFloat(floatsOnUpdate[i].name, floatsOnUpdate[i].value);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < boolsOnExit.Length; i++)
        {
            animator.SetBool(boolsOnExit[i].name, boolsOnExit[i].value);
        }

        for (int i = 0; i < floatsOnExit.Length; i++)
        {
            animator.SetFloat(floatsOnExit[i].name, floatsOnExit[i].value);
        }
    }

    #region
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    #endregion
}

public abstract class VariableBase
{
    public string name;
}

[System.Serializable]
public class BoolVariable : VariableBase
{
    public bool value;
}

[System.Serializable]
public class FloatVariable : VariableBase
{
    public float value;
}

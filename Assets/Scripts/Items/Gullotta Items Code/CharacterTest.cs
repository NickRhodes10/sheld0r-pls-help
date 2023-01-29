using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class CharacterTest : MonoBehaviour
{
    public UseItem useItem;
    public static CharacterTest instance;
    public bool Using;
    public Animator anim;
    public AnimationType animationType;

    public AnimationType GetType { get { return animationType; } set { animationType = value; } }
    public enum AnimationType
    {
        Fireball,
        Boomerang,
        Bow,
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Using == false && anim.GetCurrentAnimatorStateInfo(2).IsName("Empty") == true && animationType == AnimationType.Fireball)
        {
            anim.Play("Fireball", 2);
            Using = true;
        }

        if (Input.GetMouseButtonDown(1) && Using == false && anim.GetCurrentAnimatorStateInfo(1).IsName("Empty (Not Dodging)") == true && animationType == AnimationType.Boomerang)
        {
            anim.Play("Boomerang", 1);
            Using = true;
        }

        if (Input.GetMouseButtonDown(1) && Using == false && anim.GetCurrentAnimatorStateInfo(1).IsName("Empty (Not Dodging)") == true && animationType == AnimationType.Bow)
        {
            anim.Play("Bow", 2);
            Using = true;
        }

    }



    public void UseItem()
    {
        useItem.OnUseItem(transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class PlayerMoveScript : MonoBehaviour
{

public float PLAYERSPEED = 0.05f;
public float STOPDASHTIME = 0.5f;

public float DASHSPEEDMULTIPLIER = 5f;
public float DASHCOOLDOWN = 3f;
public float DASHstaminaDamage = 10f;

public float STAMINAREGENWAIT = 3f;

public Rigidbody rb;

public bool isDashing;

public LayerMask Ground;

public GameObject PlayerBody;
public GameObject mouseLocation;
public GameObject PlayerParent;
public float velocity;
public Transform CameraPosition;

public CharacterController controller;

private float gravity = -9.81f;

private PlayerManager _pm;
private bool CanDash = true;
private Animator anim;
private Vector3 gravityVelocity;
private Vector3 PlayerVect;
private Vector3 orientation;
private float _dashTimeCheck;



// Start is called before the first frame update
void Start()
{
    //_pm = GetComponentInParent<PlayerManager>();
    rb = GetComponent<Rigidbody>();
    anim = GetComponent<Animator>();
    controller = GetComponentInParent<CharacterController>();
}

// Update is called once per frame
void Update()
{
    velocity = controller.velocity.magnitude;
    //Adds Gravity
    gravityVelocity.y += gravity * Time.deltaTime;
    controller.Move(gravityVelocity * Time.deltaTime);;


    Move();
    Dash();
    AttackCombo();

}
//Top Down Camera
private void FixedUpdate()
{
    //Casts ray to mouse position
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, Ground))
    {
        //Raycasts to get point on Ground
        mouseLocation.transform.position = ray.GetPoint(hit.distance);
        //Draws ray to location
        Debug.DrawRay(ray.origin, ray.direction * 1000);
        //Make sure there is a playerbody set
        if (PlayerBody != null)
        { 
            //Player look at mouseLocation
             PlayerBody.transform.LookAt((mouseLocation.transform.position + new Vector3(0f,0.5f,0f)), Vector3.up * 0.05f * Time.deltaTime);
        }
    }
}

//Player Attacking Animation
private void AttackCombo()
{
    //If left click
    if (Input.GetMouseButton(0))
    {
        //Set attack trigger
        anim.SetTrigger("Attack");
    }
    else { anim.ResetTrigger("Attack"); }

    //ONLY FOR 1H SWORD
    //If current state's name is combo
    if (anim.GetCurrentAnimatorStateInfo(1).IsName("combo"))
    {
        //and if left click again during combo
        if (Input.GetMouseButton(0))
        {
            //Set bool and trigger to perform second attack (if animation)
            anim.SetBool("Attacking2", true);
            anim.SetTrigger("Attack2");
        }
    }
    else
    {
        //Reset
        anim.ResetTrigger("Attack2");
        anim.SetBool("Attacking2", false);
    }
}
//Player Movement WASD
private void Move()
{
    //Gets Input
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");
    PlayerVect = new Vector3(0, 0, 0);

    //Checks if player is dashing
    if (isDashing == false)
    {
        //Gets Player control input ( A and D), moves player horizontally
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //Moves Player left and right
            PlayerVect = PlayerParent.transform.right * x;
            controller.Move(PlayerVect * PLAYERSPEED);
        }

        //Gets Player control input ( W and S), moves player vertically
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            //Moves Player forward and back based on stationary parent transform
            PlayerVect = PlayerParent.transform.forward * z;
            controller.Move(PlayerVect * PLAYERSPEED);
        }
    }

    //Orientation of the player
    orientation = mouseLocation.transform.position - gameObject.transform.position;
    orientation = orientation.normalized;

    //If z orientation is within range
    if (orientation.z > 0.5 || orientation.z < -0.5)
    {
        //Sets character animations
        anim.SetFloat("veloX", x);
        anim.SetFloat("veloY", z);
    }

    //If z orientation is within range, flip animator values
    if (orientation.z < 0.5 || orientation.z > -0.5)
    {
        //Sets flipped input to blend tree;
        anim.SetFloat("veloY", x, 0.2f, Time.deltaTime);
        anim.SetFloat("veloX", z, 0.2f, Time.deltaTime);
    }
}

private void Dash()
{
    //Checks for player's Input, if the player is dashing, and if player is able to dash
    if(CanDash == true && _pm.CurrentStamina >= DASHstaminaDamage && isDashing == false)
    {
        if (Input.GetMouseButtonDown(1))
        {
            _pm.stamRegen = false;
            _dashTimeCheck = Time.time + STAMINAREGENWAIT;
            isDashing = true;

            _pm.PlayerTakeStaminaDamage(DASHstaminaDamage);
            StartCoroutine(StopDashing(STOPDASHTIME));
        }
        //checks to see if check time is set and compares it to current time.time
        if (_dashTimeCheck != 0 && Time.time > _dashTimeCheck)
        {
            //if time passed then regen stamina and set dash check to 0
            _pm.stamRegen = true;
            _dashTimeCheck = 0;
        }
    }

    if (isDashing == true)
    {
        //Moves controller and camera
        anim.Play("Dodge", 0);
        controller.Move(orientation * PLAYERSPEED * DASHSPEEDMULTIPLIER);
    }

    //checks if player is out of stamina and waits for dash check to start regenerating
    if(_pm.CurrentStamina <= DASHstaminaDamage && Time.time > _dashTimeCheck)
    {
        _pm.stamRegen = true;
        _dashTimeCheck = 0;
    }
}

//Stops dashing and calls cooldown coroutine
private IEnumerator StopDashing(float dashTime)
{
    yield return new WaitForSeconds(dashTime);

    StartCoroutine(DashCooldown(DASHCOOLDOWN));
    isDashing = false;
    CanDash = false;
}

//Wait cooldown time to dash again
private IEnumerator DashCooldown(float cooldownTime)
{
    yield return new WaitForSeconds(cooldownTime);

    CanDash = true;
}


}

*/

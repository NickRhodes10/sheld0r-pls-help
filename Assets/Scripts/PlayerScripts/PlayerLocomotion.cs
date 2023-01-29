using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private InputHandler input;
    public Animator anim;

    public LayerMask Ground; // Used for the raycast that finds mouse location

    [SerializeField] public Camera cam; // Camera Reference
    [SerializeField] public bool rotateTowardsMouse; // Enable or Disable Rotation Based on Mouse Input.

    [Header("Boolean States")]
    [SerializeField] public bool isDodging; // Boolean controling whether or not the player is actively dodging
    [SerializeField] public bool readyToDodge; // Whether or not the player is allowed to dodge
    [SerializeField] public bool isAttacking; // Boolean controling whether or not the player is actively attacking

    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed; // Player Move Speed
    [SerializeField] public float rotateSpeed; // Player Rotate Speed

    [Header("Dodge Settings")]
    [SerializeField] public float dodgeSpeedMultiplier; // Dodge Speed
    [SerializeField] public float dodgeLengthTime; // Length of time the dodge will last
    [SerializeField] public float dodgeCooldown; // Length of time between dodges

    private float tempDodgeTime; // Copies the dodgeLengthTime and subtracts Time.deltaTime while rolling
    private bool hitWall = false; // Used to detect if a wall was hit while rolling and, if so, the desired result
    private bool attemptingMovement; // Used to measure if the targetVector is high enough in any direction to allow a dodge to occur    

    public bool canMove = true;

    /*
    public enum PlayerState // List of states the player is able to be in, add to it as we add more functionality to the player
    {
        Moving,
        Dodging,
        Attacking
    }
    public PlayerState state; // Controls the state the player is in
    */


    private void Awake()
    {
        input = GetComponent<InputHandler>(); // Grab input Component and assign to variable.
        anim = GetComponentInChildren<Animator>(); // Grab animator and assign that shit.

    }

    private void Start()
    {
        readyToDodge = true; // Allows the player to dodge for the first time
        anim.SetBool("canDodge", true); // Same bool, but for the animator
    }

    void Update()
    {
        if (canMove == true)
        {
            MyInput(); // Always holds the targetVector (combined hor. and vert. inputs) and handles the inputs of different abilities
        }
    }

    private void MyInput()
    {
        var targetVector = new Vector3(input.inputVector.x, 0, input.inputVector.y); // Create Target Vector based on our input vector from InputHandler script.

        //  -- Dodging Input --
        #region 
        if (targetVector.x > 0.08f || targetVector.x < -0.08f || targetVector.z > 0.08f || targetVector.z < -0.08f) // Sets the value of attemptingMovement based on if the player is actually moving
            attemptingMovement = true;
        else
            attemptingMovement = false;

        // Calls Dodge() using the combined hor. and vert. inputs once per dodge attempt,
        // and will end the dodge after either a set amount of time or if a wall is hit during the dodge
        if (input.dodgeKey && readyToDodge && !isDodging && anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion") && attemptingMovement)
        {
            readyToDodge = false;
            Dodge(targetVector);
        }
        // Calls Dodge() each subsequent frame until the dodge is finished executing
        else if (isDodging)
        {
            Dodge(targetVector);
        }
        #endregion
        
        // -- Movement Input --
        else if (!isDodging) // Dodging is meant to prevent all other movement and player abilities from use until it is complete
        {
            Movement(targetVector); 
        }

        // -- Attacking Bool -- (just used for debuging, attacking is handled in SwordCombat, and attacks are cancelled and prevented while dodging using the Animator)
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack 1") || anim.GetCurrentAnimatorStateInfo(1).IsName("Attack 2") || anim.GetCurrentAnimatorStateInfo(1).IsName("Attack 3"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    /* 
    private void StateHandler()
    {
        // Mode - Dodging
        if (isDodging)
        {
            state = PlayerState.Dodging;
        }

        // Mode - Attacking
        if (isAttacking && !isDodging)
        {
            state = PlayerState.Attacking;
        }

        // Mode - Moving
        else if (!isDodging && !isAttacking)
        {
            state = PlayerState.Moving;
        }
    }
    */

    private void Movement(Vector3 targetVector)
    {
        var movementVector = MoveTowardTarget(targetVector);                                               // Generate movementVector by calling MoveTowardTarget which returns a movementVector.

        if (!rotateTowardsMouse)                                                                           // If we are not rotating with mouse,
            RotateTowardMovementVector(movementVector);                                                    // Rotate manually.
        else
            RotateTowardMouseVector(movementVector);                                                       // Rotate with mouse.

        CalculateAnimation(movementVector);                                                                // Calculates the blend tree based on movementVector and mouse position
    }

    // -- Dodging Code --
    #region
    private void Dodge(Vector3 targetVector)
    {
        if (!isDodging)                                                                                    // Because Dodge() is an update function, this if statement will only activate on the first frame
        {
            float animSpeedMulti = 1.867f / dodgeLengthTime;                                               // Sets the length of the dodge animation to roughly equal desired length of the dodge
            anim.SetFloat("dodgeAnimSpeed", animSpeedMulti - 0.5f);                                        // Takes a little off the top of the animation speed to make the transition out a bit smoother       

            tempDodgeTime = dodgeLengthTime;                                                               // Copies the desired length of dodge

            var rotation = Quaternion.LookRotation(targetVector);                                          // Assigns the targetVector (combined hor. and vert. inputs) to a rotation variable
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * 50); // Snaps the rotation of the player to that variable's value      
        }

        isDodging = true;                                                                                  // Prevents the previous code from running more than once per dodge
        anim.SetBool("dodging", true);                                                                     // Transitions to the dodging animation
                                                                                                           // Prevents another dodge anim transition from locomotion until both values are true again

        if (tempDodgeTime > 0)
        {
            tempDodgeTime -= Time.deltaTime;                                                               // Count down the copied length of the dodge every frame

            if (!hitWall)                                                                                  // If the player HAS NOT hit a wall yet...
            {
                var movementVector = DodgeTowardTarget(targetVector);                                      // Move the player using DodgeTowardTarget()
            }
            else if (hitWall)                                                                              // If the player HAS hit a wall...
            {
                anim.SetBool("hitWall", true);                                                             // Stop the dodge animation while keeping the player in a dodging state
            }
        }
        else                                                                                               // If the timer runs out regardless of hitting the wall or not...
        {
            EndDodge();                                                                                    // End the dodge.
        }

        // Old rolling code I left in case I need it. Will delete at build time if still unused //
        #region
        //if (!isDodging)
        //{
        //    startTime = Time.time;

        //    var rotation = Quaternion.LookRotation(targetVector);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * 1000);

        //    // Next, find the distance from the player to where they will end up at the end of the dodge
        //    // by multiplying targetVector by speed and time set in inspector
        //    dodgeSpeed = moveSpeed * dodgeSpeedMultiplier;
        //    var time = dodgeLengthTime;
        //    targetVector = Vector3.Normalize(targetVector);

        //    //dodgeFinalPosition = transform.position + targetVector * dodgeSpeed;
        //    dodgeFinalDistance = dodgeSpeed * time;
        //    dodgeFinalPosition = transform.position + targetVector * dodgeSpeed * time;
        //    //dodgeFinalDistance = Vector3.Distance(transform.position, dodgeFinalPosition);
        //    Debug.Log("Final Distance: " + dodgeFinalDistance);

        //    anim.SetFloat("dodgeAnimSpeed", dodgeLengthTime * 1.867f + 0.1f);
        //    anim.SetTrigger("Dodging");
        //}

        //isDodging = true;        

        //float distCovered = (Time.time - startTime) * dodgeSpeed;
        //float fractionOfDodge = distCovered / dodgeFinalDistance;
        //Debug.Log("frac of Dodge: " + fractionOfDodge);
        //transform.position = Vector3.Lerp(transform.position, dodgeFinalPosition, fractionOfDodge);
        #endregion
    }

    private Vector3 DodgeTowardTarget(Vector3 targetVector)                                                // This code is nearly identical to MoveTowardTarget, just with a predetermined rotation
    {
        var speed = moveSpeed * dodgeSpeedMultiplier * Time.deltaTime;                                     // Sets the speed of the dodge to moveSpeed x dodgeSpeedMultiplier

        targetVector = Vector3.Normalize(targetVector);                                                    // Sets the combined hor. and vert. targetVector to a magnitude of one
        var targetPosition = transform.position + targetVector * speed;                                    // Sets the target position of the dodge
        transform.position = targetPosition;                                                               // "Moves" the player to the target position by just like putting them there, lil by lil idk I'm getting sleepy
        return targetVector;                                                                               // returns the targetVector
    }

    private void OnTriggerEnter(Collider other) // Detects if the smaller capsule collider on the "Player" object touches a Wall object and
    {                                           // stops the roll animation while keeping the player in a dodging state which prevents spam
        if (isDodging)
        {
            if (other.tag == "Wall")
            {
                hitWall = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isDodging)
        {
            if (other.tag == "Wall")
            {
                hitWall = true;
            }
        }
    }

    private void EndDodge() // When EndDodge() is called, every bool allowing dodging is set to false, stopping the dodge no matter what
    {                       // and ResetDodge() is invoked after dodgeCooldown is elapsed
        isDodging = false;
        readyToDodge = false;
        anim.SetBool("canDodge", false);
        anim.SetBool("hitWall", false);
        anim.SetBool("dodging", false);
        Invoke(nameof(ResetDodge), dodgeCooldown);
    }

    private void ResetDodge() // Self-explanitory
    {
        hitWall = false;
        readyToDodge = true;
        anim.SetBool("canDodge", true);
    }
    #endregion

    // -- Moving Code --
    #region
    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime; // Set speed scaled by Time.deltaTime.

        targetVector = Quaternion.Euler(0, cam.gameObject.transform.eulerAngles.y, 0) * targetVector; // Create target rotation vector using euler angles and multiply by our targetVector.
        targetVector = Vector3.Normalize(targetVector);
        var targetPosition = transform.position + targetVector * speed; // targetPosition is where we want to be and at what speed we want to get there.
        transform.position = targetPosition; // set our transform to the targetPosition.
        return targetVector; // Return our movement vector.
    }

    // Rotate with Mouse Function
    private void RotateTowardMouseVector(Vector3 movementVector)
    {
        Ray ray = cam.ScreenPointToRay(input.mousePosition); // Generate raycast from camera to terrain.

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f, Ground)) // Limit distance and grab hitInfo from whatever the ray hits.
        {
            var target = hitInfo.point; // Assign target varable to the point at which hitInfo intersected with ground.
            target.y = transform.position.y; // Lock target's y position to player y position in order to prevent awkward backwards lean near a wall or object.
            transform.LookAt(target); // Rotate toward target.
        }
    }

    // Rotate without Mouse Function
    private void RotateTowardMovementVector(Vector3 movementVector)
    {
        if (movementVector.magnitude == 0) { return; } // If player is not moving, return. We do not want to rotate when we are not moving.

        var rotation = Quaternion.LookRotation(movementVector); // Calculate rotation degree
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed); // Rotate player towards movement direction at a declared rotation speed.
    }

    private void CalculateAnimation(Vector3 movementVector)
    {
        Ray ray = cam.ScreenPointToRay(input.mousePosition); // New ray from camera using mousePosition from InputHandler script

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f, Ground)) // If the ray hits a collider within 300 units with the Ground layermask
        {
            Vector3 target = hitInfo.point; // target created from the position of the RaycastHit
            Vector3 orientation = target - transform.position; // orientation gets the difference from target to transform positions
            orientation = orientation.normalized; // Normalizes this Vector3

            //If z orientation is within range
            if (orientation.z > 0.5 || orientation.z < -0.5)
            {
                //Sets character animations
                anim.SetFloat("veloX", input.inputVector.x, 0.2f, Time.deltaTime);
                anim.SetFloat("veloY", input.inputVector.y, 0.2f, Time.deltaTime);
            }

            //If z orientation is within range, flip animator values
            if (orientation.z < 0.5 && orientation.z > -0.5)
            {
                //Sets flipped input to blend tree;
                anim.SetFloat("veloY", input.inputVector.x, 0.2f, Time.deltaTime);
                anim.SetFloat("veloX", input.inputVector.y, 0.2f, Time.deltaTime);
            }
        }
    }
    #endregion
}

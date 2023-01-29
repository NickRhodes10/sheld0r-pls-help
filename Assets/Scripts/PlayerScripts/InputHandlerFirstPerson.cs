using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerFirstPerson : MonoBehaviour
{
    public static InputHandlerFirstPerson instance;

    // Mostly identical to the original Input Handler, but with unused features stripped away for now.

    public Vector2 inputVector { get; private set; }
    public bool swordKey { get; private set; }
    public bool sprintKey { get; private set; }
    public bool openMenuKey { get; private set; }
    public bool escapeKey { get; private set; }

    private void Awake()
    {
        if (InputHandlerFirstPerson.instance == null)
            InputHandlerFirstPerson.instance = this;
        else if (InputHandlerFirstPerson.instance != this)
            Destroy(this);
    }

    void Update()
    {
        // Movement
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        inputVector = new Vector2(h, v);

        // Sword Swing
        swordKey = Input.GetMouseButtonDown(0);

        // Sprinting
        sprintKey = Input.GetKey(KeyCode.LeftShift);

        // Open Menu
        openMenuKey = Input.GetKeyDown(KeyCode.Tab);

        // Escape
        escapeKey = Input.GetKeyDown(KeyCode.Escape);
    }
}

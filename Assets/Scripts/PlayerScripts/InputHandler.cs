using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public Vector2 inputVector { get; private set; }
    public Vector3 mousePosition { get; private set; }
    public bool dodgeKey { get; private set; }
    public bool swordKey { get; private set; }
    public bool testKey { get; private set; }

    public ZoomSwitch zs;

    // Update is called once per frame
    void Update()
    {
        // Movement
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        inputVector = new Vector2(h, v);

        mousePosition = Input.mousePosition;

        // Camera Zoom
        if (Input.GetKeyDown(KeyCode.Z))
            zs.SwitchState();

        // Dash
        dodgeKey = Input.GetKeyDown(KeyCode.Space);

        // Sword Swing
        swordKey = Input.GetMouseButtonDown(0);

        // Testing
        testKey = Input.GetKeyDown(KeyCode.I);
    }
}

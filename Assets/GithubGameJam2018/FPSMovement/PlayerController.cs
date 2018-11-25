using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Motor))]
[RequireComponent(typeof(ZeroG))]
/*
 * This is the class for the controller for the player. It requires a motor (which does
 * regular base movement on the ground), a ZeroG component (for hovering / flying) and a rigidbody for all
 * physics calculations. Here is where we assign the keys that controls our game component.
 */
public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Motor motor;
    public float jumpSpeed;
    public float movementSpeed;
    private bool isGrounded;

    private ZeroG zeroG;
    //number of jumps allowed
    //TODO - implement
    public int numberOfJumps;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        zeroG = GetComponent<ZeroG>();
        motor = GetComponent<Motor>();

    }

    
    void OnCollisionStay (Collision collision) {
        isGrounded = true;
    }

    //FixedUpdate - Physics
    void FixedUpdate()
    {
        //jump
        if (Input.GetButton("Jump") && isGrounded)
        {
            motor.Jump(jumpSpeed);
            isGrounded = false;
        }
        //start zero G
        if (Input.GetKeyDown(KeyCode.E))
        {
            zeroG.MoveInZeroG();
        }
        //stop zero g (fall)
        if (Input.GetKeyDown(KeyCode.F))
        {
            zeroG.Reset();
        }
        //all movement that's not in ZeroG.
        if (!zeroG.inZeroG)
        {
            motor.Move(movementSpeed);
        }
    }

}

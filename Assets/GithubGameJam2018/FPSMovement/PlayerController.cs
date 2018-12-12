using System;
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

    //Notable events
    public Action OnEnterZeroG;
        //end



    private Rigidbody rb;
    private Motor motor;
    public float jumpSpeed;
    public float movementSpeed;


    private ZeroG zeroG;
    //number of jumps allowed
    //TODO - implement
    public int numberOfJumps;
    private int currentJumps;
    private bool canJump;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        canJump = false;
        zeroG = GetComponent<ZeroG>();
        motor = GetComponent<Motor>();
        currentJumps = 0;

    }

    
    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Floor")){
            canJump = true;
            currentJumps = 0;
        }
    }

    //FixedUpdate - Physics
    void FixedUpdate()
    {
        if (currentJumps >= numberOfJumps)
        {
            canJump = false;
        }
        //jump
        if (Input.GetButtonDown("Jump") && canJump)
        {
            currentJumps += 1;
            motor.Jump(jumpSpeed);
            if (currentJumps >= numberOfJumps)
            {
                canJump = false;
            }
        }
        //start zero G
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(OnEnterZeroG != null)
            {
                OnEnterZeroG.Invoke();

                //on enter zero G
            }

            if (zeroG.canZeroG)
            {
                zeroG.MoveInZeroG();
            }
        }
        //all movement that's not in ZeroG.
        if (!zeroG.inZeroG)
        {
            motor.Move(movementSpeed);
        }
    }

}

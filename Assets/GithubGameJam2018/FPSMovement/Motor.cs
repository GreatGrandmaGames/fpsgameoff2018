using System;
using System.Collections.Generic;
using UnityEngine;
/*
 * Vroom Vroom, welcome to the motor
 * This does the positional movement. Pretty basic stuff, except we're using rb.MovePosition
 */
public class Motor : MonoBehaviour {

    //Notable events
    public Action OnPlayerWalk;
    public Action OnPlayerJump;
    //end


    private Rigidbody rb;
    private Vector3 velocity;
    private ZeroG zerog;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        zerog = GetComponent<ZeroG>();
	}

    private void FixedUpdate()
    {
        //this isn't elegant and I'm sorry about it
        if (!zerog.inZeroG)
        {
            DoMovement(velocity);
        }

    }

    //movey movey
    public void Move(float movementSpeed)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 moveHorizontal = transform.right * x;
        Vector3 moveVertical = transform.forward * z;

        //calculate velocity
        velocity = (moveHorizontal + moveVertical).normalized * movementSpeed;
    } 
    private void DoMovement(Vector3 velocityp)
    {
        //maybe we should have velocity equal to zero? unclear tho haven't bug tested enough
        if (velocity != Vector3.zero){

            if(OnPlayerWalk != null){
                OnPlayerWalk();

                //On player move code here

            //nb called every fixed update
            }


            rb.MovePosition(rb.position + velocityp * Time.deltaTime);
        }
    }

    //jump function, just adds a vertical impulse force.
    public void Jump(float jumpSpeed)
    {

        if (OnPlayerJump != null)
        {
            OnPlayerJump();

            //On player move code here

            //nb called every fixed update
        }

        rb.AddForce(new Vector3(0f, jumpSpeed, 0f), ForceMode.Impulse);
    }
}

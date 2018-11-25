using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the Camera based on rotation
public class CameraController : MonoBehaviour {

    private Rigidbody rb;
    //the scale at which the speed of the camera moves
    public float lookSensitivity = 2.0f;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
    public void Update () {
        //brackeys code, basically calculates rotation based on the x / y mouse axis
        float xRot = Input.GetAxisRaw("Mouse X");
        Vector3 xRotation = new Vector3(0f, xRot, 0f) * lookSensitivity;
        rb.MoveRotation(transform.rotation * Quaternion.Euler(xRotation));

        float yRot = Input.GetAxisRaw("Mouse Y");
        Vector3 yRotation = new Vector3(yRot, 0f, 0f) * lookSensitivity;

        //applies it (if we have settings in the future, we can do inverted here by - yRotatoin).
        Camera.main.transform.Rotate(-yRotation); 
    }
}

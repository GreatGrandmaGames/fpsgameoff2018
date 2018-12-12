using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWheel : MonoBehaviour {

    private Transform[] gameObjects;
	// Use this for initialization
	void Start () {
        gameObjects = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Debug.Log(gameObjects[i]);
        }

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameObjects[2].gameObject.SetActive(true);
            gameObjects[1].gameObject.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameObjects[1].gameObject.SetActive(true);
            gameObjects[2].gameObject.SetActive(false);
        }
    }
}

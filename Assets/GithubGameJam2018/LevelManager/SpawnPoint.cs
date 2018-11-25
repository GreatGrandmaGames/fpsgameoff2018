using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public GameObject prefab;

    private GameObject currentSpawned;

    public GameObject Spawn()
    {
        if(prefab == null)
        {
            return null;
        }

        if(currentSpawned == null)
        {
            currentSpawned = Instantiate(prefab, transform.position, transform.rotation);
        }

        return currentSpawned;
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {

        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}

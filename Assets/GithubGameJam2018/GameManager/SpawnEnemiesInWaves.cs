using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnEnemiesInWaves : MonoBehaviour
{
    public TextMeshProUGUI nextWaveDescription;
    public Damageable player;
    public enum SpawnState
	{
		SPAWNING,
		WAITING,
		COUNTING
	};
	
	[System.Serializable]
	public class Wave
	{
		public string name;
		public GameObject enemy;
		public int count;
		public float rate;
        public Transform position;
        public string nextWaveDescription;
	}

	public Wave[] waves;
    [HideInInspector]
	public int nextWave = 0;

	public float timeBetweenWaves = 5f;

	public float waveCountdown;

	private float searchCountdown = 1f;

    private bool start;
    //private GameObject Manager;
    [HideInInspector]
	public SpawnState state = SpawnState.COUNTING;
	// Use this for initialization
	void Start ()
	{
		waveCountdown = timeBetweenWaves;
        start = false;
        //Manager = GameObject.FindGameObjectWithTag("Manager");

    }
	
	// Update is called once per frame
	void Update () 
	{
		//kill all, then switch wave
        if (start)
        {
            nextWaveDescription.text = "3 seconds until wave starts";
        }
		if (state == SpawnState.WAITING)
		{
            if (!EnemyIsAlive())
			{
				WaveCompleted();
				return;
			}
			else
			{
				return;
			}
		}
		
		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
                nextWaveDescription.enabled = false;
                StartCoroutine(SpawnWave(waves[nextWave]));
                start = false;

			}
		}
		else
		{
            waveCountdown -= Time.unscaledDeltaTime;
        }
	}

	void WaveCompleted()
	{

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;
        nextWaveDescription.enabled = true;
        nextWaveDescription.text = waves[nextWave].nextWaveDescription;
        nextWave += 1;
		//wtf brackeys
		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			Debug.Log("All Waves Completed");
		}
		Debug.Log("Wave Completed");

	}

	bool EnemyIsAlive()
	{
        searchCountdown -= Time.unscaledDeltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}
		
	IEnumerator SpawnWave(Wave wave)
	{
		Debug.Log("Spawning Wave" + wave.name);
		state = SpawnState.SPAWNING;

		for (int i = 0; i < wave.count; i++)
		{
            SpawnEnemy(wave.enemy, wave.position.position);
			yield return new WaitForSeconds(1f / wave.rate);
		}

		//now we're waiting for more enemies
		state = SpawnState.WAITING;
		yield break;
	}

	void SpawnEnemy(GameObject enemy, Vector3 position)
	{

		GameObject e = Instantiate(enemy, position, transform.rotation);
        e.GetComponent<PF_AI>().SetTarget(player);
	}
}
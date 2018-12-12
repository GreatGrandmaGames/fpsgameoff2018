using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnEnemiesInWaves : MonoBehaviour
{
    //Notable Events:
    public Action OnWaveStart;
    public Action OnWaveComplete;
    //end


    #region Data Classes
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
        public List<EnemyCluster> clusters;
	}

    [System.Serializable]
    public class EnemyCluster
    {
        public PF_AI enemyPrefab;
        public int count;
        public Transform spawnPoint;
        public float rate;
    }
    #endregion

    //References
    public Damageable player;

    //Variables - Modify me in the inspector
    public int startingWave;
    public float timeBetweenWaves = 5f;

    [SerializeField]
	public Wave[] waves;

    //Bookkeeping
    private List<PF_AI> enemies = new List<PF_AI>();

    public int WaveCount { get; private set; }

	private SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        StartCoroutine(StartNextWave(0f));
    }

    bool EnemyIsAlive()
	{
        return enemies.Count > 0;
	}

    IEnumerator StartNextWave(float delay)
    {
        Debug.Log("Starting Wave: " + WaveCount);

        yield return new WaitForSeconds(delay);

        if(OnWaveStart != null)
        {
            OnWaveStart.Invoke();

            //Wave start code here
        }

        Wave wave = waves[WaveCount];


        state = SpawnState.SPAWNING;

        int completeCount = wave.clusters.Count;

        foreach(var ec in wave.clusters)
        {
            StartCoroutine(SpawnCluster(ec, () =>
            {
                completeCount--;

                if(completeCount <= 0)
                {
                    state = SpawnState.WAITING;
                }
            }));
        }

        StartCoroutine(ListenForEndOfWave());
    }

    IEnumerator ListenForEndOfWave()
    {
        while (EnemyIsAlive() || state == SpawnState.SPAWNING)
        {
            yield return null;
        }

        state = SpawnState.COUNTING;

        Debug.Log("Wave: " + WaveCount + " Complete" );

        if (OnWaveComplete != null)
        {
            OnWaveComplete.Invoke();

            //Wave complete code here
        }

        WaveCount = (WaveCount + 1) % waves.Length;

        StartCoroutine(StartNextWave(timeBetweenWaves));
    }

    IEnumerator SpawnCluster(EnemyCluster cluster, Action onComplete)
    {
        for (int i = 0; i < cluster.count; i++)
        {
            SpawnEnemy(cluster.enemyPrefab, cluster.spawnPoint);
            yield return new WaitForSeconds(1f / cluster.rate);
        }

        if (onComplete != null)
        {
            onComplete();
        }
    }

    void SpawnEnemy(PF_AI enemy, Transform t)
	{
        Debug.Log("Spawning Enemy at " + t.position);

		PF_AI e = Instantiate(enemy, t.position, t.rotation);
        e.SetTarget(player);

        enemies.Add(e);

        e.GetComponent<Damageable>().OnDestroyed += () =>
        {
            enemies.Remove(e);
        };
	}
}
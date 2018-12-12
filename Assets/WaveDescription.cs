using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveDescription : MonoBehaviour {

    public SpawnEnemiesInWaves waveManager;
    public TextMeshProUGUI tmp;

    private void Awake()
    {
        waveManager.OnWaveStart += () =>
        {
            tmp.text = "Starting Wave " + waveManager.WaveCount;
        };

        waveManager.OnWaveComplete += () =>
        {
            tmp.text = "Wave " + waveManager.WaveCount + " Complete";
        };
    }
}

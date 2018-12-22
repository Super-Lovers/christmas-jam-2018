using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour {
    public Text StageText;
    public GameObject WaveNotification;
    public int SecondsBetweenWaves;
    public GameObject[] Wave1;
    public GameObject[] Wave2;
    public GameObject[] Wave3;
    public static int CurrentStage = 1;
    public static int CurrentWave = 1;
    public static bool IsEnemyDefeated = false;

    private int _secondsLeft = 0;

    private void Start()
    {
        _secondsLeft = SecondsBetweenWaves;
        StartNewWave();
    }

    private void Update()
    {
        // We must only go over waves that exist [1-3]
        if (CurrentStage == 1 && IsEnemyDefeated && CurrentWave < 4)
        {
            StartNewWave();
            IsEnemyDefeated = false;
        }

        if (CurrentStage == 2 && IsEnemyDefeated && CurrentWave < 4)
        {
            StageText.text = "Stage " + CurrentStage + "-" + CurrentWave;
        }
    }

    public void StartNewWave()
    {
        _secondsLeft = SecondsBetweenWaves;
        InvokeRepeating("CountdownToNextWave", 0, 1);
    }

    private void CountdownToNextWave()
    {
        _secondsLeft--;
        if (_secondsLeft == 0)
        {
            StartCoroutine(FlashNotification());

            Invoke("PickNewWave", 4f);
        }
    }

    private IEnumerator FlashNotification()
    {
        for (int i = 0; i < 3; i++)
        {
            WaveNotification.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            WaveNotification.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            WaveNotification.SetActive(false);
        }
    }

    private void PickNewWave()
    {
        switch (CurrentWave)
        {
            case 1: SpawnNewWave(Wave1); break;
            case 2: SpawnNewWave(Wave2); break;
            case 3: SpawnNewWave(Wave3); break;
        }

        StageText.text = "Stage " + CurrentStage + "-" + CurrentWave; 
    }

    private void SpawnNewWave(GameObject[] wave)
    {
        foreach (GameObject enemy in wave)
        {
            Instantiate(enemy);
        }
    }
}

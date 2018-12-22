using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour {
    public Text StageText;
    public GameObject WaveNotification;
    public int SecondsBetweenWaves;
    public string StageOne = "This is the first stages' waves";
    public GameObject[] Wave1_1;
    public GameObject[] Wave1_2;
    public GameObject[] Wave1_3;
    public string StageTwo = "This is the second stages' waves";
    public GameObject[] Wave2_1;
    public GameObject[] Wave2_2;
    public GameObject[] Wave2_3;
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
            StartNewWave();
            IsEnemyDefeated = false;
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
        if (StageController.CurrentStage == 1)
        {
            switch (CurrentWave)
            {
                case 1: SpawnNewWave(Wave1_1); break;
                case 2: SpawnNewWave(Wave1_2); break;
                case 3: SpawnNewWave(Wave1_3); break;
            }
        } else if (StageController.CurrentStage == 2)
        {
            switch (CurrentWave)
            {
                case 1: SpawnNewWave(Wave2_1); break;
                case 2: SpawnNewWave(Wave2_2); break;
                case 3: SpawnNewWave(Wave2_3); break;
            }
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

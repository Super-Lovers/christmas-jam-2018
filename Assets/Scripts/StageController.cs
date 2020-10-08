using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour {
    public Text StageText;
    public GameObject WaveNotification;
    public AudioClip ContinueSound;
    public int SecondsBetweenWaves;
    public string StageOne = "This is the first stages' waves";
    public GameObject[] Wave1_1;
    public GameObject[] Wave1_2;
    public GameObject[] Wave1_3;
    public string StageTwo = "This is the second stages' waves";
    public GameObject[] Wave2_1;
    public GameObject[] Wave2_1_SpawnPoints;
    public GameObject[] Wave2_2;
    public GameObject[] Wave2_2_SpawnPoints;
    public GameObject[] Wave2_3;
    public GameObject[] Wave2_3_SpawnPoints;
    public string StageThree = "This is the last stages' waves";
    public GameObject[] Wave3_1;
    public GameObject[] Wave3_1_SpawnPoints;
    public GameObject[] Wave3_2;
    public GameObject[] Wave3_2_SpawnPoints;
    public GameObject[] Wave3_3;
    public GameObject[] Wave3_3_SpawnPoints;
    public static int CurrentStage = 1;
    public static int CurrentWave = 1;
    public static bool IsEnemyDefeated = false;
    public static int EnemiesCurrentlyAlive = 0;
    public static bool IsPlayerSpeedUpdated = false;

    private int _secondsLeft = 0;

    private void Start()
    {
        _secondsLeft = SecondsBetweenWaves;
        StartNewWave();
    }

    private void Update()
    {
        if (CurrentStage != 1 && IsPlayerSpeedUpdated == false)
        {
            Invoke("UpdatePlayerSpeed", 1f);
            IsPlayerSpeedUpdated = true;
        }

        // We must only go over waves that exist [1-3]
        if (CurrentStage == 1 && IsEnemyDefeated && CurrentWave < 4)
        {
            StartNewWave();
            IsEnemyDefeated = false;
        }

        // if (CurrentStage == 2 && IsEnemyDefeated && CurrentWave < 4 &&
        //     CutscenesManager.IsCutsceneOver)
        // {
        //     StartNewWave();
        //     IsEnemyDefeated = false;
        //     StageText.text = "Stage " + CurrentStage + "-" + CurrentWave;
        // }

        // if (CurrentStage == 3 && IsEnemyDefeated && CurrentWave < 4 &&
        //     CutscenesManager.IsCutsceneOver)
        // {
        //     StartNewWave();
        //     IsEnemyDefeated = false;
        //     StageText.text = "Last Stage";
        // }
    }

    private void UpdatePlayerSpeed()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CharacterSpeed = 130f;
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
        if (CurrentStage == 2)
        {
            WaveNotification.GetComponentInChildren<Text>().text = "CONTINUE";
        } else
        {
            WaveNotification.GetComponentInChildren<Text>().text = "NEXT WAVE";
        }
        for (int i = 0; i < 3; i++)
        {
            WaveNotification.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            WaveNotification.SetActive(true);
            PlayerController.SoundsSource.PlayOneShot(ContinueSound);
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
                case 1: SpawnNewWave(Wave1_1, null); break;
                case 2: SpawnNewWave(Wave1_2, null); break;
                case 3: SpawnNewWave(Wave1_3, null); break;
            }

            StageText.text = "Stage " + CurrentStage + "-" + CurrentWave;
        } else if (StageController.CurrentStage == 2)
        {
            switch (CurrentWave)
            {
                case 1: SpawnNewWave(Wave2_1, Wave2_1_SpawnPoints); break;
                case 2: SpawnNewWave(Wave2_2, Wave2_2_SpawnPoints); break;
                case 3: SpawnNewWave(Wave2_3, Wave2_3_SpawnPoints); break;
            }

            StageText.text = "Stage " + CurrentStage + "-" + CurrentWave;
        }
        else if (StageController.CurrentStage == 3)
        {
            switch (CurrentWave)
            {
                case 1: SpawnNewWave(Wave3_1, Wave3_1_SpawnPoints); break;
                case 2: SpawnNewWave(Wave3_2, Wave3_2_SpawnPoints); break;
                case 3: SpawnNewWave(Wave3_3, Wave3_3_SpawnPoints); break;
            }
        }
    }

    private void SpawnNewWave(GameObject[] wave, GameObject[] spawnPoints)
    {
        if (spawnPoints != null)
        {
            int index = 0;
            foreach (GameObject enemy in wave)
            {
                Instantiate(enemy, spawnPoints[index].transform.position,
                    Quaternion.identity);
                index++;
                EnemiesCurrentlyAlive++;
            }
        } else
        {
            foreach (GameObject enemy in wave)
            {
                // We want the carts to spawn on a random horizontal
                // position only in the first stage.
                if (CurrentStage == 1)
                {
                    Instantiate(enemy, new Vector2(Random.Range(-5f, 5f),
                        enemy.transform.position.y),
                        Quaternion.identity);
                    EnemiesCurrentlyAlive++;
                } else
                {
                    Instantiate(enemy);
                    EnemiesCurrentlyAlive++;
                }
            }
        }
    }
}

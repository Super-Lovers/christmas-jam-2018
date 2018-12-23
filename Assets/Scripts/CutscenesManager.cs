﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutscenesManager : MonoBehaviour {
    private Animator _animator;
    public GameObject _fadeTransitioner;
    public static string Stage = "Stage 1";

    private GameObject _player;
    public GameObject StageOneLevel;
    public GameObject StageTwoLevel;
    public GameObject StageThreeLevel;

    public string[] StageTwoDialogue;
    public string[] StageThreeDialogue;
    public string[] EndingDialogue;
    public static int CurrentDialogueIndex;
    public static bool IsCutsceneOver = true;

    // This is used for toggling the background effects once
    // the first stage is complete
    public GameObject CloudsContainer;
    public GameObject GroundContainer;
    public GameObject BackgroundManager;

    private Vector2 _newPlayerPosition;

    private void Start()
    {
        _animator = _fadeTransitioner.GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (IsCutsceneOver == false && Input.GetKeyDown(KeyCode.Space))
        {
            if (Stage == "Stage 2")
            {
                if (CurrentDialogueIndex == StageTwoDialogue.Length)
                {
                    FadeOut();
                }
                else
                {
                    _fadeTransitioner.GetComponentInChildren<Text>().text =
                        StageTwoDialogue[CurrentDialogueIndex];

                    CurrentDialogueIndex++;
                }
            } else if (Stage == "Stage 3")
            {
                if (CurrentDialogueIndex == StageThreeDialogue.Length)
                {
                    FadeOut();
                }
                else
                {
                    _fadeTransitioner.GetComponentInChildren<Text>().text =
                        StageThreeDialogue[CurrentDialogueIndex];

                    CurrentDialogueIndex++;
                }
            } else if (Stage == "Ending Scene")
            {
                if (CurrentDialogueIndex == StageThreeDialogue.Length)
                {
                    FadeOut();
                }
                else
                {
                    _fadeTransitioner.GetComponentInChildren<Text>().text =
                        EndingDialogue[CurrentDialogueIndex];

                    CurrentDialogueIndex++;
                }
            }
        }
    }

    public void FadeIn()
    {
        _animator.SetBool("CanTransition", true);

        _newPlayerPosition = _player.transform.position;
        if (Stage == "Stage 1")
        {
            _fadeTransitioner.GetComponentInChildren<Text>().text = "Stage 1";
        } else if (Stage == "Stage 2")
        {
            _newPlayerPosition = StageTwoLevel.transform.position;

            _fadeTransitioner.GetComponentInChildren<Text>().text = "Stage 2";
            StartCoroutine(ChangeLevel(StageOneLevel, StageTwoLevel));
        }
        else if (Stage == "Stage 3")
        {
            _newPlayerPosition = StageThreeLevel.transform.position;

            _fadeTransitioner.GetComponentInChildren<Text>().text = "Stage 3";
            StartCoroutine(ChangeLevel(StageTwoLevel, StageThreeLevel));
        } else if (Stage == "Ending Scene")
        {
            _fadeTransitioner.GetComponentInChildren<Text>().text = "Ending Scene";
        }

        _animator.SetBool("HideTransition", false);

        Invoke("StopTransition", 1f);
    }

    public void FadeOut()
    {
        _animator.SetBool("CanTransition", true);
        _animator.SetBool("HideTransition", true);

        Invoke("StopTransition", 1f);
        IsCutsceneOver = true;
        if (Stage == "Ending Scene")
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    private void StopTransition()
    {

        _player.transform.position = _newPlayerPosition;
        _animator.SetBool("CanTransition", false);
    }

    private IEnumerator ChangeLevel(GameObject previousLevel, GameObject newLevel)
    {
        yield return new WaitForSeconds(1f);

        CloudsContainer.SetActive(false);
        GroundContainer.SetActive(false);
        BackgroundManager.SetActive(false);
        
        if (Stage != "Stage 3")
        {
            _player.GetComponent<PlayerController>().UpdatePlayerPerspective();
        }

        previousLevel.SetActive(false);
        newLevel.SetActive(true);
    }
}

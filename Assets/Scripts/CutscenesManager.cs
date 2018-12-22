using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutscenesManager : MonoBehaviour {
    private Animator _animator;
    public GameObject _fadeTransitioner;
    public static string Stage = "Stage 1";

    private GameObject _player;
    public GameObject StageOneLevel;
    public GameObject StageTwoLevel;

    public string[] StageTwoDialogue;
    private int _currentDialogueIndex;
    public static bool IsCutsceneOver = true;

    private void Start()
    {
        _animator = _fadeTransitioner.GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (IsCutsceneOver == false && Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentDialogueIndex == StageTwoDialogue.Length)
            {
                FadeOut();
            } else
            {
                _fadeTransitioner.GetComponentInChildren<Text>().text =
                    StageTwoDialogue[_currentDialogueIndex];

                _currentDialogueIndex++;
            }
        }
    }

    public void FadeIn()
    {
        _animator.SetBool("CanTransition", true);

        if (Stage == "Stage 1")
        {
            _fadeTransitioner.GetComponentInChildren<Text>().text = "Skyscraping";
        } else if (Stage == "Stage 2")
        {
            _fadeTransitioner.GetComponentInChildren<Text>().text = "Progress";

            StartCoroutine(ChangeLevel(StageOneLevel, StageTwoLevel));
        }
        else if (Stage == "Stage 3")
        {
            _fadeTransitioner.GetComponentInChildren<Text>().text = "Challenger";
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
    }

    private void StopTransition()
    {
        _animator.SetBool("CanTransition", false);
    }

    private IEnumerator ChangeLevel(GameObject previousLevel, GameObject newLevel)
    {
        yield return new WaitForSeconds(1f);

        _player.GetComponent<PlayerController>().UpdatePlayerPerspective();

        previousLevel.SetActive(false);
        newLevel.SetActive(true);
    }
}

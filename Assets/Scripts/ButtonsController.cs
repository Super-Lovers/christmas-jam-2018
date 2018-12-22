using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour {
    public GameObject FadeTransitioner;
    public string[] StageOneDialogue;
    private int _currentDialogueIndex;

    public void StartGame()
    {
        FadeTransitioner.SetActive(true);
    }

    private void Update()
    {
        if (FadeTransitioner.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_currentDialogueIndex == StageOneDialogue.Length)
                {
                    SceneManager.LoadScene("SampleScene");
                } else
                {
                    FadeTransitioner.GetComponentInChildren<Text>().text =
                        StageOneDialogue[_currentDialogueIndex];

                    _currentDialogueIndex++;
                }
            }
        }
    }
}

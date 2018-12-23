using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour {
    public GameObject FadeTransitioner;
    public GameObject CurrentCutsceneImage;
    public GameObject CurrentHeaderImage;
    public string[] StageOneDialogue;
    public Sprite[] StageOneImages;
    private int _currentDialogueIndex;

    public void StartGame()
    {
        FadeTransitioner.SetActive(true);
    }

    private void Update()
    {
        CurrentHeaderImage.GetComponent<Image>().sprite =
            CurrentCutsceneImage.GetComponent<Image>().sprite;

        if (FadeTransitioner.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_currentDialogueIndex == StageOneDialogue.Length)
                {
                    SceneManager.LoadScene("SampleScene");
                } else
                {
                    CurrentCutsceneImage.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                    CurrentCutsceneImage.GetComponent<Image>().sprite =
                        StageOneImages[_currentDialogueIndex];
                    FadeTransitioner.GetComponentInChildren<Text>().text =
                        StageOneDialogue[_currentDialogueIndex];

                    _currentDialogueIndex++;
                }
            }
        }
    }
}

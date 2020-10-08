using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutscenesModel : MonoBehaviour {
    [SerializeField] private Image cutscene_cover;
    [SerializeField] private TextMeshProUGUI cutscene_text;

    [SerializeField] private Cutscene[] cutscenes;

    [SerializeField] private Cutscene current_cutscene;
    private int current_passage_index = 0;

    private void Start() {
        PlayCutscene(current_cutscene);
    }

    public void PlayCutscene(Cutscene cutscene) {
        // Retrieving the cutscene with that dialogue
        for (int i = 0; i < cutscenes.Length; i++) {
            if (cutscenes[i].dialogue == cutscene.dialogue) { current_cutscene = cutscenes[i]; break; }
        }

        LoadPassage();
    }

    private void LoadPassage() {
        // Makes the cutscene visuals visible
        cutscene_cover.color = new Color(1, 1, 1, 1);
        Passage current_passage = null;

        // Retrieving the current passage of the current dialogue
        for (int i = 0; i < current_cutscene.dialogue.passages.Length; i++) {
            var passage = current_cutscene.dialogue.passages[i];
            
            if (i == current_passage_index) {
                current_passage = passage;
                current_passage_index++;
                break;
            }
        }

        if (current_passage == null) { return; }
        cutscene_cover.sprite = current_passage.cover;
        cutscene_text.text = current_passage.text;
    }

    private void CloseCutscene() {
        cutscene_cover.color = new Color(0, 0, 0, 0);
        cutscene_text.text = string.Empty;

        current_passage_index = 0;
        current_cutscene = null;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (current_passage_index < current_cutscene.dialogue.passages.Length) { LoadPassage(); }
            else { CloseCutscene(); }
        }
    }
}

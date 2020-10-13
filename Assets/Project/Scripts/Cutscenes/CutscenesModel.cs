using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutscenesModel : MonoBehaviour {
	[SerializeField] private Image cutscene_cover;
	[SerializeField] private TextMeshProUGUI cutscene_text;

	[SerializeField] private Cutscene[] cutscenes;

	private Cutscene current_cutscene;
	private int current_passage_index = 0;

	private float current_volume;

	public void PlayCutscene(Cutscene cutscene) {
		// Stops the audio from playing during a dualogue
		current_volume = AudioListener.volume;
		AudioListener.volume = 0;

		App.Get().settings.is_paused = true;

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
		AudioListener.volume = current_volume;

		// If this cutscene loads has a new scene, it will do so
		// (currently used specifically for the ending cutscene)
		// ********************************************
		if (current_cutscene.scene_to_load != "" &&
			current_cutscene.scene_to_load != string.Empty &&
			current_cutscene.scene_to_load != null) {
			SceneManager.LoadScene(current_cutscene.scene_to_load);
			return;
		}

		cutscene_cover.color = new Color(0, 0, 0, 0);
		cutscene_text.text = string.Empty;

		current_passage_index = 0;

		current_cutscene = null;

		App.Get().settings.is_paused = false;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space) && current_cutscene != null) {
			if (current_passage_index < current_cutscene.dialogue.passages.Length) { LoadPassage(); }
			else { CloseCutscene(); }
		}
	}
}

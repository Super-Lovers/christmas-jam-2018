using UnityEngine;

public class StagesModel : MonoBehaviour {
	[SerializeField] private GameObject[] stages;

	// ********************************************
	// Dependancies
	[SerializeField] private CutscenesModel cutscenes_model;
	// ********************************************

	[HideInInspector] public StageController current_stage;

	private void Start() { StartStage(stages[1]); }

	public void StartStage(GameObject stage) {
		if (current_stage != null) { current_stage.gameObject.SetActive(false); }

		stage.SetActive(true);
		var stage_controller = stage.GetComponent<StageController>();
		current_stage = stage_controller;

		if (App.Get().settings.debug_mode) {
			Debug.Log("Started stage <color=\"gray\">" + stage_controller.stage_name + "</color>.");
		}

		cutscenes_model.PlayCutscene(stage_controller.cutscene);
		stage_controller.StartNextWave();

		stage_controller.stage_player.transform.SetParent(stage_controller.start_transform);
		stage_controller.stage_player.transform.position = stage_controller.start_transform.position;
	}
}

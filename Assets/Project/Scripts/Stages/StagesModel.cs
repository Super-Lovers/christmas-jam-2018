using UnityEngine;

public class StagesModel : MonoBehaviour {
	[SerializeField] private GameObject[] stages;

	// ********************************************
	// Dependancies
	[SerializeField] private CutscenesModel cutscenes_model;
	[SerializeField] private PlayerController player;
	// ********************************************

	private void Start() { StartStage(stages[0]); }

	public void StartStage(GameObject stage) {
		stage.SetActive(true);
		var stage_controller = stage.GetComponent<StageController>();

		if (App.Get().settings.debug_mode) {
			Debug.Log("Started stage <color=\"cyan\">" + stage_controller.stage_name + "</color>.");
		}

		cutscenes_model.PlayCutscene(stage_controller.cutscene);
		stage_controller.StartNextWave();

		player.transform.SetParent(stage_controller.start_transform);
		player.transform.position = stage_controller.start_transform.position;
	}
}

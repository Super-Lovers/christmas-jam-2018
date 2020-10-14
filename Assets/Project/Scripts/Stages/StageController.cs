using UnityEngine;

public class StageController : MonoBehaviour {
	public string stage_name;

	/// <summary>
	/// Until all the waves are completed, the player
	/// cannot further the story.
	/// </summary>
	[Space(10)]
	public Wave[] waves;
	private int current_wave;

	/// <summary>
	/// When the stage is started, the player will be
	/// transported to this transform position.
	/// </summary>
	[Space(10)]
	public Transform start_transform;

	/// <summary>
	/// The player instance for this stage, since different
	/// stages have different playable characters.
	/// </summary>
	public Entity stage_player;

	/// <summary>
	/// The cutscene to play once the stage is started.
	/// </summary>
	[Space(10)]
	public Cutscene enter_cutscene;

	/// <summary>
	/// The cutscene to play once the stage is completed.
	/// </summary>
	public Cutscene exit_cutscene;

	/// <summary>
	/// The stage to play once this one is completed.
	/// </summary>
	public StageController next_stage;

	// ********************************************
	// Dependancies
	[Header("Dependancies (to inject)")]
	[SerializeField] private StagesModel stages_model;
	[SerializeField] private CutscenesModel cutscene_model;
	// ********************************************

	/// <summary>
	/// Starts the next wave and if the waves are over, it will start
	/// the next stage, otherwise it will spawn the wave.
	/// </summary>
	public bool StartNextWave() {
		if (current_wave >= waves.Length) {
			if (next_stage != null) {
				CancelInvoke();
				stages_model.StartStage(next_stage.gameObject);
			} else if (exit_cutscene != null) {
				cutscene_model.PlayCutscene(exit_cutscene);
			}

			return false;
		}

		if (App.Get().settings.debug_mode) {
			Debug.Log("Spawned wave <color=\"red\">" + waves[current_wave].wave_name + "</color>.");
		}

		Invoke("SpawnNextWave", 1f);

		return true;
	}

	/// <summary>
	/// Spawns the entities of the next wave.
	/// </summary>
	private void SpawnNextWave() {
		waves[current_wave].gameObject.SetActive(true);
		foreach (var entity in waves[current_wave].entities) {
			entity.gameObject.SetActive(true);
		}

		current_wave++;
	}
}

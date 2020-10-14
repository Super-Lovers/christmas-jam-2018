using UnityEngine;

public class PropsGenerator : MonoBehaviour {
	/// <summary>
	/// Props definitions and prefabs with the container in which they spawn in.
	/// </summary>
    [SerializeField] private PropConfig[] props;

	// ********************************************
	// Dependancies
	[SerializeField] private StagesModel stages_model;
	// ********************************************

	private void Start () { InvokeRepeating("Spawn", 1f, 1.5f); }
	
	private void Spawn() {
		if (App.Get().settings.is_paused) { return; }

		// If the stage is no longer the first stage
		// then stop spawning props in the background.
		if (stages_model.current_stage.stage_name != "s_1") { this.CancelInvoke(); }

		// Choosing a random prop type
		var prop_type = Random.Range(0, (int)Prop.number_of_props);
		var prop = (Prop)prop_type;

		// Retrieving the chosen prop type's configuration with the prefab
		var prop_config = props[0];
		for (int i = 0; i < props.Length; i++) {
			if (props[i].prop_type == prop) { prop_config = props[i]; }
		}

		// Choosing a random number of new props
		var count = Random.Range(1, 3);

		// Instantiating the chosen chosen a random number of times
		for (int i = 0; i < count; i++) {
			// A position somewhere in the direction of the player
			// outside of his field of view
			var newPosition = new Vector2(
				Random.Range(-5, 5),
				Random.Range(
					stages_model.current_stage.stage_player.transform.position.y + 8,
					stages_model.current_stage.stage_player.transform.position.y + 12
				)
			);

			Instantiate(
				prop_config.prop_prefabs[Random.Range(0, prop_config.prop_prefabs.Length)],
				newPosition,
				Quaternion.identity,
				this.transform
			);
		}
	}
}

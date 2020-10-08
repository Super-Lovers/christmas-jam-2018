using UnityEngine;

public class PropsGenerator : MonoBehaviour {
	/// <summary>
	/// Props definitions and prefabs with the container in which they spawn in.
	/// </summary>
    [SerializeField] private PropConfig[] props;
    [SerializeField] private PlayerController player;

	// Use this for initialization
	private void Start () {
		InvokeRepeating("Spawn", 1f, 1.5f);
	}
	
	private void Spawn() {
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
			var newPosition = new Vector2(
				Random.Range(-5, 5),
				Random.Range(player.transform.position.y + 2, + 4)
			);

			Instantiate(
				prop_config.prop_prefabs[Random.Range(0, prop_config.prop_prefabs.Length)],
				newPosition, Quaternion.identity,
				this.transform);
		}
	}
}

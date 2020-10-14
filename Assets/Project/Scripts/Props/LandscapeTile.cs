using UnityEngine;

public class LandscapeTile : MonoBehaviour {
	/// <summary>
	/// The tile that will be generated every time the
	/// player reaches the edge of the visible landscape
	/// </summary>
	[SerializeField] private GameObject[] background_tile_prefabs;

	/// <summary>
	/// Necessary to calculate the size of the
	/// tile and to place the new one after it
	/// </summary>
	private SpriteRenderer[] sprite_renderers;
	private float total_tiles_height;

	private void Start() {
		sprite_renderers = GetComponentsInChildren<SpriteRenderer>();

		for (int i = 0; i < sprite_renderers.Length; i++) {
			total_tiles_height += sprite_renderers[i].bounds.size.y;
		}

		// This accounts for the origin being in the center of the sprite
		// so the new tile won't be positioned incorrectly
		total_tiles_height -= sprite_renderers[0].bounds.size.y / 2;
	}

	/// <summary>
	/// Creates a new tile after the current one
	/// when the player enters its bounds
	/// </summary>
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			var new_position = new Vector3(
				transform.position.x,
				transform.position.y + total_tiles_height,
				transform.position.z
			);

			Instantiate(
				background_tile_prefabs[ // Random tile from tiles
					Random.Range(0, background_tile_prefabs.Length)],
				new_position,
				Quaternion.identity,
				transform.parent.parent // Mhm..
			);
		}
	}
}

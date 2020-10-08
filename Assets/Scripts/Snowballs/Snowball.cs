using UnityEngine;

public class Snowball : MonoBehaviour {
	[SerializeField] private Rigidbody2D rigidbody_2d;

	private void Start() {
		// After a few seconds from spawning, the snowball will 
		// be destroyed from the game to save performance
		Invoke("DestroySnowball", 3f);
	}

	public void CreateSnowball(string direction) {
		switch (direction) {
			case "up": rigidbody_2d.velocity = new Vector2(0, 200f * Time.deltaTime); break;
			case "down": rigidbody_2d.velocity = new Vector2(0, -200f * Time.deltaTime); break;
			default: break;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("entity")) { Destroy(gameObject); }
	}

	private void DestroySnowball() {
		Destroy(gameObject);
	}
}
using UnityEngine;

public class Snowball : MonoBehaviour {
	[SerializeField] private Rigidbody2D rigidbody_2d;
	[SerializeField] private int damage;

	/// <summary>
	/// The entity that fired this snowball.
	/// </summary>
	private Entity parent_entity;

	private void Start() {
		// After a few seconds from spawning, the snowball will 
		// be destroyed from the game to save performance
		Invoke("DestroySnowball", 3f);

		parent_entity = GetComponentInParent<Entity>();
	}

	public void CreateSnowball(string direction) {
		switch (direction) {
			case "up": rigidbody_2d.velocity = new Vector2(0, 200f * Time.deltaTime); break;
			case "down": rigidbody_2d.velocity = new Vector2(0, -200f * Time.deltaTime); break;
			default: break;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("entity")) {
			var entity = other.GetComponent<Entity>();
			if (entity != parent_entity) {
				other.GetComponent<Entity>().TakeDamage(damage);
				Destroy(gameObject);
			}
		}
	}

	private void DestroySnowball() {
		Destroy(gameObject);
	}
}
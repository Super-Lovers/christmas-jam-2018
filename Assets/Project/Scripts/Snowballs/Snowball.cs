using UnityEngine;

public class Snowball : MonoBehaviour {
	private Rigidbody2D rigidbody_2d;
	[SerializeField] private int damage;

	/// <summary>
	/// The entity that fired this snowball.
	/// </summary>
	private Entity parent_entity;

	private void Awake() {
		rigidbody_2d = GetComponent<Rigidbody2D>();

		// After a few seconds from spawning, the snowball will 
		// be destroyed from the game to save performance
		Invoke("DestroySnowball", 5f);

		parent_entity = GetComponentInParent<Entity>();

		// Ignores the collision between itself and its parent
		Physics2D.IgnoreCollision(
			GetComponent<Collider2D>(),
			parent_entity.GetComponent<Collider2D>());
			
		// Ignores the collision between itself and other snowballs
		Physics2D.IgnoreLayerCollision(9, 9);
	}

	public void CreateSnowball(string direction) {
		switch (direction) {
			case "up": rigidbody_2d.velocity = new Vector2(0, 200f * Time.deltaTime); break;
			case "down": rigidbody_2d.velocity = new Vector2(0, -200f * Time.deltaTime); break;
			default: break;
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("entity") || other.gameObject.CompareTag("Player")) {
			var entity = other.gameObject.GetComponent<Entity>();
			if (entity != parent_entity) {
				other.gameObject.GetComponent<Entity>().TakeDamage(damage);

				CancelInvoke();
				Destroy(gameObject);
			}
		}
	}

	private void DestroySnowball() {
		Destroy(gameObject);
	}
}
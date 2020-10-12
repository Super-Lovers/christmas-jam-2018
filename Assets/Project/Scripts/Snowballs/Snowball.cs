using UnityEngine;

public class Snowball : MonoBehaviour {
	private Rigidbody2D rigidbody_2d;
	[SerializeField] private int damage;
	[SerializeField] [Range(100, 300)] private int speed;
	private int speed_multiplier = 100;

	[SerializeField] private float time_until_destroy = 5f;

	[Space(10)]
	[Header("Fire snowball forward automatically")]
	[SerializeField] private bool fire_automatically;

	/// <summary>
	/// The entity that fired this snowball.
	/// </summary>
	private Entity parent_entity;

	private void Awake() {
		rigidbody_2d = GetComponent<Rigidbody2D>();

		// After a few seconds from spawning, the snowball will 
		// be destroyed from the game to save performance
		Invoke("DestroySnowball", time_until_destroy);

		parent_entity = GetComponentInParent<Entity>();

		// Ignores the collision between itself and its parent
		var parent_collider = parent_entity.GetComponent<Collider2D>();
		if (parent_collider != null) {
			Physics2D.IgnoreCollision(
				GetComponent<Collider2D>(),
				parent_collider);
		}
			
		// Ignores the collision between itself and other snowballs
		Physics2D.IgnoreLayerCollision(9, 9);

		if (fire_automatically) { 
			// Ignores the boss collider
			Physics2D.IgnoreCollision(
				GetComponent<Collider2D>(),
				parent_entity.GetComponentInChildren<BoxCollider2D>());

			rigidbody_2d.AddRelativeForce(Vector2.up * speed * Time.deltaTime * speed_multiplier);
		}
	}

	public void CreateSnowball(string direction) {
		switch (direction) {
			case "up": rigidbody_2d.AddForce(new Vector2(0, speed * Time.deltaTime * speed_multiplier)); break;
			case "down": rigidbody_2d.AddForce(new Vector2(0, -speed * Time.deltaTime * speed_multiplier)); break;
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
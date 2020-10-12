using UnityEngine;

public class EnemyElfHumanoidController : Entity {
	private Rigidbody2D rigid_body;
	private CircleCollider2D radius_collider;
	
	private Collider2D player;

	private void Start() {
		rigid_body = GetComponent<Rigidbody2D>();
		radius_collider = GetComponentInChildren<CircleCollider2D>();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		// ********************************************
		// Throwing snowballs
		// ********************************************

		// ********************************************
		// Movement
		// ********************************************
		if (player != null) {
			if (Vector2.Distance(player.transform.position, transform.position) > 1.5f) {
				transform.position = Vector2.MoveTowards(
						transform.position,
						player.transform.position,
						(this.movement_speed * 0.01f) * Time.deltaTime);
			}

			var direction = player.transform.position - transform.position;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			// The -90f offsets the sprite origin direction
			transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag("Player")) { player = other; }
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) { player = null; }
	}
}

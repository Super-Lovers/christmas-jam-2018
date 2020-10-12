using UnityEngine;

public class PlayerElfHumanoidController : Entity {
	private Rigidbody2D rigid_body;

	private void Start() {
		rigid_body = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		if (!Input.anyKey) { return; }

		// ********************************************
		// Candy stick attack
		// ********************************************
		if (Input.GetKeyDown(KeyCode.Space)) { }

		// ********************************************
		// Movement
		// ********************************************
		var horizontal = Input.GetAxisRaw("Horizontal");
		var vertical = Input.GetAxisRaw("Vertical");

		if (horizontal != 0 || vertical != 0) {
			if (horizontal > 0) {
				rigid_body.AddForce(new Vector2(this.movement_speed * Time.deltaTime, 0), ForceMode2D.Force);
			} else if (horizontal < 0) {
				rigid_body.AddForce(new Vector2(-this.movement_speed * Time.deltaTime, 0), ForceMode2D.Force);
			}

			if (vertical > 0) {
				rigid_body.AddForce(new Vector2(0, this.movement_speed * Time.deltaTime), ForceMode2D.Force);
			} else if (vertical < 0) {
				rigid_body.AddForce(new Vector2(0, -this.movement_speed * Time.deltaTime), ForceMode2D.Force);
			}
		}
	}
}

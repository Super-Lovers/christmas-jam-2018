using UnityEngine;

public class PlayerController : Entity {
	private SnowballController snowball_controller;
	private Rigidbody2D rigid_body;

	private void Start() {
		snowball_controller = GetComponent<SnowballController>();
		rigid_body = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		if (!Input.anyKey) { return; }

		if (Input.GetKeyDown(KeyCode.Space)) {
			snowball_controller.FireSnowball("up");
		}

		var horizontal = Input.GetAxisRaw("Horizontal");

		if (horizontal != 0) {
			if (horizontal > 0) {
				rigid_body.AddForce(new Vector2(this.movement_speed * Time.deltaTime, 0), ForceMode2D.Force);
			} else if (horizontal < 0) {
				rigid_body.AddForce(new Vector2(-this.movement_speed * Time.deltaTime, 0), ForceMode2D.Force);
			}
		}
	}
}

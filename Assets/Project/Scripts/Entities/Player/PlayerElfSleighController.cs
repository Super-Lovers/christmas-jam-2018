using UnityEngine;

public class PlayerElfSleighController : Entity {
	private SnowballController snowball_controller;
	private Rigidbody2D rigid_body;

	private void Start() {
		snowball_controller = GetComponent<SnowballController>();
		rigid_body = GetComponent<Rigidbody2D>();

		Init();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		if (!Input.anyKey) { return; }

		Attack();
		Move();
	}

	public override void Attack() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			snowball_controller.FireSnowball("up");
		}
	}

	public override void Move() {
		var horizontal = Input.GetAxisRaw("Horizontal");

		if (horizontal != 0) {
			if (horizontal > 0) {		 // Right
				rigid_body.AddForce(
					new Vector2(this.movement_speed * Time.deltaTime, 0),
					ForceMode2D.Force);
			} else if (horizontal < 0) { // Left
				rigid_body.AddForce(
					new Vector2(-this.movement_speed * Time.deltaTime, 0),
					ForceMode2D.Force);
			}
		}
	}
}

using UnityEngine;

public class PlayerController : Entity {
	private SnowballController snowball_controller;

	private void Start() {
		snowball_controller = GetComponent<SnowballController>();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		if (Input.GetKeyDown(KeyCode.Space)) {
			snowball_controller.FireSnowball("up");
		}
	}
}

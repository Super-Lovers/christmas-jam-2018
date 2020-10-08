using UnityEngine;

public class EnemySleighController : Entity {
	private SnowballController snowball_controller;
	private float countdown = 5;

	private void Start() {
		snowball_controller = GetComponent<SnowballController>();
	}

	private void Update() {
		if (countdown > 0) { countdown -= Time.deltaTime; }
		else if (countdown <= 0) {
			snowball_controller.FireSnowball("down");
			countdown = Random.Range(2, 5);
		}
	}
}

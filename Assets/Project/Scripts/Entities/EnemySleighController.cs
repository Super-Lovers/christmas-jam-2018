using UnityEngine;

public class EnemySleighController : Entity {
	private SnowballController snowball_controller;

	/// <summary>
	/// The time it takes before this entity can fire a snowball
	/// </summary>
	private float fire_countdown = 5;
	private float fire_countdown_max;

	/// <summary>
	/// The time it takes before this entity moves around
	/// </summary>
	private float moving_countdown = 3;
	private float moving_countdown_max;
	[SerializeField] private float move_multiplier = 3;

	/// <summary>
	/// The wave this entity belongs in.
	/// </summary>
	private Wave wave;

	private Rigidbody2D rigid_body;

	private void Start() {
		fire_countdown_max = fire_countdown;
		moving_countdown_max = moving_countdown;

		snowball_controller = GetComponent<SnowballController>();
		rigid_body = GetComponent<Rigidbody2D>();

		wave = GetComponentInParent<Wave>();
		wave.AddToWave(this);

		Init();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		Attack();
		Move();
	}

	public override void Attack() {
		if (fire_countdown > 0) { fire_countdown -= Time.deltaTime; }
		else if (fire_countdown <= 0) {
			snowball_controller.FireSnowball("down");
			fire_countdown = Random.Range(2, fire_countdown_max);
		}
	}

	public override void Move() {
		if (moving_countdown > 0) { moving_countdown -= Time.deltaTime; }
		else if (moving_countdown <= 0) {
			var direction_rng = Random.Range(0, 100);
			if (direction_rng >= 50) {	// Right
				rigid_body.AddForce(
					new Vector2(this.movement_speed * Time.deltaTime * move_multiplier, 0),
					ForceMode2D.Impulse);
			} else {					// Left
				rigid_body.AddForce(
					new Vector2(-this.movement_speed * Time.deltaTime * move_multiplier, 0),
					ForceMode2D.Impulse);
			}

			moving_countdown = Random.Range(1, moving_countdown_max);
		}
	}

	public override void TakeDamage(int damage) {
		if (health - damage <= 0) {
			wave.RemoveFromWave(this);
		}
		base.TakeDamage(damage);
	}
}

using UnityEngine;

public class Boss : Entity {
	[SerializeField] private float attack_rate;
	private float attack_rate_max;

	/// <summary>
	/// The wave this entity belongs in.
	/// </summary>
	private Wave wave;

	[Space(10)]
	[SerializeField] private GameObject[] splash_attacks;
	// Linear, so that the players can learn the pattern
	// of the boss's attacks
	private int current_splash_attack_index;

	private void Start() {
		attack_rate_max = attack_rate;

		wave = GetComponentInParent<Wave>();
		wave.AddToWave(this);

		Init();
	}

	private void Update() {
		if (attack_rate > 0) { attack_rate -= Time.deltaTime; }
		else { Attack(); }
	}

	public override void Attack() {
		SpawnSplashAttack();
		attack_rate = attack_rate_max;
	}

	private void SpawnSplashAttack() {
		var splash = Instantiate(
			splash_attacks[current_splash_attack_index],
			transform);

		current_splash_attack_index++;
		if (current_splash_attack_index >= splash_attacks.Length) {
			current_splash_attack_index = 0;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("sword")) { TakeDamage(10); }
	}

	public override void TakeDamage(int damage) {
		if (health - damage <= 0) {
			wave.RemoveFromWave(this);
		}

		base.TakeDamage(damage);
	}
}

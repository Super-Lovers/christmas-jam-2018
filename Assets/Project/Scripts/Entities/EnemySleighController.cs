using UnityEngine;

public class EnemySleighController : Entity {
	private SnowballController snowball_controller;
	private float countdown = 5;

	/// <summary>
	/// The wave this entity belongs in.
	/// </summary>
	private Wave wave;

	private void Start() {
		snowball_controller = GetComponent<SnowballController>();
		wave = GetComponentInParent<Wave>();
		wave.AddToWave(this);
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		if (countdown > 0) { countdown -= Time.deltaTime; }
		else if (countdown <= 0) {
			snowball_controller.FireSnowball("down");
			countdown = Random.Range(2, 5);
		}
	}

    public override void TakeDamage(int damage)
    {
		if (health - damage <= 0) { wave.RemoveFromWave(this); }
        base.TakeDamage(damage);
    }
}

using UnityEngine;

public class EnemyElfHumanoidController : Entity {
	private Animator animator;
	
	private GameObject player;

	/// <summary>
	/// Melee weapon (candy stick) that inflicts damage to enemy entities
	/// </summary>
	[SerializeField] private GameObject sword;
	[SerializeField] private float attack_rate;
	private float attack_rate_max;

	/// <summary>
	/// The wave this entity belongs in.
	/// </summary>
	private Wave wave;

	private void Start() {
		// TODO: Find a better solution
		player = GameObject.FindGameObjectWithTag("Player");
		attack_rate_max = attack_rate;

		animator = GetComponent<Animator>();

		// Ignoring the colliders of this object to avoid the attack hitting it.
		Physics2D.IgnoreCollision(
			gameObject.GetComponent<BoxCollider2D>(),
			sword.GetComponent<BoxCollider2D>());
		// ********************************************

		wave = GetComponentInParent<Wave>();
		wave.AddToWave(this);

		Init();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		Attack();
		if (Vector2.Distance(
			transform.position,
			player.transform.position) < 3f) {
			Move();
		}
	}

	public override void Attack() {
		if (attack_rate > 0) { attack_rate -= Time.deltaTime; }
	}

	public override void Move() {
		if (player != null) {
			if (Vector2.Distance(player.transform.position, transform.position) > 1.5f) {
				transform.position = Vector2.MoveTowards(
						transform.position,
						player.transform.position,
						(this.movement_speed * 0.01f) * Time.deltaTime);
			}

			if (Vector2.Distance(player.transform.position, transform.position) < 2f && attack_rate <= 0) {
				AudioManager.Get().PlaySound(AudioFile.CandyCaneAttack);

				animator.SetBool("is_attacking", true);
				sword.SetActive(true);

				Invoke("ResetAttack", 0.2f);
				attack_rate = attack_rate_max;
			}

			var direction = player.transform.position - transform.position;
			var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// (temporary) Hack due to collisions not working in build 
		if (other.gameObject.name == "sword") { TakeDamage(10); }
	}

	private void ResetAttack() {
		animator.SetBool("is_attacking", false);
		sword.SetActive(false);
	}
	
	public override void TakeDamage(int damage) {
		if (health - damage <= 0) {
			wave.RemoveFromWave(this);
			player.GetComponent<Entity>().Heal(55);
		}

		base.TakeDamage(damage);
	}
}

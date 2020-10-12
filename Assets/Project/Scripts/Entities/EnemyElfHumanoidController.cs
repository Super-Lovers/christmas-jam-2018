using UnityEngine;

public class EnemyElfHumanoidController : Entity {
	private Rigidbody2D rigid_body;
	private CircleCollider2D radius_collider;
	private Animator animator;
	
	private Collider2D player;

	/// <summary>
	/// Melee weapon (candy stick) that inflicts damage to enemy entities
	/// </summary>
	[SerializeField] private GameObject sword;
	[SerializeField] private float attack_rate;
	private float attack_rate_max;

	private void Start() {
		attack_rate_max = attack_rate;

		rigid_body = GetComponent<Rigidbody2D>();
		radius_collider = GetComponentInChildren<CircleCollider2D>();
		animator = GetComponent<Animator>();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		Attack();
		Move();
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

	private void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag("Player")) { player = other; }
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) { player = null; }
	}

	private void ResetAttack() {
		animator.SetBool("is_attacking", false);
		sword.SetActive(false);
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerElfHumanoidController : Entity {
	private Rigidbody2D rigid_body;
	private Animator animator;
	private AudioSource audio_source;

	// ********************************************
	// Dependancies
	[SerializeField] private Camera main_camera;
	// ********************************************

	/// <summary>
	/// Melee weapon (candy stick) that inflicts damage to enemy entities
	/// </summary>
	[SerializeField] private GameObject sword;
	[SerializeField] private float attack_rate;
	private float attack_rate_max;

	private void Start() {
		attack_rate_max = attack_rate;
		audio_source = GetComponent<AudioSource>();
		rigid_body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		audio_source.enabled = false;

		Physics2D.IgnoreCollision(
			gameObject.GetComponent<BoxCollider2D>(),
			sword.GetComponent<BoxCollider2D>());

		Init();
	}

	private void Update() {
		if (App.Get().settings.is_paused) { return; }

		if (attack_rate > 0) { attack_rate -= Time.deltaTime; }

		// ********************************************
		// Rotating the player with the mouse
		main_camera.transform.position = new Vector3(
				transform.position.x,
				transform.position.y,
				main_camera.transform.position.z);

		// The camera has to be separated from the player, because
		// rotating the player would mean rotating the camera if
		// they were nested together
		var mouse_direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		var angle = Mathf.Atan2(mouse_direction.y, mouse_direction.x) * Mathf.Rad2Deg;
		// The -90f offsets the sprite origin direction
	 	transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
		// ********************************************

		if (!Input.anyKey) { 
			if (audio_source.enabled) {
				audio_source.Stop();
				audio_source.enabled = false;
			}
			
			return;
		}

		Attack();
		Move();
	}

	public override void Attack() {
		if (Input.GetKeyDown(KeyCode.Space) && attack_rate <= 0) {
			AudioManager.Get().PlaySound(AudioFile.CandyCaneAttack);

			animator.SetBool("is_attacking", true);
			sword.SetActive(true);

			Invoke("ResetAttack", 0.2f);
			attack_rate = attack_rate_max;
		}
	}

	public override void Move() {
		var horizontal = Input.GetAxisRaw("Horizontal");
		var vertical = Input.GetAxisRaw("Vertical");

		if (horizontal != 0 || vertical != 0) {
			if (!audio_source.isPlaying) {
				audio_source.enabled = true;
				audio_source.Play();
			}

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

	private void OnTriggerEnter2D(Collider2D other) {
		// (temporary) Hack due to collisions not working in build 
		if (other.gameObject.name == "sword") { TakeDamage(10); }
	}

	private void ResetAttack() {
		animator.SetBool("is_attacking", false);
		sword.SetActive(false);
	}

	public override void TakeDamage(int damage)
	{
		if (health - damage <= 0) { SceneManager.LoadScene("game_over"); }
		base.TakeDamage(damage);
	}
}

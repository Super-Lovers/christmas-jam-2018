using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
	[SerializeField] protected int health = 100;
	[Range(100, 250)]
	[SerializeField] protected float movement_speed;

	public SpriteRenderer sprite_renderer;

	public virtual void Attack() {}

	public virtual void TakeDamage(int damage) {
		if (health - damage <= 0) { Destroy(this.gameObject); }
		else { health -= damage; StartCoroutine(Flash()); }
	}

	public virtual void Move() {}

	/// <summary>
	/// Gives a red overlay flash to indicate this entity
	/// has taken damage.
	/// </summary>
	public virtual IEnumerator Flash() {
		for (int i = 0; i < 3; i++) {
			sprite_renderer.color = new Color(1, 0, 0, 1);
			yield return new WaitForSeconds(0.1f);
			sprite_renderer.color = new Color(1, 1, 1, 1);
			yield return new WaitForSeconds(0.1f);
		}
	}
}
using UnityEngine;

public abstract class Entity : MonoBehaviour {
	protected int health = 100;
	[Range(100, 250)]
	[SerializeField] protected float movement_speed;

	public virtual void Attack() {}

	public virtual void TakeDamage(int damage) {
		if (health - damage <= 0) { Destroy(this.gameObject); }
		else { health -= damage; }
	}

	public virtual void Move() {}
}
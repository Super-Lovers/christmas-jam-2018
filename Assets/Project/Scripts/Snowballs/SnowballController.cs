using UnityEngine;

public class SnowballController : MonoBehaviour {
	[SerializeField] private GameObject snowball_prefab;
	[SerializeField] [Range(0, 3)] private float fire_rate;
	private float initial_fire_rate;

	private void Start() { initial_fire_rate = fire_rate; }

	private void Update() {
		if (fire_rate > 0) {
			fire_rate -= Time.deltaTime;
		}
	}

	/// <summary>
	/// Fires a snowball in the given direction when the rate of fire permits.
	/// </summary>
	/// <param name="direction"></param>
	public void FireSnowball(string direction) {
		if (fire_rate > 0) { return; }

		var snowball = Instantiate(snowball_prefab);
		var snowball_script = snowball.GetComponent<Snowball>();

		snowball_script.CreateSnowball(direction);
		fire_rate = initial_fire_rate;
	}
}

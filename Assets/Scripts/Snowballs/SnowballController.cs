using UnityEngine;

public class SnowballController : MonoBehaviour {
	[SerializeField] private GameObject snowball_prefab;

	public void FireSnowball(string direction) {
		var snowball = Instantiate(snowball_prefab);
		var snowball_script = snowball.GetComponent<Snowball>();

		snowball_script.CreateSnowball(direction);
	}
}

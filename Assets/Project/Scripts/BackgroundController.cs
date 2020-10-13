using UnityEngine;

public class BackgroundController : MonoBehaviour {
	/// <summary>
	/// Interactables references that control how fast
	/// they will be moving forward
	/// </summary>
	[SerializeField] private GameObject interactables;
	[SerializeField] [Range(0, 100)] private int speed;

	private void Update() {
		interactables.transform.position = new Vector3(
				0,
				interactables.transform.position.y + (speed * Time.deltaTime),
				interactables.transform.position.z);
	}
}

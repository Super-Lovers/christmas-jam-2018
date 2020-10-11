using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceController : MonoBehaviour {
	public void LoadScene(string scene_name) {
		SceneManager.LoadScene(scene_name);
	}

	public void ExitGame() {
		Application.Quit();
	}
}

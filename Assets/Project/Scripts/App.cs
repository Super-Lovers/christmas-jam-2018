using UnityEngine;

public class App : MonoBehaviour {
    private static App Instance;
    public Settings settings;

    private void Start() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static App Get() { return Instance; }
}

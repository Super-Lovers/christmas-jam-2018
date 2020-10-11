using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings", order = 1)]
public class Settings : ScriptableObject {
	public bool debug_mode;
    public bool is_paused;
}
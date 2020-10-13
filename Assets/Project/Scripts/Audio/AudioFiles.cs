using UnityEngine;

[CreateAssetMenu(fileName = "audio_files", menuName = "Scriptable Objects/Audio Files", order = 1)]
public class AudioFiles : ScriptableObject {
	public AudioClip[] audio_clips;
}

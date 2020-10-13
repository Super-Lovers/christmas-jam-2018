using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	private static AudioManager Instance;
	public AudioFiles audio_files;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public static AudioManager Get() { return Instance; }

	public void PlaySound(AudioFile file) {
		var obj = new GameObject("sound_" + file.ToString());
		var obj_audio_source = obj.AddComponent<AudioSource>();
		obj_audio_source.PlayOneShot(GetAudioFile(file));
		
		StartCoroutine(DestroyObj(obj));
	}

	public UnityEngine.AudioClip GetAudioFile(AudioFile file) {
		for (int i = 0; i < audio_files.audio_clips.Length; i++) {
			if (audio_files.audio_clips[i].audio_file == file) {
				return audio_files.audio_clips[i].audio_clip;
			}
		}

		return null;
	}

	private IEnumerator DestroyObj(GameObject obj) {
		yield return new WaitForSeconds(1f);
		Destroy(obj);
		yield return new WaitForEndOfFrame();
	}
}

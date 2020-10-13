using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	private static AudioManager Instance;
	public AudioFiles audio_files;

	/// <summary>
	/// Stores whether or not any of the sound effects
	/// are ready to be played (repeateable sound effects).
	/// </summary>
	private Dictionary<AudioFile, float> sound_timers;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	
	private void Start() {
		sound_timers = new Dictionary<AudioFile, float>();
		sound_timers[AudioFile.PlayerMove] = 0f;
	}

	public static AudioManager Get() { return Instance; }

	public void PlaySound(AudioFile file) {
		var obj = new GameObject("sound_" + file.ToString());
		var obj_audio_source = obj.AddComponent<AudioSource>();
		obj_audio_source.PlayOneShot(GetAudioFile(file));
		
		Destroy(obj, 1f);
	}

	public UnityEngine.AudioClip GetAudioFile(AudioFile file) {
		for (int i = 0; i < audio_files.audio_clips.Length; i++) {
			if (audio_files.audio_clips[i].audio_file == file) {
				return audio_files.audio_clips[i].audio_clip;
			}
		}

		return null;
	}
}

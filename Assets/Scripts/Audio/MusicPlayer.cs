using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {
	private AudioSource background;
	//private int index;

	public GameObject notMute;
	public GameObject mute;
	
	void Start() {
		background = gameObject.AddComponent<AudioSource>();
		LoadAudio(0);
	}

	private void OnLevelWasLoaded(int level) {
		LoadAudio(level);;
	}

	private void LoadAudio(int index) {
		switch (index) {
			case 0:
				Play(background, @"/Music2/Building1.mp3", AudioType.MPEG);
				break;

			case 1:
				Play(background, @"/Music2/Building1", AudioType.MPEG);
				break;

			case 2:
				Play(background, @"/Music/audio/Building1wav.wav", AudioType.WAV);
				break;

			case 3:
				Play(background, @"/Music/audio/Building3wav.wav", AudioType.WAV);
				break;
		}
	}

	private void Play(AudioSource source, string location, AudioType type) {
		source.clip = ES3.LoadAudio(Path.Combine(Application.streamingAssetsPath + location), type);
		source.volume = 0.3f;
		source.Play();
	}

	public void Mute() {
		mute.SetActive(!mute.active);
		notMute.SetActive(!notMute.active);
		background.mute = !background.mute;
	}
}

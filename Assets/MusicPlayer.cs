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
		LoadAudio(level);
		print("lol");
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.P)) background.Play();
	}


	private void LoadAudio(int index) {
		switch (index) {
			case 0:
				Play(background, @"/Music/audio/Main menu-wav.wav", AudioType.WAV);
				break;

			case 1:
				Play(background, @"/Music/audio/Building2wav.wav", AudioType.WAV);
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
		source.clip = ES3.LoadAudio(Application.streamingAssetsPath + location, type);
		source.volume = 0.1f;
		source.Play();
	}

	public void Mute() {
		mute.SetActive(!mute.active);
		notMute.SetActive(!notMute.active);
		background.mute = !background.mute;
	}
}

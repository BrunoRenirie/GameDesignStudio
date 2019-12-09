using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour {

	private AudioSource background;
    [SerializeField] private AudioMixer _AudioMixer;

	public GameObject notMute;
	public GameObject mute;

	private AudioSource muziek;
	
	void Start() {
        /*
		background = gameObject.AddComponent<AudioSource>();
		background.volume = 0.3f;
		background.loop = true;
		LoadAudio(0);
        */
    }

	public void LoadLevel(int level) {
		LoadAudio(level);

        if (mute.activeSelf)
            TempMute();
        else
            TempUnMute();
	}

	private void LoadAudio(int index) {
		switch (index) {
			case 0:
				Play(background, @"/Music/audio/Main menu-wav.wav", AudioType.WAV);
				break;

			case 1:
				Play(background, @"/Music2/Building2wav.wav", AudioType.WAV);
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
		source.Play();
	}

	public void Mute() {
		mute.SetActive(!mute.active);
		notMute.SetActive(!notMute.active);
		background.mute = !background.mute;
	}

	public void TempMute() {
		mute.SetActive(true);
		notMute.SetActive(false);

        _AudioMixer.SetFloat("MusicVolume", -80);

        /*
		muziek = GameObject.Find("Muziek").GetComponent<AudioSource>();
		muziek.mute = true;
        */
    }

    public void TempUnMute()
    {
        mute.SetActive(false);
        notMute.SetActive(true);

        _AudioMixer.SetFloat("MusicVolume", 0);

        /*
        muziek = GameObject.Find("Muziek").GetComponent<AudioSource>();
        muziek.mute = false;
        */
    }
}

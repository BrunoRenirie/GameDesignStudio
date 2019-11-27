using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour {
	private Player _player;
	private AudioSource source;
	private string[] paths;
	private string path;


	void Start() {
		path = Application.persistentDataPath + @"/Audio";
		_player = Player._Instance;
		_player.OnStateChange += PlayAudio;
		source = gameObject.AddComponent<AudioSource>();

		paths = new string[10]{
			path + @"/idle.wav",
			path + @"/move.wav",
			path + @"/jump.wav",
			path + @"/fall.wav",
			path + @"/shoot.wav",
			path + @"/duck.wav",
			path + @"/hurt.wav",
			path + @"/defeat.wav",
			path + @"/second.wav",
			path + @"/music.wav"
		};
	}

	private void PlayAudio(PlayerState state) {
		switch (state) {
			case PlayerState.idle:
				PlaySound(0);
				break;

			case PlayerState.moving:
				PlaySound(1);
				break;

			case PlayerState.jumping:
				PlaySound(2);
				break;

			case PlayerState.falling:
				PlaySound(3);
				break;

			case PlayerState.Shoot:
				PlaySound(4);
				break;

			case PlayerState.Duck:
				PlaySound(5);
				break;

			case PlayerState.Hurt:
				PlaySound(6);
				break;

			case PlayerState.Defeat:
				PlaySound(7);
				break;

			case PlayerState.SecondaryAttack:
				PlaySound(8);
				break;

		}
	}

	private void PlaySound(int index) {
		source.clip = ES3.LoadAudio(paths[index], AudioType.WAV);
		source.Play();
	}

	/*IEnumerator GetAudioClip(string location) {
		using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(location, AudioType.WAV)) {

			yield return www.SendWebRequest();

			if (www.isNetworkError) {
				Debug.Log(www.error);
			} else {
				print("location = " + location);
				clip = DownloadHandlerAudioClip.GetContent(www);
			}
		}

	}*/
}
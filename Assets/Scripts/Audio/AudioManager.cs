using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour {
	private Player _player;
	private string[] paths;
	private AudioClip clip;
	private AudioSource source;


	void Start() {
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		_player.OnStateChange += PlayAudio;
		source = gameObject.AddComponent<AudioSource>();

		paths = new string[9]{
			Application.dataPath + "/Audio/idle.wav",
			Application.dataPath + "/Audio/move.wav",
			Application.dataPath + "/Audio/jump.wav",
			Application.dataPath + "/Audio/fall.wav",
			Application.dataPath + "/Audio/shoot.wav",
			Application.dataPath + "/Audio/duck.wav",
			Application.dataPath + "/Audio/hurt.wav",
			Application.dataPath + "/Audio/defeat.wav",
			Application.dataPath + "/Audio/second.wav"
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

		StartCoroutine(GetAudioClip(paths[index]));
		source.clip = clip;
		source.Play();
	}

	IEnumerator GetAudioClip(string location) {
		using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(location, AudioType.WAV)) {

			yield return www.Send();

			if (www.isNetworkError) {
				Debug.Log(www.error);
			} else {
				clip = DownloadHandlerAudioClip.GetContent(www);
			}
		}

	}
}

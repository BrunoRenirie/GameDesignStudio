using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RecordAudio : MonoBehaviour {
	private string device;
	private AudioClip source;
	public Slider timeslide;
	public Timer time;
	public Dropdown AudioSelect;

	void Start() {
		device = GetDevice();
		time = gameObject.GetComponent<Timer>();
		time.TimerDone += StopRecord;
	}

	public void ButtonPressed() {
		ResetClip();
		RecordClip((int)timeslide.value);
	}

	private void RecordClip(int seconds) {	
		source = Record(seconds);
		time.StartTimer(seconds + 1);
	}


	private AudioClip Record(int seconds) {
		AudioClip clip;
		return clip = Microphone.Start(device, false, seconds, 44100);
		
	}

	private void StopRecord() {
		Microphone.End(device);
		SaveClip(source, GetFileName(AudioSelect));
	}

	private void SaveClip(AudioClip clip, string fileName) {
		SaveWav.Save(fileName, clip);
	}

	private string GetDevice() {
		return Microphone.devices[0];
	}

	private void ResetClip() {
		if (source == null) return;

		source = null;
	}

	private string OpenFile() {
		string location = EditorUtility.OpenFilePanel("Select file", "", "wav");
		return location;
	}

	public void CopyFile() {
		string path = Application.persistentDataPath + @"/Audio/" + GetFileName(AudioSelect) + ".wav";

		if (!File.Exists(path)) {
			FileUtil.CopyFileOrDirectory(OpenFile(), path);			
		} else {
			FileUtil.ReplaceFile(OpenFile(), path);
		}
		
	}

	private string GetFileName(Dropdown dropdown) {
		switch(dropdown.value){
			case 0:
				return "idle";

			case 1:
				return "move";

			case 2:
				return "jump";

			case 3:
				return "fall";

			case 4:
				return "shoot";

			case 5:
				return "duck";

			case 6:
				return "hurt";

			case 7:
				return "defeat";

			case 8:
				return "second";

			case 9: 
				return "music";
		}

		return null;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordUI : MonoBehaviour {
	public RecordAudio record;
	public GameObject firstState;
	public GameObject secondState;

	void Start() {
		record.ButtonPress += ChangeState;
	}

	private void ChangeState() {
		firstState.SetActive(!firstState.active);
		secondState.SetActive(!secondState.active);
	}
	
}

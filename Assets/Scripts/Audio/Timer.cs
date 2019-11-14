using System;
using UnityEngine;

public class Timer : MonoBehaviour {
	private float _seconds;
	private bool _time = false;
	public event Action TimerDone;
	
	public void StartTimer(int seconds) {
		_seconds = seconds;
		_time = true;

	}

	private void Update() {
		if (_time) {
			_seconds -= Time.deltaTime;

			if(_seconds < 0) {
				TimerDone();
				_time = false;
				_seconds = 0;
			}
		}
	}
}

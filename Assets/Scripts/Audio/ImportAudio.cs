using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImportAudio : MonoBehaviour {

	string OpenFile() {
		string location = EditorUtility.OpenFilePanel("Select file", "", "wav");
		return location;
	}

	public void CopyFile() {
		FileUtil.CopyFileOrDirectory(OpenFile(), Application.dataPath + "/Audio/");
	}
}

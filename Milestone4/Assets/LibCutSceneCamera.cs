using UnityEngine;
using System.Collections;

public class LibCutSceneCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void showHelpDialog() {
		if (GameObject.FindGameObjectWithTag("friendSpeech") )
			GameObject.FindGameObjectWithTag ("friendSpeech").SetActive (false);
	}

	void closeHelpDialog() {
		if (GameObject.FindGameObjectWithTag("friendSpeech") )
			GameObject.FindGameObjectWithTag ("friendSpeech").SetActive (false);
	}
}

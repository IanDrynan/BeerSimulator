using UnityEngine;
using System.Collections;
using UnityEngine.UI;  

public class CheckPasscode : MonoBehaviour {

	public InputField input;

	public delegate void GameWon ();
	public static event GameWon onGameWon;

	void Awake () {
		input.onEndEdit.AddListener(check);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void check(string arg0) {
		if (arg0.ToLower().Equals("natty")) {
			onGameWon ();
			gameObject.SetActive (false);
		}

	}
}

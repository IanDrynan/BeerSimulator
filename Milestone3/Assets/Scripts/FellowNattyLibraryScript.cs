using UnityEngine;
using System.Collections;

public class FellowNattyLibraryScript : MonoBehaviour {

	public Transform self; 

	public delegate void LibraryBeerSaved ();
	public static event LibraryBeerSaved onLibraryBeerSaved;

	public delegate void GameOver ();
	public static event GameOver onGameOver;

	void Awake() {
		self = transform;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("pillowCollider")) {

			gameObject.SetActive (false);

			//Trigger Event that this Beer Can was saved
			if (onLibraryBeerSaved != null) {
				onLibraryBeerSaved ();
			}

		} else if (other.gameObject.CompareTag ("floor")) {
			if (onGameOver != null) {
				onGameOver ();
			}
		}
	}
}

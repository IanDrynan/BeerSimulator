using UnityEngine;
using System.Collections;
using UnityEngine.UI;  

public class Close : MonoBehaviour {

	public Button closeButton;

	void Awake () {
		closeButton.onClick.AddListener(delegate {close();});
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void close() {
		gameObject.SetActive (false);
	}
}

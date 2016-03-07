using UnityEngine;
using System.Collections;

public class FrozenBeerScript : MonoBehaviour {

	public Transform target;
	public float moveSpeed = 0.50f;
	public float turnSpeed = 4.0f;
	public float range = 90.0f;
//	public float range2 = 45.0f;
	public float stop = 2f;
	public Transform self; 


	//Events
	public delegate void BeerSaved ();
	public static event BeerSaved onBeerSaved;

	void Awake() {
		self = transform;
	}

	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag ("magnet").transform;
	}

	void Update() {
		float distance = Vector3.Distance (self.position, target.position);
		//		if (distance <= range2 && distance >= range) {
		//			//rotate 
		//			self.rotation = Quaternion.Slerp (self.rotation, Quaternion.LookRotation (target.position - self.position), turnSpeed + Time.deltaTime);
		//		} else
		if (distance <= range && distance > stop) {
			//move to the player
			self.rotation = Quaternion.Slerp (self.rotation, Quaternion.LookRotation (target.position - self.position), turnSpeed * Time.deltaTime);
			self.position += self.forward * moveSpeed * Time.deltaTime;
		} else if (distance <= stop) {
			self.rotation = Quaternion.Slerp (self.rotation, Quaternion.LookRotation (target.position - self.position), turnSpeed * Time.deltaTime);
		}
	}

	void FixedUpdate () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("heatlamp")) {

			gameObject.SetActive (false);
	

			//Trigger Event that this Beer Can was saved
			if (onBeerSaved != null) {
				onBeerSaved();
			}

		}
	}





			
}


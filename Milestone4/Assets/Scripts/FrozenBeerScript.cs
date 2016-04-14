using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FrozenBeerScript : MonoBehaviour {

	public Transform target;
	public Transform salt = null;
	public Transform player;
	public float moveSpeed = 0.50f;
	public float turnSpeed = 4.0f;
	public float range = 90.0f;
	//	public float range2 = 45.0f;
	public float stop = 2f;
	public Transform self; 


	//Events
	public delegate void FrozenBeerSaved ();
	public static event FrozenBeerSaved onFrozenBeerSaved;

	void Awake() {
		self = transform;
	}

	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag ("magnet").transform;
//		salt = GameObject.FindWithTag("salt").transform;
		player = GameObject.FindWithTag("Player").transform;
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
			if (onFrozenBeerSaved != null) {
				onFrozenBeerSaved();
			}

		}
	}
	void LateUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				salt = player.FindChild ("salt");
				if (salt != null) //if salt child was found, do this
				{
					if(hit.transform.gameObject.tag == "frozen")
					{
						gameObject.SetActive(false);
						if (onFrozenBeerSaved != null)
						{
							onFrozenBeerSaved();
						}
					}

				}
				else
				{
					print("salt not found");
				}
			}
		}
	}






}


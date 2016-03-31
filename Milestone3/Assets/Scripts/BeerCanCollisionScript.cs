using UnityEngine;
using System.Collections;

public class BeerCanCollisionScript : MonoBehaviour {

	public Transform self;
	public Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		self = transform;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		GameObject gameObj = collision.gameObject;

//		if (!gameObj.CompareTag("movable") && gameObj.name != "Plane") {
//			Physics.IgnoreCollision (gameObj.GetComponent<Collider> (), GetComponent<Collider> ());
//		} else {
		if (gameObj.CompareTag ("movable")) {
			Rigidbody rb = collision.collider.attachedRigidbody;
			if (rb == null || rb.isKinematic)
				return;
			if (rb.CompareTag ("frozen"))
				return;
			Vector3 pushDir = new Vector3 (30, 0, 30);
			rb.velocity = pushDir * 2;
		} else {
			Debug.Log (gameObj.name);
			Physics.IgnoreCollision (gameObj.GetComponent<Collider> (), GetComponent<Collider> ());
		}
	}
}

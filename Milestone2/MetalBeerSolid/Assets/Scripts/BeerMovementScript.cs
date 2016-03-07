using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeerMovementScript : MonoBehaviour {

	//controllers
	private Animator anim;
	private CharacterController controller;
	public Transform self;

	//Movement vars
	public float speed = 4.0f;
	public float turnSpeed = 60.0f;
	private Vector3 moveDirection = Vector3.zero;
	public float gravity = 9.8f;
	public float pushPower = 2.0F;


	//jumping
	private float jumpSpeed = 7; //set to 6 eventually, just set to 9 now while developing
	private float powerUpScalar = 1; //2 for mentos
	private float vSpeed = 0;
	private ParticleSystem particleSys;

	//PowerUps/Equipped Items
	GameObject equippedItem;
	Vector3 equippedItemOrigPos;
	Vector3 equippedItemOrigSize;
	GameObject powerUp;
	private int mentosJumpsLeft = 0;

	//Beers Saved Public Var (Can be modified from other classes)
	public int numBeersSaved = 0;

	//Text
	public Text powerUpText;
	public Text beersSavedText;
	public Text winText;

	//Win Image
//	public Texture winTexture;


	void Awake () {
		self = transform;
		FrozenBeerScript.onBeerSaved += incBeersSaved;
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
		particleSys = GetComponentInChildren<ParticleSystem> ();
		displayMentosText ();
		displayBeersText ();
	}
		
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) {
			anim.SetBool("rolling", true);
		} else {
			anim.SetBool("rolling", false);
		}
		if (controller.isGrounded) {
			particleSys.Stop ();
			moveDirection = transform.forward * Input.GetAxis ("Vertical") * speed;
			float turn = Input.GetAxis ("Horizontal");
			transform.Rotate (0, turn * turnSpeed * Time.deltaTime, 0);
			if (Input.GetKey("space")) {
				
				moveDirection.y = powerUpScalar*jumpSpeed;
				particleSys.Play (); 
				if (mentosJumpsLeft > 0) {
					mentosJumpsLeft -= 1;
					if (powerUp && mentosJumpsLeft <= 0) {
						//Out of Mentos Jumps
						powerUpScalar = 1;
						powerUp.SetActive (true);
						powerUp = null;
					}
					displayMentosText ();
				}
				}
		}
			
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);


	}

	void FixedUpdate() {

	}

	void LateUpdate() {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.gameObject.tag != "frozen")
                {
                    Dequip();
                    Equip(hit.transform.gameObject);
                }
			}
		}
		else if (Input.GetMouseButtonDown(1)) {
			Dequip();
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;
		if (body.CompareTag("frozen"))
			return;
		if (hit.moveDirection.y < -0.3F)
			return;

		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = pushDir * pushPower;
	}

    void Dequip()
    {
        if (equippedItem)
        {
            equippedItem.transform.parent = null;
            equippedItem.transform.position = equippedItemOrigPos;
			equippedItem.transform.localScale = equippedItemOrigSize;
            equippedItem.tag = "Respawn";
            equippedItem = null;
            equippedItemOrigPos = Vector3.zero;
			equippedItemOrigSize = Vector3.zero;
		
        }
    }

    void Equip(GameObject item)
    {
        // attach item to bone
        equippedItem = item;
        equippedItemOrigPos = equippedItem.transform.position;
		equippedItemOrigSize = equippedItem.transform.localScale;
        equippedItem.tag = "equipped";
        equippedItem.transform.parent = self.transform;
        equippedItem.transform.localPosition = new Vector3(0.8f, 1.0f, 0.5f);
        equippedItem.transform.localRotation = Quaternion.identity;
        equippedItem.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Mentos")) {
			powerUp = other.gameObject;
			powerUpScalar *= 2;
			mentosJumpsLeft = 10;
			other.gameObject.SetActive (false);
			displayMentosText ();
		}
	}

	void displayMentosText() {
		powerUpText.text = "Mentos: " + mentosJumpsLeft.ToString ();
	}

	void displayBeersText() {
		beersSavedText.text = "Fellow Nattys: " + numBeersSaved.ToString ();
	}

	void displayWinText() {
		winText.text = "Cheers. You won!";
	}

	//Events
//	void onEnable() {//subscribes character to onBeerSaved event that is fired from FrozenBeerScript
//		FrozenBeerScript.onBeerSaved += incBeersSaved;
//	}

	void onDestroy() { //unsubscribes
		FrozenBeerScript.onBeerSaved -= incBeersSaved;
	}

	void incBeersSaved() {
		numBeersSaved += 1;
		displayBeersText ();
		displayWinText ();
	}

//	void onGUI() {
//		GUI.DrawTexture(new Rect(30, 30, 60, 60), winTexture, ScaleMode.ScaleToFit, true, 10.0F);
//	}

}
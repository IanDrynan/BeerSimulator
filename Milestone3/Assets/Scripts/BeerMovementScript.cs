using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeerMovementScript : MonoBehaviour {

	//controllers
	private Animator anim;
	private CharacterController controller;
	public Transform self;
	private GameObject speechObj;
	private Text speechText;
	private GameObject friendSpeechObj;
	private Text friendSpeechText;

    //Movement vars
    public float baseSpeed = 15.0f;
	public float speed = 15.0f;
    public float acceleration = .0833f;
    public float maxSpeed = 30.0f;
	public float turnSpeed = 60.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float gravity = 4.0f;
	public float pushPower = 2.0F;
	private float vertVel = 0;

	//jumping
	private float jumpSpeed = 14f; 
	public float powerUpScalar = 1; 
    public float mentosScalar = 1.5f;
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

	void Awake () {
		self = transform;
		FrozenBeerScript.onFrozenBeerSaved += frozenBeerSaved;
		FellowNattyLibraryScript.onLibraryBeerSaved += libraryBeerSaved;
		FellowNattyLibraryScript.onGameOver += gameOver;
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
		particleSys = GetComponentInChildren<ParticleSystem> ();
		speechObj = self.FindChild("SpeechObject").gameObject;
		speechText = speechObj.GetComponentInChildren<Text> ();
		friendSpeechObj = self.FindChild ("FriendSpeechObject").gameObject;
		friendSpeechText = friendSpeechObj.GetComponentInChildren<Text> ();
		speechObj.SetActive (false);
		displayMentosText ();
		displayBeersText ();
	}
		
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKey("w") || Input.GetKey("s")) {
			anim.SetBool("rolling", true);
            if (controller.isGrounded)
            {
                if (speed < maxSpeed)
                {
                    speed += acceleration;
                }
            }
            
		}
        else if (Input.GetKey("a") || Input.GetKey("d"))
        {
            anim.SetBool("rolling", true);
        }
        else {
			anim.SetBool("rolling", false);
            if (controller.isGrounded)
            {
                speed = baseSpeed;
            }
		}

		if (controller.isGrounded) {
			particleSys.Stop ();
			gravity = 4.0f;
		} 
			moveDirection = transform.forward * Input.GetAxis ("Vertical") * speed;
			float turn = Input.GetAxis ("Horizontal");
			transform.Rotate (0, turn * turnSpeed * Time.deltaTime, 0);
		if (controller.isGrounded && Input.GetKeyDown ("space")) {
			vertVel = jumpSpeed;
			gravity = 30f;
			particleSys.Play (); 
			if (Input.GetKey("left shift") && mentosJumpsLeft > 0) {
				StartCoroutine (say("WAHOOOOO", 2));
				mentosJumpsLeft -= 1;
				vertVel *= powerUpScalar;
				if (powerUp && mentosJumpsLeft <= 0) {
					//Out of Mentos Jumps
					powerUpScalar = 1;
					powerUp.SetActive (true);
					powerUp = null;
				}
				displayMentosText ();
			}
		} 
		vertVel -= gravity * Time.deltaTime;
		moveDirection.y = vertVel;
		controller.Move (moveDirection * Time.deltaTime);
	}

	void FixedUpdate() {
	}

	void LateUpdate() {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 7)) {
                
                string tag = hit.transform.gameObject.tag;
				if (tag == "Mentos") 
				{
					powerUp = hit.transform.gameObject;
					powerUpScalar = mentosScalar;
					mentosJumpsLeft = 10;
					hit.transform.gameObject.SetActive (false);
					displayMentosText ();
				} 
				else if (tag == "MentosT") 
				{
					powerUp = hit.transform.gameObject;
					powerUpScalar = mentosScalar;
					mentosJumpsLeft = 3;
					hit.transform.gameObject.SetActive (false);
					displayMentosText ();

					string blurb = "SICK. These mentos make me feel amazing! What would happen if I clicked Shift before I jump?";
					StartCoroutine (say (blurb, 8));
				} 
				else if (tag != "frozen" && tag != "door" && tag != "equipped" && tag != "frozenBeerDoor")
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
            equippedItem.GetComponent<Rigidbody>().isKinematic = false;
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
		string itemTag = item.tag;
		if (itemTag == "key") {
			StartCoroutine (say ("Sweet! I got a key! Now let's find the door!", 3));
			//item.gameObject.transform.FindChild ("KeyCollider").gameObject.SetActive (false);
		} else if (itemTag == "salt") {
			StartCoroutine (say ("Awesome, salt! What did my Beer School Chemistry teacher say about salt and cold weather?", 3));
		} else if (itemTag == "magnet") {
			StartCoroutine (say ("Dope, a magnet! Wonder what I could attract with this?", 3));
		}

        // attach item to bone
        equippedItem = item;
        equippedItemOrigPos = equippedItem.transform.position;
		equippedItemOrigSize = equippedItem.transform.localScale;
        equippedItem.tag = "equipped";
        equippedItem.transform.parent = self.transform;
        equippedItem.GetComponent<Rigidbody>().isKinematic = true;
        equippedItem.transform.localPosition = new Vector3(-0.8f, 1.0f, 0.5f);
        equippedItem.transform.localRotation = Quaternion.identity;
		if (equippedItem.name == "magnet") {
			equippedItem.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
		} else {
			equippedItem.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
		}


    }

	void OnTriggerEnter(Collider other) {
//		if (other.gameObject.CompareTag ("Mentos")) {
//			powerUp = other.gameObject;
//			powerUpScalar = mentosScalar;
//			mentosJumpsLeft = 10;
//			other.gameObject.SetActive (false);
//			displayMentosText ();
//		} else if (other.gameObject.CompareTag ("MentosT")) {
//			powerUp = other.gameObject;
//			powerUpScalar = mentosScalar;
//			mentosJumpsLeft = 3;
//			other.gameObject.SetActive (false);
//			displayMentosText ();
//
//			string blurb = "SICK. These mentos make me feel amazing! What would happen if I clicked Shift before I jump?";
//			StartCoroutine (say (blurb, 8));
//		} else 
		if (other.gameObject.CompareTag ("movable")) { //trigger speech blurb display using child element's capsule collider 
			GameObject otherGameObject = other.gameObject.transform.parent.gameObject;
			string nameOfObject = otherGameObject.name;
			string blurb = "Hmm, this " + nameOfObject + " seems movable. Why don't I walk up to it and push it?";
			StartCoroutine (say (blurb, 3));
			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("powerUp")) {
			GameObject otherGameObject = other.gameObject.transform.parent.gameObject;
			string nameOfObject = otherGameObject.name;
			string blurb = "Hmm, it looks like I can pick up this " + nameOfObject + ". What would happen if I walked over it?";
			StartCoroutine (say (blurb, 3));
			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("equippable")) {
			GameObject otherGameObject = other.gameObject.transform.parent.gameObject;
			string nameOfObject = otherGameObject.name;
//			string blurb = "I think I can pick that " + nameOfObject + " if I get close enough. Should I Left Click it?";
//			StartCoroutine (say (blurb, 3));
			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("heatlamp")) {
			string blurb = "Oh it's a heat lamp! Jeez Louise, it's hot, better not stay near this bad boy too long, I'm sweating!";
			StartCoroutine (say (blurb, 3));
		} else if (other.gameObject.CompareTag ("frozenText")) {
			string blurb = "Oh noes, my buddy's frozen solid!";
			StartCoroutine (say (blurb, 3));
		} else if (other.gameObject.CompareTag ("helpMe")) {
			string blurb = "HELP ME. I'M UP HERE ON TOP OF THIS BOOKSHELF!";
			StartCoroutine (friendSay (blurb, 2));
		}

	}


	void displayMentosText() {
		powerUpText.text = "Mentos Left: " + mentosJumpsLeft.ToString ();
	}

	void displayBeersText() {
		beersSavedText.text = "Nattys Found: " + numBeersSaved.ToString ();
	}

	void displayWinText(string winningText) {
		winText.text = winningText;
	}
		
	void onDestroy() { //unsubscribes
		FrozenBeerScript.onFrozenBeerSaved -= frozenBeerSaved;
		FellowNattyLibraryScript.onLibraryBeerSaved -= libraryBeerSaved;
		FellowNattyLibraryScript.onGameOver -= gameOver;

	}
	void frozenBeerSaved() {
		GameObject.FindGameObjectWithTag ("frozenBeerDoor").SetActive (false);
		numBeersSaved += 1;
		displayBeersText ();
		displayWinText ("Cheers. You saved your frozen friend! Nice job, Elsa. Your friend had stolen a key right before he was frozen and you have unlocked the next room!");
	}

	void libraryBeerSaved() {
		numBeersSaved += 1;
		displayBeersText ();
		displayWinText ("Cheers. You saved your fellow Natty!");
	}

	void gameOver() {
		winText.text = "GAME OVER, SORRY! YOU KILLED YOUR FELLOW BEER. THE FALL WAS TOO LARGE";
	}

	//display speech bubble with blurb as text for duration, time
	IEnumerator say(string blurb, float time) {
		speechText.text = blurb;
		speechObj.SetActive (true);
		yield return new WaitForSeconds(time);
		speechObj.SetActive(false);
	}

	IEnumerator friendSay(string blurb, float time) {
		friendSpeechText.text = blurb;
		friendSpeechObj.SetActive (true);
		yield return new WaitForSeconds(time);
		friendSpeechObj.SetActive(false);
	}

}

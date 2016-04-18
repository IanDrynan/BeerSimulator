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
    Transform equippedItemOrigParent;
	GameObject powerUp;
	private int mentosJumpsLeft = 0;

	//Beers Saved Public Var (Can be modified from other classes)
	public int numBeersSaved = 0;

	//Text
	public Text powerUpText;
	public Text beersSavedText;
	public Text winText;

	//Cameras
	public GameObject mainCamera;
	public GameObject cutSceneCamera;
	public GameObject cutSceneCamera2;

	//Original Positions of Objects for Checkpoint System
//	private Vector3 yourLocation = new Vector3(-88.2f, 7.9f, -58.9f);
	private Vector3 yourLocation;

	//Library Room
	private Vector3 pillowLocation = new Vector3(-152.6f, 15.78f, -50.41f);
	private Vector3 pillowRotation = new Vector3(0,0,0);
	private Vector3 friendLocation = new Vector3(-157.51f, 16.95f, -93.35f);
	private Vector3 friendRotation = new Vector3 (0, 0, 0);
	private Vector3 bookLocation = new Vector3(-157.9555f, 15.54f, -94.04861f);
	private Vector3 bookRotation = new Vector3 (0, 270, 270);

	//Science Room
	//TODO books, mentos, beercan, magnet, salt, printer, and ur location
	private Vector3 book1Loc, book2Loc, book3Loc, book4Loc, book5Loc, book6Loc, book7Loc, book8Loc, book9Loc, book10Loc, book11Loc, book12Loc, book13Loc, book14Loc, book15Loc, book16Loc, book17Loc, book18Loc, book19Loc, book20Loc;
	private Vector3 mentosScienceLoc;
	private Vector3 frozenBeerLoc;
	private Vector3 magnetLoc;
	private Vector3 saltLoc;
	private Vector3 printerLoc;

	private Vector3 book1Rot, book2Rot, book3Rot, book4Rot, book5Rot, book6Rot, book7Rot, book8Rot, book9Rot, book10Rot, book11Rot, book12Rot, book13Rot, book14Rot, book15Rot, book16Rot, book17Rot, book18Rot, book19Rot, book20Rot;
	private Vector3 mentosScienceRot;
	private Vector3 frozenBeerRot;
	private Vector3 magnetRot;
	private Vector3 saltRot;
	private Vector3 printerRot;

	private string level = "tutorial";
	private GameObject MentosScienceRoom;

	void Awake () {
		self = transform;
		FrozenBeerScript.onFrozenBeerSaved += frozenBeerSaved;
        FellowNattyComputerScript.onComputerBeerSaved += computerBeerSaved;
		FellowNattyLibraryScript.onLibraryBeerSaved += libraryBeerSaved;
		FellowNattyLibraryScript.onGameOver += gameOver;
		DoorScript.onDoorOpen += clearLevel;
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
		mainCamera = self.FindChild ("MainCamera").gameObject;
		speechObj.SetActive (false);
		displayMentosText ();
		displayBeersText ();
		MentosScienceRoom = GameObject.Find ("MentosScience");

		//Set Checkpoint Variables
		yourLocation = self.transform.position;


		//Science Room Positions
		book1Loc = GameObject.Find("Book1").transform.position;
		book2Loc = GameObject.Find("Book2").transform.position;
		book3Loc = GameObject.Find("Book3").transform.position;
		book4Loc = GameObject.Find("Book4").transform.position;
		book5Loc = GameObject.Find("Book5").transform.position;
		book6Loc = GameObject.Find("Book6").transform.position;
		book7Loc = GameObject.Find("Book7").transform.position;
		book8Loc = GameObject.Find("Book8").transform.position;
		book9Loc = GameObject.Find("Book9").transform.position;
		book10Loc = GameObject.Find("Book10").transform.position;
		book11Loc = GameObject.Find("Book11").transform.position;
		book12Loc = GameObject.Find("Book12").transform.position;
		book13Loc = GameObject.Find("Book13").transform.position;
		book14Loc = GameObject.Find("Book14").transform.position;
		book15Loc = GameObject.Find("Book15").transform.position;
		book16Loc = GameObject.Find("Book16").transform.position;
		book17Loc = GameObject.Find("Book17").transform.position;
		book18Loc = GameObject.Find("Book18").transform.position;
		book19Loc = GameObject.Find("Book19").transform.position;
		book20Loc = GameObject.Find("Book20").transform.position;
		mentosScienceLoc = GameObject.FindWithTag ("Mentos").transform.position;
		frozenBeerLoc = GameObject.Find ("FrozenBeer").transform.position;
		magnetLoc = GameObject.Find ("magnet").transform.position;
		saltLoc = GameObject.Find ("salt").transform.position;
		printerLoc = GameObject.Find ("Printer").transform.position;

		//Science Room Rotations
		book1Rot = GameObject.Find("Book1").transform.eulerAngles;
		book2Rot = GameObject.Find("Book2").transform.eulerAngles;
		book3Rot = GameObject.Find("Book3").transform.eulerAngles;
		book4Rot = GameObject.Find("Book4").transform.eulerAngles;
		book5Rot = GameObject.Find("Book5").transform.eulerAngles;
		book6Rot = GameObject.Find("Book6").transform.eulerAngles;
		book7Rot = GameObject.Find("Book7").transform.eulerAngles;
		book8Rot = GameObject.Find("Book8").transform.eulerAngles;
		book9Rot= GameObject.Find("Book9").transform.eulerAngles;
		book10Rot = GameObject.Find("Book10").transform.eulerAngles;
		book11Rot = GameObject.Find("Book11").transform.eulerAngles;
		book12Rot = GameObject.Find("Book12").transform.eulerAngles;
		book13Rot = GameObject.Find("Book13").transform.eulerAngles;
		book14Rot = GameObject.Find("Book14").transform.eulerAngles;
		book15Rot = GameObject.Find("Book15").transform.eulerAngles;
		book16Rot = GameObject.Find("Book16").transform.eulerAngles;
		book17Rot = GameObject.Find("Book17").transform.eulerAngles;
		book18Rot = GameObject.Find("Book18").transform.eulerAngles;
		book19Rot = GameObject.Find("Book19").transform.eulerAngles;
		book20Rot = GameObject.Find("Book20").transform.eulerAngles;
		mentosScienceRot = GameObject.FindWithTag ("Mentos").transform.eulerAngles;
		frozenBeerRot = GameObject.Find ("FrozenBeer").transform.eulerAngles;
		magnetRot = GameObject.Find ("magnet").transform.eulerAngles;
		saltRot = GameObject.Find ("salt").transform.eulerAngles;
		printerRot = GameObject.Find ("Printer").transform.eulerAngles;
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
			if (Physics.Raycast (ray, out hit, 7)) {
                
				string tag = hit.transform.gameObject.tag;
				if (tag == "Mentos") {
					powerUp = hit.transform.gameObject;
					powerUpScalar = mentosScalar;
					mentosJumpsLeft = 10;
					hit.transform.gameObject.SetActive (false);
					displayMentosText ();
				} else if (tag == "MentosT") {
					powerUp = hit.transform.gameObject;
					powerUpScalar = mentosScalar;
					mentosJumpsLeft = 3;
					hit.transform.gameObject.SetActive (false);
					displayMentosText ();

					string blurb = "SICK. These mentos make me feel amazing! What would happen if I clicked Shift before I jump?";
					StartCoroutine (say (blurb, 8));
				} else if (tag != "frozen" && tag != "door" && tag != "equipped" && tag != "frozenBeerDoor" && tag != "BeerSensei") {
					Dequip ();
					Equip (hit.transform.gameObject);
				} 
			}
		} else if (Input.GetMouseButtonDown (1)) {
			Dequip ();
		} else if (Input.GetKey ("0")) { //reset to last checkpoint
			self.transform.position = yourLocation;
			resetCheckpointObjects ();
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
            equippedItem.transform.parent = equippedItemOrigParent;
            //equippedItem.transform.position = equippedItemOrigPos;
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
			StartCoroutine (say ("Sweet! I got a key! Now let's find the door to use this on!", 3));
			//item.gameObject.transform.FindChild ("KeyCollider").gameObject.SetActive (false);
			GameObject.FindGameObjectWithTag("doorCollider1").SetActive(false);
		} else if (itemTag == "salt") {
			StartCoroutine (say ("Awesome, salt! What did my Beer School Chemistry teacher say about salt and cold weather?", 3));
		} else if (itemTag == "magnet") {
			StartCoroutine (say ("Dope, a magnet! Wonder what I could attract with this?", 3));
		}

        // attach item to bone
        equippedItem = item;
        equippedItemOrigParent = equippedItem.transform.parent;
        equippedItemOrigPos = equippedItem.transform.position;
		equippedItemOrigSize = equippedItem.transform.localScale;
        equippedItem.tag = "equipped";
        equippedItem.transform.parent = self.transform;
        equippedItem.GetComponent<Rigidbody>().isKinematic = true;
        equippedItem.transform.localPosition = new Vector3(-0.8f, 1.0f, 0.5f);
        equippedItem.transform.localRotation = Quaternion.identity;
		if (equippedItem.name == "magnet") {
			equippedItem.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
		}
        else if(equippedItem.name == "C++")
        {
            equippedItem.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else {
			equippedItem.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
		}
    }

	void clearLevel() {
		Dequip ();
		mentosJumpsLeft = 0;
		displayMentosText ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("movable")) { //trigger speech blurb display using child element's capsule collider 
			GameObject otherGameObject = other.gameObject.transform.parent.gameObject;
			string nameOfObject = otherGameObject.name;
			string blurb = "Hmm, this " + nameOfObject + " seems movable. Why don't I walk up to it and push it?";
			StartCoroutine (say (blurb, 3));
//			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("powerUp")) {
			GameObject otherGameObject = other.gameObject.transform.parent.gameObject;
			string nameOfObject = otherGameObject.name;
			string blurb = "Hmm, it looks like I can pick up this " + nameOfObject + ". What would happen if I clicked on it?";
			StartCoroutine (say (blurb, 2));
//			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("equippable")) {
			GameObject otherGameObject = other.gameObject.transform.parent.gameObject;
			string nameOfObject = otherGameObject.name;
			string blurb = "I think I can pick that " + nameOfObject + " if I get close enough. Should I Left Click it?";
			StartCoroutine (say (blurb, 2));
//			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("heatlamp")) {
			string blurb = "Oh it's a heat lamp! Jeez Louise, it's hot, better not stay near this bad boy too long, I'm sweating!";
			StartCoroutine (say (blurb, 3));
		} else if (other.gameObject.CompareTag ("frozenText")) {
			string blurb = "Oh noes, my buddy's frozen solid!";
			StartCoroutine (say (blurb, 3));
		} else if (other.gameObject.CompareTag ("helpMe")) {
			string blurb = "HELP ME. I'M UP HERE ON TOP OF THIS BOOKSHELF! GET ME DOWN FROM HERE! IF I HIT THE GROUND, I'LL DIE. I NEED TO LAND ON SOMETHING SOFT!";
			StartCoroutine (friendSay (blurb, 2));
//		} else if (other.gameObject.CompareTag ("doorCollider1")) {
//			string blurb = "Hmmm, this door seemed lock. Maybe there's a key somewhere.";
//			StartCoroutine (say (blurb, 2));
//			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("pillow")) {
			string blurb = "Looks like a pillow or cushion of some sort. What could that be useful for?";
			StartCoroutine (say (blurb, 2));
			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("booksCollider")) {
			string blurb = "Oh cool books! Not that I want to read them, but maybe I could shove them around or something..";
			StartCoroutine (say (blurb, 2));
			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("cutscenecollider")) {
			yourLocation = other.gameObject.transform.position;
			Debug.Log ("Hit Cut Scene Collider");
			level = "science";
			StartCoroutine (startCutScene1 ());
			other.gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("libCutSceneCollider")) {
			yourLocation = other.gameObject.transform.position;
			level = "library";
			StartCoroutine (startCutScene2 ());
			other.gameObject.SetActive (false);
		}

	}

	IEnumerator startCutScene2() {
		mainCamera.GetComponent<Camera>().enabled = false;
		cutSceneCamera.GetComponent<Camera> ().enabled = false;
		cutSceneCamera2.GetComponent<Animator> ().enabled = true;
		cutSceneCamera2.GetComponent<Camera>().enabled = true;
		cutSceneCamera2.GetComponent<Animator> ().Play("bookshelf");
		yield return new WaitForSeconds (17.5f);
		mainCamera.GetComponent<Camera>().enabled = true;
		cutSceneCamera2.GetComponent<Animator> ().enabled = false;
		cutSceneCamera2.GetComponent<Camera>().enabled = false;
	}

	IEnumerator startCutScene1() {
		mainCamera.GetComponent<Camera>().enabled = false;
		cutSceneCamera2.GetComponent<Camera> ().enabled = false;
		cutSceneCamera.GetComponent<Animator> ().enabled = true;
		cutSceneCamera.GetComponent<Camera>().enabled = true;
		cutSceneCamera.GetComponent<Animator> ().Play("libCutscene");
		yield return new WaitForSeconds (24);
		mainCamera.GetComponent<Camera>().enabled = true;
		cutSceneCamera.GetComponent<Animator> ().enabled = false;
		cutSceneCamera.GetComponent<Camera>().enabled = false;
	}


	void displayMentosText() {
		powerUpText.text = "Mentos Left: " + mentosJumpsLeft.ToString ();
	}

	void displayBeersText() {
		beersSavedText.text = "Nattys Found: " + numBeersSaved.ToString ();
	}


		
	void onDestroy() { //unsubscribes
		FrozenBeerScript.onFrozenBeerSaved -= frozenBeerSaved;
        FellowNattyComputerScript.onComputerBeerSaved -= computerBeerSaved;
		FellowNattyLibraryScript.onLibraryBeerSaved -= libraryBeerSaved;
		FellowNattyLibraryScript.onGameOver -= gameOver;
		DoorScript.onDoorOpen -= clearLevel;

	}
	void frozenBeerSaved() {
		GameObject.FindGameObjectWithTag ("frozenBeerDoor").SetActive (false);
		numBeersSaved += 1;
		displayBeersText ();
		clearLevel ();
		//displayWinText ("Cheers. You saved your frozen friend! Nice job, Elsa. Your friend had stolen a key right before he was frozen and you have unlocked the next room!");
		StartCoroutine (displayWinText ("Cheers. You saved your frozen friend! Nice job, Elsa. Your friend had stolen a key right before he was frozen and you have unlocked the next room!"));

	}

	void libraryBeerSaved() {
		Debug.Log ("Library Beer Saved");
		numBeersSaved += 1;
		displayBeersText ();
		clearLevel ();
		//displayWinText ("Cheers. You saved your fellow Natty!");
		StartCoroutine (displayWinText ("Cheers. You saved your fellow Natty!"));
	}

    void computerBeerSaved() {
        
        Debug.Log("Computer Beer Saved");
        numBeersSaved += 1;
        displayBeersText();
        clearLevel();
        //displayWinText ("Cheers. Professor Beer Joins The Party!");
        StartCoroutine(displayWinText("Cheers. Professor Beer Joins The Party!"));
    }
    void gameOver() {
		StartCoroutine (displayWinText ("GAME OVER, SORRY! YOU KILLED YOUR FELLOW BEER. THE FALL WAS TOO LARGE. Press 0 to Restart the Level!"));
		//winText.text = "GAME OVER, SORRY! YOU KILLED YOUR FELLOW BEER. THE FALL WAS TOO LARGE";
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
//
//	void displayWinText(string winningText) {
//		winText.text = winningText;
//	}
//
	IEnumerator displayWinText(string winningText) {

		winText.text = winningText;
		mentosJumpsLeft = 0;
		displayMentosText ();
		yield return new WaitForSeconds (8);
		winText.text = "";

	}

	void resetCheckpointObjects() {
		if (level == "library") {
			GameObject.Find ("Pillow").transform.position = pillowLocation;
			GameObject.Find ("Pillow").transform.eulerAngles = pillowRotation;
			GameObject.Find ("BookPerch").transform.position = bookLocation;
			GameObject.Find ("BookPerch").transform.eulerAngles = bookRotation;
			GameObject.Find ("Friend2").transform.position = friendLocation;
			GameObject.Find ("Friend2").transform.eulerAngles = friendRotation;
		} else if (level == "science") {
			
			//Reset Science Room Positions
			GameObject.Find("Book1").transform.position = book1Loc;
			GameObject.Find("Book2").transform.position = book2Loc;
			GameObject.Find("Book3").transform.position = book3Loc;
			GameObject.Find("Book4").transform.position = book4Loc;
			GameObject.Find("Book5").transform.position = book5Loc;
			GameObject.Find("Book6").transform.position = book6Loc;
			GameObject.Find("Book7").transform.position = book7Loc;
			GameObject.Find("Book8").transform.position = book8Loc;
			GameObject.Find("Book9").transform.position = book9Loc;
			GameObject.Find("Book10").transform.position = book10Loc;
			GameObject.Find("Book11").transform.position = book11Loc;
			GameObject.Find("Book12").transform.position = book12Loc;
			GameObject.Find("Book13").transform.position = book13Loc;
			GameObject.Find("Book14").transform.position = book14Loc;
			GameObject.Find("Book15").transform.position = book15Loc;
			GameObject.Find("Book16").transform.position = book16Loc;
			GameObject.Find("Book17").transform.position = book17Loc;
			GameObject.Find("Book18").transform.position = book18Loc;
			GameObject.Find("Book19").transform.position = book19Loc;
			GameObject.Find("Book20").transform.position = book20Loc;
			MentosScienceRoom.SetActive (true);
			MentosScienceRoom.transform.position = mentosScienceLoc;
			GameObject.Find ("FrozenBeer").transform.position = frozenBeerLoc;
			GameObject.Find ("magnet").transform.position = magnetLoc;
			GameObject.Find ("salt").transform.position = saltLoc;
			GameObject.Find ("Printer").transform.position = printerLoc;

			//Reset Science Room Rotations
			GameObject.Find("Book1").transform.eulerAngles = book1Rot;
			GameObject.Find("Book2").transform.eulerAngles = book2Rot;
			GameObject.Find("Book3").transform.eulerAngles = book3Rot;
			GameObject.Find("Book4").transform.eulerAngles = book4Rot;
			GameObject.Find("Book5").transform.eulerAngles = book5Rot;
			GameObject.Find("Book6").transform.eulerAngles = book6Rot;
			GameObject.Find("Book7").transform.eulerAngles = book7Rot;
			GameObject.Find("Book8").transform.eulerAngles = book8Rot;
			GameObject.Find("Book9").transform.eulerAngles = book9Rot;
			GameObject.Find("Book10").transform.eulerAngles = book10Rot;
			GameObject.Find("Book11").transform.eulerAngles = book11Rot;
			GameObject.Find("Book12").transform.eulerAngles = book12Rot;
			GameObject.Find("Book13").transform.eulerAngles = book13Rot;
			GameObject.Find("Book14").transform.eulerAngles = book14Rot;
			GameObject.Find("Book15").transform.eulerAngles = book15Rot;
			GameObject.Find("Book16").transform.eulerAngles = book16Rot;
			GameObject.Find("Book17").transform.eulerAngles = book17Rot;
			GameObject.Find("Book18").transform.eulerAngles = book18Rot;
			GameObject.Find("Book19").transform.eulerAngles = book19Rot;
			GameObject.Find("Book20").transform.eulerAngles = book20Rot;
			MentosScienceRoom.transform.eulerAngles = mentosScienceRot;
			GameObject.Find ("FrozenBeer").transform.eulerAngles = frozenBeerRot;
			GameObject.Find ("magnet").transform.eulerAngles = magnetRot;
			GameObject.Find ("salt").transform.eulerAngles = saltRot;
			GameObject.Find ("Printer").transform.eulerAngles = printerRot;
		}

	}

}

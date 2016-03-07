#pragma strict

var speed : float;
private var score : int;
public var scoreText : GUIText;
public var winText : GUIText;

function Start(){
	score = 0;
	SetCountText();
	winText.text = "";
}

function FixedUpdate () {
	var moveHorizontal = Input.GetAxis("Horizontal");
	var moveVertical = Input.GetAxis("Vertical");
	
	var movement = Vector3(moveHorizontal, 0.0f, moveVertical);
	
	GetComponent.<Rigidbody>().AddForce(movement * speed * Time.deltaTime);
}

function OnTriggerEnter(other : Collider){
	if (other.gameObject.tag == 'PickUp'){
		other.gameObject.SetActive(false);
		score ++;
		SetCountText();
	}
}

function SetCountText(){
	scoreText.text = 'Score: ' + score.ToString();
	if (score == 15)
		winText.text = "You win!";
}
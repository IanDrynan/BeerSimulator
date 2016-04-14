using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

	private Transform player;
	private Transform key;

	public delegate void DoorOpen ();
	public static event DoorOpen onDoorOpen;

	void Awake () {
		
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").transform;
		key = GameObject.FindWithTag("key").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10))
			{
				key = player.transform.Find("key");
				if (key != null) //if key child was found, do this
				{
					gameObject.SetActive(false); //delete the door
					key.gameObject.SetActive(false);
					onDoorOpen ();
				}
				else
				{
					print("key not found");
				}
			}
		}
	}
}

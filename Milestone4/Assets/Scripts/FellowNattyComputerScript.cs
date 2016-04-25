using UnityEngine;
using System.Collections;

public class FellowNattyComputerScript : MonoBehaviour
{

    public Transform self;
    public Transform book = null;
    public Transform player;
    public GameObject sensei;

    public delegate void ComputerBeerSaved();
    public static event ComputerBeerSaved onComputerBeerSaved;

    void Awake()
    {
        self = transform;
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        sensei = GameObject.FindWithTag("BeerSensei");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10))
            {
                book = player.FindChild("C++");
                if (book != null) //if C++ child was found, do this
                {
                    if (hit.transform.gameObject.tag == "BeerSensei")
                    {
                        sensei.SetActive(false);
                        if (onComputerBeerSaved != null)
                        {
                            onComputerBeerSaved();
                        }
                    }

                }
                else
                {
                    print("C++ not found");
                }
            }
        }
    }
}

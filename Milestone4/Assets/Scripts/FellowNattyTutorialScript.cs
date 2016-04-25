using UnityEngine;
using System.Collections;

public class FellowNattyTutorialScript : MonoBehaviour
{

    public Transform self;
    public GameObject tutBeer;

    public delegate void TutorialBeerSaved();
    public static event TutorialBeerSaved onTutorialBeerSaved;

    void Awake()
    {
        self = transform;
    }

    // Use this for initialization
    void Start()
    {
        tutBeer = GameObject.FindWithTag("TutorialBeer");
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
                print("this shit again");
                print(hit.transform.gameObject.tag);
                if (hit.transform.gameObject.tag == "TutorialBeer")
                {
                    tutBeer.SetActive(false);
                    if (onTutorialBeerSaved != null)
                    {
                        onTutorialBeerSaved();
                    }
                }

            }
                
          }
     }
    
}

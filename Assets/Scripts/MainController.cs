using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainController : MonoBehaviour
{
    public Camera camera;
    public Transform defense;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
		{
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
			{
                if (hit.transform.tag == "Ground")
                {
                    Instantiate(defense, hit.point, defense.rotation);
                }
			}
		}
    }
}

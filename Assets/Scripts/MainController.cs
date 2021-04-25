using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainController : MonoBehaviour
{
    public Camera camera;
    public Transform defense;
    // Start is called before the first frame update

    public Material initMaterial;
    public void CreateTurrent()
	{
        Transform tur = Instantiate(defense, Vector3.zero, defense.rotation);
        tur.FindDeepChild("Turret").GetComponent<Renderer>().material = initMaterial;
        tur.FindDeepChild("Tower").GetComponent<Renderer>().material = initMaterial;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
  //      if(Input.GetMouseButtonDown(0))
		//{
  //          Ray ray = camera.ScreenPointToRay(Input.mousePosition);
  //          RaycastHit hit;
  //          if(Physics.Raycast(ray, out hit, Mathf.Infinity, int.MaxValue, QueryTriggerInteraction.Collide))
		//	{
  //              if (hit.transform.tag == "Ground")
  //              {
  //              }
		//	}
		//}
    }
}

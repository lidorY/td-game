using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MainController : MonoBehaviour
{
    public Camera camera;
    public Transform defense;

	public Transform decal;

	public static List<Transform> placed;
	public static bool active_placing = false;


	// Start is called before the first frame update

	public Material initMaterial;
	public void CreateTurrent()
	{
		if (!active_placing)
		{
			active_placing = true;
			foreach (Transform g in placed)
			{
				g.FindDeepChild("Radius").gameObject.SetActive(true);
			}
			Transform tur = Instantiate(defense, new Vector3(-3.5f, 5, -4), defense.rotation);
			Utils.SetLayerRecursively(tur.gameObject, 6);
			tur.FindDeepChild("Turret").GetComponent<Renderer>().material = initMaterial;
			tur.FindDeepChild("Tower").GetComponent<Renderer>().material = initMaterial;
		}
		
	}

	void Start()
    {
		active_placing = false;
		placed = new List<Transform>();
	}

	private bool IsPointerOverUIObject()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	// Update is called once per frame
	void Update()
    {
		// Check if there is a touch
		if (Input.GetMouseButtonDown(0))
		{
			// Check if finger is over a UI element
			if (!IsPointerOverUIObject())
			{
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hits = Physics.RaycastAll(ray);
				
				// TODO: Check for a minimal distance from the previous hit to the ground(?)
				// Disable the option to shoot lightning all the way thru buildings..
				foreach(var hit in hits)
				{
					if(hit.transform.tag == "Ground")
					{

						Vector3 endPosition = hit.point;

						Transform decal_hit = Instantiate(decal, endPosition + (Vector3.up * 0.01f), Quaternion.identity);
						StartCoroutine(WaitAndDestroy(decal_hit.gameObject, 2f));

						StartCoroutine(WaitAndDestroy(0.1f));
						break;
					}
				}
			}	
		}
	}


	void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
	{
		
	}


	IEnumerator WaitAndDestroy(float delay) {
        yield return new WaitForSeconds(delay);
	}
	IEnumerator WaitAndDestroy(GameObject go, float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(go);
	}

}

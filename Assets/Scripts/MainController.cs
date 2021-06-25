using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MainController : MonoBehaviour
{
    public Camera camera;
    public Transform lightning;
    public Transform defense;

	public Transform decal;

	public static List<Transform> placed;
	public static bool active_placing = false;


	//public Transform laserBeam;
	//public Vector3 laserBaemOffset;

	public LineRenderer laserLineRenderer;
	public float laserWidth = 0.1f;
	public float laserMaxLength = 5f;


	


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
		//laserBeam.gameObject.SetActive(false);
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
				//Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 95);
				//Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pos);


				Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.transform.tag != "Placing")
					{

						Vector3 endPosition = hit.point;
						//Vector3 startPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 80)) + laserBaemOffset;

						// place the laserBeam in the center
						//laserBeam.position = (endPosition - startPosition) * 0.5f + startPosition;
						// rotate the laserBeam accordingly
						//laserBeam.rotation = Quaternion.FromToRotation(Vector3.up, (endPosition - startPosition));
						// Stretch laserBeam
						//laserBeam.localScale = new Vector3(laserBeam.localScale.x, (endPosition - startPosition).magnitude, laserBeam.localScale.z);

						//laserBeam.gameObject.SetActive(true);

						Transform decal_hit = Instantiate(decal, endPosition + (Vector3.up * 0.01f), Quaternion.identity);
						StartCoroutine(WaitAndDestroy(decal_hit.gameObject, 2f));

						StartCoroutine(WaitAndDestroy(0.1f));
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
		//Destroy(go);
		//laserBeam.gameObject.SetActive(false);
	}
	IEnumerator WaitAndDestroy(GameObject go, float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(go);
		//laserBeam.gameObject.SetActive(false);
	}

}

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

	GameObject Curr_placing;
	public GameObject add_button;
	public GameObject del_button;

	public HealthBar mana;
	public float mana_per_attack;
	public float mana_grow_rate;
	// Start is called before the first frame update

	public Material initMaterial;
	public void CreateTurrent()
	{
		if (!active_placing)
		{
			active_placing = true;
			placed.RemoveAll(o => o == null);

			foreach (Transform g in placed)
			{
				g.FindDeepChild("Radius").gameObject.SetActive(true);
			}
			Transform tur = Instantiate(defense, new Vector3(-3.5f, 5, -4), defense.rotation);
			Utils.SetLayerRecursively(tur.gameObject, 6);
			tur.FindDeepChild("Turret").GetComponent<Renderer>().material = initMaterial;
			tur.FindDeepChild("Tower").GetComponent<Renderer>().material = initMaterial;

			Curr_placing = tur.gameObject;
			tur.GetComponent<TurrentPlacing>().add_button = add_button;
			tur.GetComponent<TurrentPlacing>().del_button = del_button;
			add_button.SetActive(false);
			del_button.SetActive(true);
		}
		
	}

	public void DisablePlacing()
	{
		Destroy(Curr_placing);
		active_placing = false;
		add_button.SetActive(true);
		del_button.SetActive(false);
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
		mana.SetHealthRelative(-mana_grow_rate * Time.deltaTime);
		// Check if there is a touch
		if (Input.GetMouseButtonDown(0))
		{
			// Check if finger is over a UI element
			if (!IsPointerOverUIObject())
			{
				if (mana.GetHealth() >= mana_per_attack)
				{
					Ray ray = camera.ScreenPointToRay(Input.mousePosition);
					RaycastHit[] hits = Physics.RaycastAll(ray);

					// TODO: Check for a minimal distance from the previous hit to the ground(?)
					// Disable the option to shoot lightning all the way thru buildings..
					bool is_placing = false;
					foreach (var hit in hits)
					{
						if (hit.transform.tag == "Placing")
							is_placing = true;
					}

					if (!is_placing)
					{
						foreach (var hit in hits)
						{
							// placing is always on top of the screen height..

							if (hit.transform.tag == "Ground")
							{
								Vector3 endPosition = hit.point;

								Transform decal_hit = Instantiate(decal, endPosition + (Vector3.up * 0.01f), Quaternion.identity);
								StartCoroutine(WaitAndDestroy(decal_hit.gameObject, 2f));

								StartCoroutine(WaitAndDestroy(0.1f));
								mana.SetHealthRelative(mana_per_attack);
								break;
							}
						}
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

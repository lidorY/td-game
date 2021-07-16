using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentPlacing : MonoBehaviour
{


    public Transform turrent;
    public Transform pos_place;
    public Vector3 pos_offset;

   public EnemySpawner spawnerRef;

    public float clickDelta = 0.35f;  // Max between two click to be considered a double click
    private bool click = false;
    private float clickTime;

    public Material enable;
    public Material disable;


    public static uint rolling_id = 1;
    // Start is called before the first frame update
    void Start()
    {
        spawnerRef = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        prev_coll = true;

        // Placing pos position
        Vector3 dir = transform.position - Camera.main.ScreenToWorldPoint(Vector3.zero);
        float t = (0 - transform.position.y) / dir.y;

        pos_place.position = new Vector3(
            transform.position.x + t * dir.x,
            0,
            transform.position.z + t * dir.z
            );

    }

    // Update is called once per frame
    void Update()
    {
        if (click && Time.time > (clickTime + clickDelta))
        {
            click = false;
        }
            if (transform.FindDeepChild("placingpos").GetComponent<EmptyDetect>().collided != prev_coll)
        {
            prev_coll = transform.FindDeepChild("placingpos").GetComponent<EmptyDetect>().collided;
            if (prev_coll)
            {
                transform.FindDeepChild("Turret").GetComponent<Renderer>().material = disable;
                transform.FindDeepChild("Tower").GetComponent<Renderer>().material = disable;
            }
            else
            {
                transform.FindDeepChild("Turret").GetComponent<Renderer>().material = enable;
                transform.FindDeepChild("Tower").GetComponent<Renderer>().material = enable;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
      //  collided = true;
        print("Colided with: " + collision.transform.name);
    }

    private Vector3 screenPoint;
    private Vector3 offset;
    void OnMouseDown()
    {
        if (click && Time.time <= (clickTime + clickDelta))
        {
            if (!transform.FindDeepChild("placingpos").GetComponent<EmptyDetect>().collided)
            {
                Transform p = Instantiate(turrent, pos_place.position, Quaternion.identity);
                foreach(Transform t in MainController.placed)
				{
                    t.FindDeepChild("Radius").gameObject.SetActive(false);
                }
                MainController.placed.Add(p);
                p.GetComponent<BuildingPositions>().SetId(rolling_id);
                spawnerRef.AddBuilding(rolling_id, p.GetComponent<BuildingPositions>());
                p.GetComponent<BuildingPositions>().SetParent(GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>());
                rolling_id++;

                MainController.active_placing = false;
                Destroy(gameObject);
            }

            
        }
        else
        {
            click = true;
            clickTime = Time.time;
        }

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

     
    }

    private bool prev_coll;
    private void OnMouseDrag()
	{
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

        // NEED to make general based on the camera perspectives.. (currently fits camera with 45 rotations)
        float y_offset = (cursorPosition.y - transform.position.y) * 0.85f;//(curs orPosition.y - transform.position.y);
        transform.localPosition = new Vector3(cursorPosition.x + y_offset,
            transform.position.y,
            cursorPosition.z + y_offset);



        // Placing pos position
        Vector3 dir =  transform.position - Camera.main.ScreenToWorldPoint(Vector3.zero);
        float t = (0 - transform.position.y) / dir.y;

        pos_place.position = new Vector3(
            transform.position.x + t * dir.x,
            0,
            transform.position.z + t* dir.z
            );



    }
}









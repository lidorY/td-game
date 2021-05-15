using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentPlacing : MonoBehaviour
{

    public Transform turrent;
    public Vector3 pos_offset;

    public float clickDelta = 0.35f;  // Max between two click to be considered a double click
    private bool click = false;
    private float clickTime;


    public Material enable;
    public Material disable;
    // Start is called before the first frame update
    void Start()
    {
        prev_coll = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (click && Time.time > (clickTime + clickDelta))
        {
            click = false;
        }
            if (transform.FindDeepChild("Tower").GetComponent<EmptyDetect>().collided != prev_coll)
        {
            prev_coll = transform.FindDeepChild("Tower").GetComponent<EmptyDetect>().collided;
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
            print("Double click!");
            if (!transform.FindDeepChild("Tower").GetComponent<EmptyDetect>().collided)
            {
                Instantiate(turrent, transform.position + pos_offset, Quaternion.identity);
                Destroy(gameObject);
            }
            //click = false;
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
    }
}









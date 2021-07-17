using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningScorch : MonoBehaviour
{
    //public Transform decal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void OnCollisionEnter(Collision collision)
	{
  //      if (collision.transform.tag == "Ground")
		//{
  //          ContactPoint contact = collision.contacts[0];
  //          Transform decal_hit = Instantiate(decal, contact.point + (Vector3.up * 0.01f), Quaternion.identity);
  //          StartCoroutine(WaitAndDestroy(decal_hit.gameObject, 1f));
  //      }
	}

    //IEnumerator WaitAndDestroy(GameObject go, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    Destroy(go);
    //}
}

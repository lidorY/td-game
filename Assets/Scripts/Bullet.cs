using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Vector3 ShootDir;
    public void Setup(Vector3 shootDir)
	{
        ShootDir = shootDir.normalized;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += ShootDir * Time.deltaTime * 10f;
    }

	private void OnCollisionEnter(Collision collision)
	{
        Destroy(gameObject);
	}
}

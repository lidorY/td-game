using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyDetect : MonoBehaviour
{

    public bool collided;
    // Start is called before the first frame update
    void Start()
    {
        collided = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        collided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        collided = false;
    }
}

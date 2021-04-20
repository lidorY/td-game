using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{


    private bool menu_active;
    [SerializeField] private GameObject ui;

    // Start is called before the first frame update
    void Start()
    {
        menu_active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnMouseDown()
	{
        print("hellooo");
        menu_active = !menu_active;
        ui.active = menu_active;
    }
}

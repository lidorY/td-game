using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuControl : MonoBehaviour
{


    private bool menu_active;
    private bool first_click;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject radius;


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        menu_active = false;
        ui.active = false;
        menu_active = false;
        first_click = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPointerOverUIObject())
        {
            if (Input.GetMouseButtonDown(0) && menu_active)
            {
                if (!first_click)
                {
                    menu_active = false;
                    ui.active = menu_active;
                    radius.active = menu_active;
                }
                else
                {
                    first_click = false;
                }
            }
        }
    }

	private void OnMouseDown()
	{
        menu_active = !menu_active;
        ui.active = menu_active;
        radius.active = menu_active;
        first_click = true;
    }


    public void SelfDestroy()
	{
        MainController.placed.Remove(transform);
        Destroy(gameObject);
	}
}
